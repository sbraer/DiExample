using System.Configuration;

namespace DiExample
{
	public interface IConfiguration
	{
		string FileSourceWithPath { get; }
		string DbConnectionString { get; }
		string FileLogWithPath { get; }
		bool SkipFirstLine { get; }
		string FileSourceDateColumnFormat { get; }

		string FtpHost { get; }
		string FtpUsername { get; }
		string FtpPassword { get; }
		string FtpFileNameWithPath { get; }

		int RowsNumberBuffer { get; }

		string SqlInsertString { get; }
	}

	public class Configuration : IConfiguration
	{
		private readonly IHelper _helper;
		private int? _RowsNumberBuffer;
		private bool? _SkipFirstLine;

		private Configuration() { }
		public Configuration(Helper helper)
		{
			_helper = helper;
		}

		public string FileSourceWithPath
		{
			get
			{
				return ConfigurationManager.AppSettings["FileSourceWithPath"];
			}
		}

		public string DbConnectionString
		{
			get
			{
				return ConfigurationManager.AppSettings["DbConnectionString"];
			}
		}
		public string FileLogWithPath
		{
			get
			{
				return ConfigurationManager.AppSettings["FileLogWithPath"];
			}
		}
		public string FileSourceDateColumnFormat
		{
			get
			{
				return ConfigurationManager.AppSettings["FileSourceDateColumnFormat"];
			}
		}

		public bool SkipFirstLine
		{
			get
			{
				if (_SkipFirstLine.HasValue)
				{
					return _SkipFirstLine.Value;
				}

				_SkipFirstLine = _helper.GetBoolValue(ConfigurationManager.AppSettings["SkipFirstLine"], false);
				return _SkipFirstLine.Value;
			}
		}
		public string FtpHost
		{
			get
			{
				return ConfigurationManager.AppSettings["FtpHost"];
			}
		}
		public string FtpUsername
		{
			get
			{
				return ConfigurationManager.AppSettings["FtpUsername"];
			}
		}
		public string FtpPassword
		{
			get
			{
				return ConfigurationManager.AppSettings["FtpPassword"];
			}
		}

		public string FtpFileNameWithPath
		{
			get
			{
				return ConfigurationManager.AppSettings["FtpFileNameWithPath"];
			}
		}

		public int RowsNumberBuffer
		{
			get
			{
				if (_RowsNumberBuffer.HasValue)
				{
					return _RowsNumberBuffer.Value;
				}

				_RowsNumberBuffer = _helper.GetIntValue(ConfigurationManager.AppSettings["RowsNumberBuffer"], 1);
				return _RowsNumberBuffer.Value;
			}
		}

		public string SqlInsertString
		{
			get
			{
				return ConfigurationManager.AppSettings["SqlInsertString"];
			}
		}
	}
}
