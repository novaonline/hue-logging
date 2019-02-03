using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Standard.Models.Interfaces
{
	public interface IBasicBinarySerializer
	{
		byte[] To<T>(T value);

		T From<T>(byte[] serializedValue);
	}
}
