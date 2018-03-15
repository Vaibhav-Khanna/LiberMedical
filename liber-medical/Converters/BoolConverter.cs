using System;
using Newtonsoft.Json;

namespace libermedical.Converters
{
	public class BoolConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return reader.Value ?? false;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool);
		}
	}

	public class TupleConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
            if(reader.Value!=null)
            {
                return new Tuple<string, bool>(reader.Value.ToString(), true);
            }
			return null;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}

    public class BoolSyncConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value ?? true;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
