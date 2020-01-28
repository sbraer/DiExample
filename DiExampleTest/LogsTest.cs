using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using DiExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Match = System.Text.RegularExpressions.Match;

namespace DiExampleTest
{
	[TestClass]
	public class LogsTest
	{
		[TestMethod]
		public void WriteLogLine()
		{
			var myConfiguration = new MyLogsConfiguration();

			// Reset cache file
			File.WriteAllText(myConfiguration.FileLogWithPath, string.Empty);

			var logs = new Mock<Logs>(myConfiguration) { CallBase = true };
			logs.Setup(t => t.GetDateUtcNowString).Returns("2000-01-01 12.12.12");

			logs.Object.WriteLogLine("line1");
			logs.Object.WriteLogLine("line2", LogType.Debug);
			logs.Object.WriteLogLine("line3", LogType.Error);
			logs.Object.WriteLogLine("line4", LogType.Info);
			logs.Object.WriteLogLine("line5", LogType.Warning);

			string allLogs = logs.Object.GetLogs();
			Assert.AreEqual(allLogs, File.ReadAllText("Fixtures/LogsTest1.txt"));

			File.WriteAllText(myConfiguration.FileLogWithPath, string.Empty);
		}

		[TestMethod]
		public void TestDateTime()
		{
			var myConfiguration = new MyLogsConfiguration();
			var logs = new Logs(myConfiguration);
			string dt = logs.GetDateUtcNowString;

			Regex regex = new Regex(@"^\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d$");
			Match match = regex.Match(dt);
			Assert.IsTrue(match.Success, $"Expected date format: yyyy-MM-dd HH:mm:ss but property returns: {dt}");
		}

		[TestMethod]
		public void TestDateTimeExact()
		{
			var myConfiguration = new MyLogsConfiguration();
			var logs = new Logs(myConfiguration);
			Assert.IsTrue(DateTime.TryParseExact(logs.GetDateUtcNowString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _), "GetDateUtcNowString doesn't returns yyyy-MM-dd HH:mm:ss format");
		}

		[DataTestMethod]
		[DataRow(2019, 1, 11, 8, 40, 30, "2019-01-11 08:40:30")]
		[DataRow(2019, 10, 1, 18, 40, 30, "2019-10-01 18:40:30")]
		[DataRow(2009, 11, 12, 13, 0, 5, "2009-11-12 13:00:05")]
		public void TestDateTimeValue(int year, int month, int day, int hour, int minute, int second, string dtCompare)
		{
			var dt = new DateTime(year, month, day, hour, minute, second);

			var myConfiguration = new MyLogsConfiguration();
			var logs = new Mock<Logs>(myConfiguration) { CallBase = true };
			logs.Setup(t => t.GetDateUtc).Returns(dt);

			Assert.AreEqual(logs.Object.GetDateUtcNowString, dtCompare);
		}
	}

	#region Support class
	public class MyLogsConfiguration : IConfiguration
	{
		private const string CacheLogFile = "Cache\\logs-file.txt";
		public string FileLogWithPath => CacheLogFile;

		#region Not Implemented
		public string FileSourceWithPath => throw new NotImplementedException();

		public string DbConnectionString => throw new NotImplementedException();


		public bool SkipFirstLine => throw new NotImplementedException();

		public string FileSourceDateColumnFormat => throw new NotImplementedException();

		public string FtpHost => throw new NotImplementedException();

		public string FtpUsername => throw new NotImplementedException();

		public string FtpPassword => throw new NotImplementedException();

		public string FtpFileNameWithPath => throw new NotImplementedException();

		public int RowsNumberBuffer => throw new NotImplementedException();

		public string SqlInsertString => throw new NotImplementedException();
		#endregion
	}
	#endregion
}
