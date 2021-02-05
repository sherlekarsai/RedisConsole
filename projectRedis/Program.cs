using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectRedis
{
    class Program
    {
        static void Main(string[] args)
        {
             CacheStrigsStack _cacheStrigsStack  = new CacheStrigsStack(); 
            var key = "Name";

            List<abc> lstabc = new List<abc>();
            lstabc.Add(new abc() { Id = 1, Name = "sai" });
            lstabc.Add(new abc() { Id = 2, Name = "Shardul" });
            lstabc.Add(new abc() { Id = 3, Name = "suresh" });

            _cacheStrigsStack.SetStrings(key, "sai1");
            _cacheStrigsStack.StoreList<List<abc>>("name1", lstabc, new TimeSpan(0,5,0));
         var ff =    _cacheStrigsStack.GetList<List<abc>>("name1");

            var dd = _cacheStrigsStack.GetStrings(key);
        }
    }
    public class abc
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CacheStrigsStack
    {
        private readonly RedisEndpoint _redisEndpoint;
        public CacheStrigsStack()
        {
            var host = "localhost";
            var port = 6379;
            _redisEndpoint = new RedisEndpoint(host, port);
        }

        public bool IsKeyExists(string key)
        {
            using (var redisClient = new RedisClient(_redisEndpoint))
            {
                if (redisClient.ContainsKey(key))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SetStrings(string key, string value)
        {
            using (var redisClient = new RedisClient(_redisEndpoint))
            {
                redisClient.SetValue(key, value);
            }
        }

        public string GetStrings(string key)
        {
            using (var redisClient = new RedisClient(_redisEndpoint))
            {
                return redisClient.GetValue(key);
            }
        }

        public bool StoreList<T>(string key, T value, TimeSpan timeout)
        {
            try
            {
                using (var redisClient = new RedisClient(_redisEndpoint))
                {
                    redisClient.As<T>().SetValue(key, value, timeout);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T GetList<T>(string key)
        {
            T result;

            using (var client = new RedisClient(_redisEndpoint))
            {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }

        public long Increment(string key)
        {
            using (var client = new RedisClient(_redisEndpoint))
            {
                return client.Increment(key, 1);
            }
        }

        public long Decrement(string key)
        {
            using (var client = new RedisClient(_redisEndpoint))
            {
                return client.Decrement(key, 1);
            }
        }
    }
}
