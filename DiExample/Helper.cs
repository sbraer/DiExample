using System;

namespace DiExample
{
	public interface IHelper
	{
		bool GetBoolValue(string stringValue, bool defaultValue);
		int GetIntValue(string stringValue, int defaultValue);
	}

	public class Helper : IHelper
	{
		public int GetIntValue(string stringValue, int defaultValue)
		{
			if (string.IsNullOrEmpty(stringValue))
			{
				return defaultValue;
			}

			if (int.TryParse(stringValue, out int value))
			{
				return value;
			}

			throw new HelperException($"'{stringValue}' cannot be converted in Int value");
		}
		public bool GetBoolValue(string stringValue, bool defaultValue)
		{
			if (string.IsNullOrEmpty(stringValue))
			{
				return defaultValue;
			}

			if (bool.TryParse(stringValue, out bool value))
			{
				return value;
			}

			throw new HelperException($"'{stringValue}' cannot be converted in Bool value");
		}
	}

	public class HelperException : Exception
	{
		public HelperException(string message) : base(message)
		{
		}

		public HelperException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
