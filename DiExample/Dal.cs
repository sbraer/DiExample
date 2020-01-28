using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DiExample
{
	public interface IDal
	{
		void InsertRows(List<RowObject> coll);
	}

	public class Dal : IDal, IDisposable
	{
		private readonly IDbConnection _connection;
		private readonly IDbCommand _insertCommand;
		private readonly IConfiguration _configuration;
		private readonly SqlParameter _insertParameter1, _insertParameter2;
		private bool disposed = false;

		private Dal() { }
		public Dal(IDbConnection connection, IDbCommand insertCommand, IConfiguration configuration)
		{
			_connection = connection;
			_insertCommand = insertCommand;
			_configuration = configuration;

			_connection.ConnectionString = _configuration.DbConnectionString;
			_insertCommand.Connection = _connection;

			_insertParameter1 = new SqlParameter("@name", SqlDbType.VarChar, 50);
			_insertParameter2 = new SqlParameter("@date", SqlDbType.DateTime);
			_insertCommand.CommandType = CommandType.Text;
			_insertCommand.CommandText = _configuration.SqlInsertString;
			_insertCommand.Parameters.Add(_insertParameter1);
			_insertCommand.Parameters.Add(_insertParameter2);
			_insertCommand.Prepare();
		}
		public virtual void InsertRows(List<RowObject> coll)
		{
			try
			{
				_connection.Open();

				foreach (var item in coll)
				{
					_insertParameter1.Value = item.Name;
					_insertParameter2.Value = item.Date;
					_insertCommand.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				throw new DalException(ex.Message);
			}
			finally
			{
				_connection.Close();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				_insertCommand.Dispose();
				_connection.Dispose();
			}

			disposed = true;
		}

		~Dal()
		{
			Dispose(false);
		}
	}
	public class DalException : Exception
	{
		public DalException(string message) : base(message)
		{
		}

		public DalException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public DalException()
		{
		}
	}
}
