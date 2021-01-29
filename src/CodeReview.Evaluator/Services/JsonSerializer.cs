using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ReviewItEasy.Evaluator.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
        }
    }
}
