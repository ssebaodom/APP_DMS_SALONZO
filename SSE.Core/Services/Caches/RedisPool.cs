//using Microsoft.Extensions.Configuration;
////using ServiceStack.Redis;
//using SSE.Core.Common.Constants;

//namespace SSE.Core.Services.Caches
//{
//    public class RedisPool
//    {
//        //private readonly RedisManagerPool redisManagerPool;

//        //public RedisPool(IConfiguration configuration)
//        //{
//        //    try
//        //    {
//        //        var connects = configuration.GetSection(CONFIGURATION_KEYS.REDIS);
//        //        string redisConnect = connects.GetValue<string>(CONFIGURATION_KEYS.REDIS_HOST);
//        //        this.redisManagerPool = new RedisManagerPool(redisConnect, new RedisPoolConfig { MaxPoolSize = 1000 });
//        //    }
//        //    catch (System.Exception)
//        //    {
//        //        throw new System.Exception(CORE_STRINGS.REDIS_CONNECT_ERROR);
//        //    }
//        //}

//        //public IRedisClient createRedisClient()
//        //{
//        //    return this.redisManagerPool.GetClient();
//        //}
//    }
//}