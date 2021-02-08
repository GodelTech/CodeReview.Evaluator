using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, Settings);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(json));
            
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}