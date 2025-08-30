using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SSE.Core.Services.Caches
{
    public interface ICached
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool Set<T>(string key, T value, TimeSpan? timeSpan = null);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeSpan = null);
        bool Remove(string key);
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 10/10/2023 tiennq
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expired"></param>
        /// <returns></returns>
        //bool Set<T>(string key, T value, TimeSpan expired);

        bool Set<T>(string key, T value);

        //T Get<T>(string key);

        bool ContainsKey(string key);

        long Count(string key);

        IEnumerable<string> Keys();

        //bool Remove(string key);

        void AddToList<T>(string listId, T value);

        void AddRangeToList<T>(string listId, List<T> value);

        bool RemoveItemFromList<T>(string listId, T value);

        void EditItemFromList<T>(string listId, T oldValue, T newValue);

        List<T> GetList<T>(string listId);

        void ResetList<T>(string listId);

        //void Close();
    }
}