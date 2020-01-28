using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiExampleTest
{
	[TestClass]
	public class DbHelperTest
	{
		[TestMethod]
		public void InsertRows()
		{
			var myLogs = new MyDbHelperLogs();
			var db = new DbHelper(new MyDbHelperDal(), myLogs, new MyDbHelperConfiguration());
			var coll = new List<string>(new string[] {
				"a1,01/01/2001",
				"b1,30/11/2002",
				"c1,12/12/2003,", // <- Wrong split
				"d1,10/30/2004" // <- Wrong date
			});

			db.InsertRows(coll);
			var logText = myLogs.GetLogs();
			Assert.AreEqual(logText, File.ReadAllText(@"Fixtures\DbHelperTest1.txt"));
		}
	}

	#region Support class
	public class MyDbHelperDal : IDal
	{
		public void InsertRows(List<RowObject> coll)
		{
			Assert.AreEqual(2, coll.Count);
			Assert.AreEqual("a1", coll[0].Name);
			Assert.AreEqual("b1", coll[1].Name);
			Assert.AreEqual(new DateTime(2001, 1, 1), coll[0].Date);
			Assert.AreEqual(new DateTime(2002, 11, 30), coll[1].Date);
		}
	}

	public class MyDbHelperLogs : ILogs
	{
		private readonly StringBuilder _sb;
		public MyDbHelperLogs()
		{
			_sb = new StringBuilder();
		}
		public string GetDateUtcNowString => "2019-01-01";

		public DateTime GetDateUtc => throw new NotImplementedException();

		public string GetLogs()
		{
			return _sb.ToString();
		}

		public void WriteLogLine(string message, LogType logType = LogType.Info)
		{
			_sb.Append($"{logType}: {message}\r\n");
		}
	}

	public class MyDbHelperConfiguration : IConfiguration
	{
		public MyDbHelperConfiguration()
		{
		}

		public string FileSourceDateColumnFormat => "dd/MM/yyyy";

		#region NotImplemented
		public string FileSourceWithPath => throw new NotImplementedException();

		public string DbConnectionString => throw new NotImplementedException();

		public string FileLogWithPath => throw new NotImplementedException();

		public bool SkipFirstLine => throw new NotImplementedException();


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
