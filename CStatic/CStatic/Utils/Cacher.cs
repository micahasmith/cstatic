using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Utils
{
    public class Cacher
    {
        public static object Proxy(string key, Func<object> gen)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            if (cache[key] != null)
                return cache[key];
            var val = gen();
            cache[key] = val;
            return val;

        }

        public static void Set(string key, object o)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            cache[key] = o;
        }

        public static object Get(string key)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            return cache[key];
        }
    }
}
