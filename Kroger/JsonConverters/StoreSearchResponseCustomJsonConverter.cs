using Czf.ApiWrapper.Kroger.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger
{
    public class StoreSearchResponseCustomJsonConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return typeof(StoreSearchResponse) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            if(token.Type != JTokenType.Object) { return null; }

            token = token.SelectToken("data.storeSearch");
            if (token.Type != JTokenType.Object) { return null; }

            return token.ToObject<StoreSearchResponse>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
