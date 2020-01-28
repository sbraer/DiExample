using System;
using System.IO;

namespace DiExample
{
	public enum LogType { Info, Debug, Warning, Error };

	public interface ILogs
	{
		void WriteLogLine(string message, LogType logType = LogType.Info);
		string GetLogs();
		string GetDateUtcNowString { get; }
		DateTime GetDateUtc { get; }
	}

	public class Logs : ILogs
	{
		private readonly string _fileLogWithPath;
		private readonly IConfiguration _configuration;
		private Logs() { }
		public Logs(IConfiguration configuration)
		{
			_configuration = configuration;
			_fileLogWithPath = _configuration.FileLogWithPath;
			if (_fileLogWithPath.Contains("{0:"))
			{
				_fileLogWithPath = string.Format(_fileLogWithPath, DateTime.UtcNow);
			}
		}

		public virtual string GetDateUtcNowString
		{
			get
			{
				return GetDateUtc.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}

		public virtual DateTime GetDateUtc
		{
			get
			{
				return DateTime.UtcNow;
			}
		}

		public virtual void WriteLogLine(string message, LogType logType = LogType.Info)
		{
			string text = $"{this.GetDateUtcNowString} {logType}: {message}";
#if (DEBUG)
			Console.WriteLine(text);
#endif

			lock (string.Intern(_fileLogWithPath))
			{
				File.AppendAllText(_fileLogWithPath, $"{text}{Environment.NewLine}");
			}
		}

		public virtual string GetLogs()
		{
			lock (string.Intern(_fileLogWithPath))
			{
				string allText = File.ReadAllText(_fileLogWithPath);
#if (DEBUG)
				Console.WriteLine(allText);
#endif
				return allText;
			}
		}
	}
}
