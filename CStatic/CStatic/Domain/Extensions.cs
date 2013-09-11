using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public static class Extensions
    {
        public static string GetArg(this IEnumerable<string> args,string name)
        {
            if (args == null || args.Count() == 0) return null;
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                if (arg.StartsWith(name+":"))
                {
                    return arg.Split(':')[1].Trim();
                }
            }
            return null;
        }

        public static IEnumerable<KeyValuePair<string,string>> GetArgs(this IEnumerable<string> args)
        {
            if (args == null || args.Count() == 0) yield break;
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                if (arg.Contains(":"))
                {
                    var b = arg.Split(':');
                    yield return new KeyValuePair<string,string>(b[0].Trim(), b[1].Trim());
                }
            }
            yield break;
        }

        public static Dictionary<string, string> AsDictionary(this IEnumerable<KeyValuePair<string, string>> args)
        {
            var r = new Dictionary<string,string>();
            foreach (var arg in args)
                r[arg.Key] = arg.Value;
            return r;
        }

        public static void MixIn(this Dictionary<string, string> me, Dictionary<string, string> other)
        {
            if (other == null) return;
            foreach (var k in other.Keys)
            {
                me[k] = other[k];
            }
        }

        public static Dictionary<string, string> MixInAndReturn(this Dictionary<string, string> me, Dictionary<string, string> other)
        {
            me.MixIn(other);
            return me;
        }
    }
}
