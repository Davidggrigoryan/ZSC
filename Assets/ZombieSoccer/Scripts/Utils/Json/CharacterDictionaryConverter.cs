using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.ZombieSoccer.Scripts.Utils.Json
{
    public class CharacterDictionaryConverter : JsonConverter<Characters>
    {
        public override void WriteJson(JsonWriter writer, Characters value, JsonSerializer serializer)
        {
            writer.WriteValue(value); // TODO: Verify this works
        }

        public override Characters ReadJson(JsonReader reader, Type objectType, Characters existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            Dictionary<string, Character> charactersDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, Character>>(jObject.ToString());
            
            return new Characters()
            {
                list = charactersDictionary
            };
        }
    }
}