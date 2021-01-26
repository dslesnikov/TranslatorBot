using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TranslatorBot.Json
{
    #nullable enable
    public class PolicyBasedJsonEnumConverter : JsonConverterFactory
    {
        private readonly JsonNamingPolicy _policy;
        
        public PolicyBasedJsonEnumConverter(JsonNamingPolicy policy)
        {
            _policy = policy;
        }
        
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = (JsonConverter?)Activator.CreateInstance(
                typeof(PolicyBasedEnumConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new object?[] {_policy},
                null);
            return converter;
        }
    }
}