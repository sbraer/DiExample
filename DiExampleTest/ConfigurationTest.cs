using DiExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiExampleTest
{
	[TestClass]
	public class ConfigurationTest
	{
		[DataTestMethod]
		[DataRow("FileSourceWithPath", "myfile.txt")]
		[DataRow("DbConnectionString", "connectionString")]
		[DataRow("FileLogWithPath", "Logs_{0:yyyy-MM-dd}.txt")]
		[DataRow("FileSourceDateColumnFormat", "dd/mm/yyyy")]
		[DataRow("FtpHost", "127.0.0.1")]
		[DataRow("FtpUsername", "username")]
		[DataRow("FtpPassword", "password")]
		[DataRow("FtpFileNameWithPath", "example.txt")]
		[DataRow("SqlInsertString", "insert into [MyTable]([name],[date]) values(@name, @date)")]
		public void TestAppConfigString(string propertyName, string value)
		{
			IConfiguration conf = new Configuration(new Helper());
			Assert.AreEqual(value, conf.GetType().GetProperty(propertyName).GetValue(conf, null).ToString());
		}

		[DataTestMethod]
		[DataRow("SkipFirstLine", true)]
		public void TestAppConfigBool(string propertyName, bool value)
		{
			IConfiguration conf = new Configuration(new Helper());
			bool value2 = bool.Parse(conf.GetType().GetProperty(propertyName).GetValue(conf, null).ToString());
			Assert.AreEqual(value2, value);
		}

		[DataTestMethod]
		[DataRow("RowsNumberBuffer", 1000)]
		public void TestAppConfigInt(string propertyName, int value)
		{
			IConfiguration conf = new Configuration(new Helper());
			int value2 = int.Parse(conf.GetType().GetProperty(propertyName).GetValue(conf, null).ToString());
			Assert.AreEqual(value2, value);
		}
	}
}
