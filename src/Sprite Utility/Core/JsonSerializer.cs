using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Boxer.Core
{

    public static class JsonSerializer
    {
        private static readonly JsonSerializerSettings Settings;

        static JsonSerializer()
        {
            Settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        public static string Serialize<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value, Settings);
            return json;
        }



        public static T Deserialize<T>(string json)
        {
            var instance = JsonConvert.DeserializeObject<T>(json, Settings);
            return instance;
        }
    }
}
