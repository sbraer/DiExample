using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using DiExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiExampleTest
{
	[TestClass]
	public class DalTest
	{
		[TestMethod]
		public void InsertRows()
		{
			var myCommand = new MyDalDbCommand();
			using (var dal = new Dal(new MyDalDbConnection(), myCommand, new MyDalConfiguration()))
			{
				var pars = myCommand.Parameters;
				Assert.AreEqual(2, myCommand.Parameters.Count);
				Assert.AreEqual("@name", ((SqlParameter)myCommand.Parameters[0]).ParameterName);
				Assert.AreEqual("@date", ((SqlParameter)myCommand.Parameters[1]).ParameterName);
				Assert.AreEqual(SqlDbType.VarChar, ((SqlParameter)myCommand.Parameters[0]).SqlDbType);
				Assert.AreEqual(50, ((SqlParameter)myCommand.Parameters[0]).Size);
				Assert.AreEqual(SqlDbType.DateTime, ((SqlParameter)myCommand.Parameters[1]).SqlDbType);

				var coll = new List<RowObject>(new RowObject[] {
					new RowObject("a1", new DateTime(2011,1,1)),
					new RowObject("a2", new DateTime(2012,2,2))
					});

				dal.InsertRows(coll);
			}
		}

		[TestMethod]
		public void InsertRowsException()
		{
			var myCommand = new MyDalDbCommand();
			using (var dal = new Dal(new MyDalDbConnection(), myCommand, new MyDalConfiguration()))
			{
				var coll = new List<RowObject>(new RowObject[] {
					new RowObject("a1", new DateTime(2011,1,2))
					});

				try
				{
					dal.InsertRows(coll);
					Assert.Fail("coll inserted correctly but I'm wait exception");
				}
				catch (DalException ex)
				{
					Assert.AreEqual(ex.Message, "Assert.Fail failed. Wrong parameters: a1 1/2/2011 12:00:00 AM");
				}
			}
		}
	}

	#region Support class
	public class MyDalDbConnection : IDbConnection
	{
		private bool IsOpened = false;
		public string ConnectionString { get => "myconnectionstring"; set => Assert.AreEqual(value, "myconnectionstring"); }

		public void Open()
		{
			Assert.AreEqual(IsOpened, false);
			IsOpened = true;
		}

		public void Close()
		{
			Assert.AreEqual(IsOpened, true);
			IsOpened = false;
		}

		public void Dispose()
		{
			Assert.AreEqual(IsOpened, false, "Connection is open must be closed");
		}

		#region NotImplemented
		public int ConnectionTimeout => throw new NotImplementedException();

		public string Database => throw new NotImplementedException();

		public ConnectionState State => throw new NotImplementedException();

		public IDbTransaction BeginTransaction()
		{
			throw new NotImplementedException();
		}

		public IDbTransaction BeginTransaction(IsolationLevel il)
		{
			throw new NotImplementedException();
		}

		public void ChangeDatabase(string databaseName)
		{
			throw new NotImplementedException();
		}

		public IDbCommand CreateCommand()
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class MyDalDbCommand : IDbCommand
	{
		bool _isCompiled;
		public MyDalDbCommand()
		{
			ConstructorInfo ctor = typeof(SqlParameterCollection).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
			SqlParameterCollection abc = (SqlParameterCollection)ctor.Invoke(new object[] { });
			this.Parameters = abc;
			_isCompiled = false;
		}

		public IDbConnection Connection { get => throw new NotImplementedException(); set => _ = value; }
		public CommandType CommandType { get => throw new NotImplementedException(); set => Assert.AreEqual(CommandType.Text, value); }
		public string CommandText { get => throw new NotImplementedException(); set => Assert.AreEqual("insert into mytable", value); }
		public IDataParameterCollection Parameters { get; }
		public int ExecuteNonQuery()
		{
			Assert.AreEqual(_isCompiled, true);
			var par1 = ((SqlParameter)this.Parameters[0]).Value;
			var par2 = ((SqlParameter)this.Parameters[1]).Value;
			if (par1.ToString() == "a1" && DateTime.Parse(par2.ToString()) == new DateTime(2011, 1, 1))
			{
				return 1;
			}

			if (par1.ToString() == "a2" && DateTime.Parse(par2.ToString()) == new DateTime(2012, 2, 2))
			{
				return 1;
			}

			Assert.Fail($"Wrong parameters: {par1} {par2}");
			return 0;
		}

		public void Dispose()
		{ }

		public void Prepare()
		{
			_isCompiled = true;
		}

		#region NotImplemented
		public IDbTransaction Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Cancel()
		{
			throw new NotImplementedException();
		}

		public IDbDataParameter CreateParameter()
		{
			throw new NotImplementedException();
		}

		public IDataReader ExecuteReader()
		{
			throw new NotImplementedException();
		}

		public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			throw new NotImplementedException();
		}

		public object ExecuteScalar()
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class MyDalConfiguration : IConfiguration
	{
		public string DbConnectionString => "myconnectionstring";
		public string SqlInsertString => "insert into mytable";

		#region NotImplemented
		public string FileSourceWithPath => throw new NotImplementedException();

		public string FileLogWithPath => throw new NotImplementedException();

		public bool SkipFirstLine => throw new NotImplementedException();

		public string FileSourceDateColumnFormat => throw new NotImplementedException();

		public string FtpHost => throw new NotImplementedException();

		public string FtpUsername => throw new NotImplementedException();

		public string FtpPassword => throw new NotImplementedException();

		public string FtpFileNameWithPath => throw new NotImplementedException();

		public int RowsNumberBuffer => throw new NotImplementedException();

		#endregion
	}
	#endregion
}
