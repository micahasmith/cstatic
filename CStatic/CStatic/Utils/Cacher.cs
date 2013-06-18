using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Utils
{
    public class Cacher
    {

        private static Dictionary<string,object> _Cache  = new Dictionary<string,object>();

        public static object Proxy(string key, Func<object> gen)
        {
            var cache = _Cache;
            if (cache.ContainsKey(key))
                return cache[key];
            var val = gen();
            cache[key] = val;
            return val;

        }

        public static void Set(string key, object o)
        {
            _Cache[key] = o;
        }

        public static object Get(string key)
        {
            if(_Cache.ContainsKey(key))
                return _Cache[key];
            return null;
        }
    }
}
