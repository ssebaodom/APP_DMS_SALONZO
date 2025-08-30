using Microsoft.Extensions.Configuration;
//using ServiceStack.Redis;
using StackExchange.Redis;
using SSE.Core.Common.Constants;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SSE.Core.Services.Caches
{
    //public class RedisCached : ICached
    //{
    //    private readonly RedisManagerPool redisManagerPool;
    //    private readonly IRedisClient redisClient;

    //    public RedisCached(IConfiguration configuration)
    //    {
    //        try
    //        {
    //            var connects = configuration.GetSection(CONFIGURATION_KEYS.REDIS);
    //            string redisConnect = connects.GetSection(CONFIGURATION_KEYS.REDIS_HOST).Value;
    //            this.redisManagerPool = new RedisManagerPool(redisConnect, new RedisPoolConfig { MaxPoolSize = 1000 });
    //            this.redisClient = redisManagerPool.GetClient();
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception(CORE_STRINGS.REDIS_CONNECT_ERROR);
    //        }
    //    }

    //    public void Close()
    //    {
    //        this.redisClient.Dispose();
    //        this.redisManagerPool.Dispose();
    //    }

    //    public bool ContainsKey(string key)
    //    {
    //        return this.redisClient.ContainsKey(key);
    //    }

    //    public long Count(string key)
    //    {
    //        return this.redisClient.GetStringCount(key);
    //    }

    //    public T Get<T>(string key)
    //    {
    //        return this.redisClient.Get<T>(key);
    //    }

    //    public IEnumerable<string> Keys()
    //    {
    //        return this.redisClient.GetAllKeys();
    //    }

    //    public bool Remove(string key)
    //    {
    //        return this.redisClient.Remove(key);
    //    }

    //    public bool Set<T>(string key, T value, TimeSpan expired)
    //    {
    //        return this.redisClient.Set(key, value, expired);
    //    }

    //    public bool Set<T>(string key, T value)
    //    {
    //        return this.redisClient.Set(key, value);
    //    }

    //    public void AddToList<T>(string listId, T value)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        redis.Lists[listId].Add(value);
    //    }

    //    public void AddRangeToList<T>(string listId, List<T> value)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        redis.Lists[listId].AddRange(value);
    //    }

    //    public bool RemoveItemFromList<T>(string listId, T value)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        return redis.Lists[listId].Remove(value);
    //    }

    //    public void EditItemFromList<T>(string listId, T oldValue, T newValue)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        redis.Lists[listId].Remove(oldValue);
    //        redis.Lists[listId].Add(newValue);
    //    }

    //    public List<T> GetList<T>(string listId)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        return redis.Lists[listId].GetAll();
    //    }

    //    public void ResetList<T>(string listId)
    //    {
    //        var redis = this.redisClient.As<T>();
    //        redis.Lists[listId].Clear();
    //    }
    //}

    public class RedisCached : ICached
    {
        private static ConnectionMultiplexer _connectionMultiplexer;
        private static readonly object _lock = new object();
        private readonly IDatabase _database;
        private readonly IConfiguration _config;

        public RedisCached(IConfiguration config)
        {
            _config = config;
            _database = GetDatabase("Redis_Default");
        }

        /// <summary>
        /// Đảm bảo chỉ có một ConnectionMultiplexer duy nhất
        /// </summary>
        private ConnectionMultiplexer GetConnect(IConfigurationSection redisConfig)
        {
            if (_connectionMultiplexer == null)
            {
                lock (_lock)
                {
                    if (_connectionMultiplexer == null)
                    {
                        var connStr = redisConfig["Connection"];
                        _connectionMultiplexer = ConnectionMultiplexer.Connect(connStr);
                    }
                }
            }
            return _connectionMultiplexer;
        }

        /// <summary>
        /// Kiểm tra và lấy cấu hình Redis
        /// </summary>
        private IConfigurationSection CheckConfig(string configName)
        {
            IConfigurationSection redisConfig = _config.GetSection("RedisConfig").GetSection(configName);
            if (redisConfig == null)
            {
                throw new ArgumentNullException($"Không tìm thấy cấu hình Redis cho {configName}");
            }
            return redisConfig;
        }

        /// <summary>
        /// Lấy database từ Redis
        /// </summary>
        private IDatabase GetDatabase(string configName = null, int? db = null)
        {
            IConfigurationSection redisConfig = CheckConfig(configName);
            int defaultDb = db ?? 0;

            var strDefaultDatabase = redisConfig["DefaultDatabase"];
            if (!string.IsNullOrEmpty(strDefaultDatabase) && int.TryParse(strDefaultDatabase, out var intDefaultDatabase))
            {
                defaultDb = intDefaultDatabase;
            }

            return GetConnect(redisConfig).GetDatabase(defaultDb);
        }

        /// <summary>
        /// Kiểm tra key có tồn tại trong cache không
        /// </summary>
        public bool ContainsKey(string key)
        {
            return _database.KeyExists(key);
        }

        /// <summary>
        /// Lấy số lượng phần tử trong danh sách Redis
        /// </summary>
        public long Count(string key)
        {
            return _database.ListLength(key);
        }

        /// <summary>
        /// Lấy giá trị từ cache
        /// </summary>
        public T Get<T>(string key)
        {
            string value = _database.StringGet(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Lấy giá trị từ cache (async)
        /// </summary>
        public async Task<T> GetAsync<T>(string key)
        {
            string value = await _database.StringGetAsync(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Xóa một key khỏi cache
        /// </summary>
        public bool Remove(string key)
        {
            return _database.KeyDelete(key);
        }

        /// <summary>
        /// Set giá trị vào cache với thời gian hết hạn
        /// </summary>
        public bool Set<T>(string key, T value, TimeSpan? expired = null)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            return _database.StringSet(key, serializedValue, expired);
        }

        /// <summary>
        /// Set giá trị vào cache không có thời gian hết hạn
        /// </summary>
        public bool Set<T>(string key, T value)
        {
            return Set(key, value, null);
        }

        /// <summary>
        /// Set giá trị vào cache với thời gian hết hạn (async)
        /// </summary>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan? expired = null)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            return _database.StringSetAsync(key, serializedValue, expired);
        }

        /// <summary>
        /// Thêm một item vào danh sách Redis
        /// </summary>
        public void AddToList<T>(string listId, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            _database.ListLeftPush(listId, serializedValue);
        }

        /// <summary>
        /// Thêm nhiều item vào danh sách Redis
        /// </summary>
        public void AddRangeToList<T>(string listId, List<T> values)
        {
            var serializedValues = values.ConvertAll(value => (RedisValue)JsonConvert.SerializeObject(value));
            _database.ListLeftPush(listId, serializedValues.ToArray());
        }

        /// <summary>
        /// Xóa một item khỏi danh sách Redis
        /// </summary>
        public bool RemoveItemFromList<T>(string listId, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            return _database.ListRemove(listId, serializedValue) > 0;
        }

        /// <summary>
        /// Chỉnh sửa một item trong danh sách Redis
        /// </summary>
        public void EditItemFromList<T>(string listId, T oldValue, T newValue)
        {
            string oldSerializedValue = JsonConvert.SerializeObject(oldValue);
            string newSerializedValue = JsonConvert.SerializeObject(newValue);
            _database.ListRemove(listId, oldSerializedValue);
            _database.ListLeftPush(listId, newSerializedValue);
        }

        /// <summary>
        /// Lấy danh sách từ Redis
        /// </summary>
        public List<T> GetList<T>(string listId)
        {
            var serializedValues = _database.ListRange(listId);
            return serializedValues.Select(value => JsonConvert.DeserializeObject<T>(value)).ToList();
        }

        /// <summary>
        /// Xóa toàn bộ danh sách
        /// </summary>
        public void ResetList<T>(string listId)
        {
            _database.KeyDelete(listId);
        }

        /// <summary>
        /// Đóng kết nối Redis
        /// </summary>
        public void Close()
        {
            _connectionMultiplexer?.Dispose();
        }

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Keys()
        {
            throw new NotImplementedException();
        }
    }
}