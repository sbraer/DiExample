using DiExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiExampleTest
{
	[TestClass]
	public class HelperTest
	{
		private readonly IHelper _helper;
		public HelperTest()
		{
			_helper = new Helper();
		}

		[DataTestMethod]
		[DataRow("true", false, true)]
		[DataRow("false", true, false)]
		[DataRow(" true", false, true)]
		[DataRow(" false", true, false)]
		[DataRow("true ", false, true)]
		[DataRow("false ", true, false)]
		[DataRow("  true ", false, true)]
		[DataRow("  false ", true, false)]
		[DataRow("True", false, true)]
		[DataRow("False", true, false)]
		[DataRow("TRUE", false, true)]
		[DataRow("FALSE", true, false)]
		[DataRow("", false, false)]
		[DataRow("", true, true)]
		public void TestGetBoolValue(string fromValue, bool defaultValue, bool finalValue)
		{
			Assert.AreEqual(_helper.GetBoolValue(fromValue, defaultValue), finalValue);
		}

		[DataTestMethod]
		[DataRow("abc")]
		[DataRow("123")]
		[DataRow(" ")]
		[DataRow("1")]
		[DataRow("0")]
		[DataRow("true;")]
		[DataRow("'false'")]
		[DataRow("\"false\"")]
		public void TestGetBoolValueException(string fromValue)
		{
			Assert.ThrowsException<HelperException>(()=> _helper.GetBoolValue(fromValue, false));
		}

		[DataTestMethod]
		[DataRow("a1", "'a1' cannot be converted in Bool value")]
		public void TestGetBoolValueExceptionString(string fromValue, string msg)
		{
			try
			{
				_helper.GetBoolValue(fromValue, false);
				Assert.Fail("Method doesn't throw Exception");
			}
			catch (HelperException ex)
			{
				Assert.AreEqual(ex.Message, msg);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Method doesn't throw correct Exception {ex.Message}");
			}
		}

		[DataTestMethod]
		[DataRow("1", 0, 1)]
		[DataRow("-3", 0, -3)]
		[DataRow(" 1", 0, 1)]
		[DataRow(" -8", 0, -8)]
		[DataRow("4 ", 0, 4)]
		[DataRow("-4 ", 0, -4)]
		[DataRow(" 2 ", 0, 2)]
		[DataRow(" -7 ", 0, -7)]
		[DataRow("", 3, 3)]
		[DataRow("", -3, -3)]
		public void TestGetIntValue(string fromValue, int defaultValue, int finalValue)
		{
			Assert.AreEqual(_helper.GetIntValue(fromValue, defaultValue), finalValue);
		}

		[DataTestMethod]
		[DataRow("a1")]
		[DataRow("-1a")]
		[DataRow("a12")]
		[DataRow("q2z")]
		[DataRow(" ")]
		public void TestGetIntValueException(string fromValue)
		{
			Assert.ThrowsException<HelperException>(() => _helper.GetIntValue(fromValue, 0));
		}

		[DataTestMethod]
		[DataRow("a1", "'a1' cannot be converted in Int value")]
		public void TestGetIntValueExceptionString(string fromValue, string msg)
		{
			try
			{
				_helper.GetIntValue(fromValue, 0);
				Assert.Fail("Method doesn't throw Exception");
			}
			catch(HelperException ex)
			{
				Assert.AreEqual(ex.Message, msg);
			}
			catch (Exception ex)
			{
				Assert.Fail($"Method doesn't throw correct Exception {ex.Message}");
			}
		}
	}
}
