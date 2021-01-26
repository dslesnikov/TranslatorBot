using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TranslatorBot.Json
{
    #nullable enable
    public class PolicyBasedEnumConverter<T> : JsonConverter<T>
        where T : Enum
    {
        private static readonly Dictionary<string, T> Members = new();
        private static readonly Dictionary<T, string> MemberNames = new();

        public PolicyBasedEnumConverter(JsonNamingPolicy policy)
        {
            var values = typeof(T).GetEnumValues().Cast<T>();
            foreach (var enumValue in values)
            {
                var valueName = policy.ConvertName(enumValue.ToString());
                Members[valueName] = enumValue;
                MemberNames[enumValue] = valueName;
            }
        }
        
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();
            if (enumString == null)
            {
                return default;
            }
            if (Members.TryGetValue(enumString, out var value))
            {
                return value;
            }
            throw new JsonException($"Неподдерживаемое значение перечисления {enumString} для перечисления {typeof(T).FullName}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(MemberNames[value]);
        }
    }
}