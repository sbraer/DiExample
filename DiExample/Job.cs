using System;
using System.Collections.Generic;
using System.IO;

namespace DiExample
{
	public class Job
	{
		private readonly IConfiguration _configuration;
		private readonly IDbHelper _dbHelper;
		private readonly IFtpService _ftpService;
		private readonly ILogs _logs;
		private Job() { }
		public Job(IConfiguration configuration, IDbHelper dbHelper, IFtpService ftpService, ILogs logs)
		{
			_configuration = configuration;
			_dbHelper = dbHelper;
			_ftpService = ftpService;
			_logs = logs;
		}

		public void Start()
		{
			_logs.WriteLogLine("Start application");

			try
			{
				_logs.WriteLogLine($"FileSourceWithPath= {_configuration.FileSourceWithPath}");
				_logs.WriteLogLine($"DbConnectionString= {_configuration.DbConnectionString}");
				_logs.WriteLogLine($"FileLogWithPath= {_configuration.FileLogWithPath}");
				_logs.WriteLogLine($"FtpHost= {_configuration.FtpHost}");
				_logs.WriteLogLine($"FtpUsername= {_configuration.FtpUsername}");
				_logs.WriteLogLine($"FtpPassword= {_configuration.FtpPassword}");

				_ftpService.DownloadFile(_configuration.FtpFileNameWithPath, _configuration.FileSourceWithPath);
				var collBuffer = new List<string>();
				using (var file = new StreamReader(_configuration.FileSourceWithPath))
				{
					string line;
					bool firstLine = true;
					while ((line = file.ReadLine()) != null)
					{
						_logs.WriteLogLine(line, LogType.Debug);
						if (firstLine)
						{
							firstLine = false;
							if (_configuration.SkipFirstLine)
							{
								_logs.WriteLogLine("Line skipped", LogType.Debug);
							}
						}
						else
						{
							collBuffer.Add(line);
							if (collBuffer.Count >= _configuration.RowsNumberBuffer)
							{
								_dbHelper.InsertRows(collBuffer);
								collBuffer.Clear();
							}
						}
					}

					if (collBuffer.Count != 0)
					{
						_dbHelper.InsertRows(collBuffer);
					}
				}
			}
			catch (HelperException ex)
			{
				_logs.WriteLogLine($"From Helper: {ex.Message}", LogType.Error);
			}
			catch (FtpException ex)
			{
				_logs.WriteLogLine($"From FtpService: {ex.Message}", LogType.Error);
			}
			catch (DalException ex)
			{
				_logs.WriteLogLine($"From Dal: {ex.Message}", LogType.Error);
			}
			catch (Exception ex)
			{
				_logs.WriteLogLine($"{ex.Message}", LogType.Error);
			}

			_logs.WriteLogLine("End application");
		}
	}
}
