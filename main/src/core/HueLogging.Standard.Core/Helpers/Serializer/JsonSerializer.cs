using HueLogging.Standard.Models.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace HueLogging.Standard.Library.Helpers.Serializer
{
	public class JsonSerializer : IBasicBinarySerializer
	{
		public T From<T>(byte[] serializedValue)
		{
			var s = Encoding.ASCII.GetString(serializedValue);
			return JsonConvert.DeserializeObject<T>(s);
		}

		public byte[] To<T>(T value)
		{
			var s = JsonConvert.SerializeObject(value);
			return Encoding.ASCII.GetBytes(s);
		}
	}
}
