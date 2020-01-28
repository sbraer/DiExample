using System;
using System.Collections.Generic;
using System.Globalization;

namespace DiExample
{
	public interface IDbHelper
	{
		void InsertRows(List<string> rows);
	}

	public class DbHelper : IDbHelper
	{
		private readonly IDal _dal;
		private readonly ILogs _logs;
		private readonly IConfiguration _configuration;
		public DbHelper(IDal dal, ILogs logs, IConfiguration configuration)
		{
			_dal = dal;
			_logs = logs;
			_configuration = configuration;
		}
		public virtual void InsertRows(List<string> rows)
		{
			var collBuffer = new List<RowObject>();
			foreach (var row in rows)
			{
				string[] columns = row.Split(',');
				if (columns.Length != 2)
				{
					_logs.WriteLogLine($"Wrong split (length != 2)", LogType.Warning);
				}
				else
				{
					string name = columns[0];
					if (DateTime.TryParseExact(columns[1], _configuration.FileSourceDateColumnFormat, CultureInfo.InvariantCulture,
										 DateTimeStyles.None, out DateTime dateValue))
					{
						collBuffer.Add(new RowObject(name, dateValue));
					}
					else
					{
						_logs.WriteLogLine($"Wrong date value", LogType.Warning);
					}
				}
			}

			_dal.InsertRows(collBuffer);
		}
	}
}
