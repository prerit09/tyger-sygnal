using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TygerSygnal
{
    class Utils
    {
        public static IEnumerable<T> SplitTo<T>(string str)
        {
            return str
                .Trim(';')
                .Split(';')
                .Select(token => (T)Activator.CreateInstance(typeof(T), token));
        }

        public static string ToJson(object obj, Formatting formatting = Formatting.Indented)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(obj, formatting, settings);
            return json;
        }
    }
}
