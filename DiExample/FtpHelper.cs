using FluentFTP;
using System;
using System.Net;

namespace DiExample
{
	public interface IFtpService
	{
		void DownloadFile(string remotePathAndFile, string localPathAndFile);
	}

	public class FtpService : IFtpService, IDisposable
	{
		private readonly IFtpClient _ftpClient;
		private readonly IConfiguration _configuration;
		private bool disposed = false;

		private FtpService() { }

		public FtpService(IFtpClient ftpClient, IConfiguration configuration)
		{
			_ftpClient = ftpClient;
			_configuration = configuration;
		}

		public virtual void DownloadFile(string remotePathAndFile, string localPathAndFile)
		{
			_ftpClient.Host = _configuration.FtpHost;
			_ftpClient.Credentials = new NetworkCredential(_configuration.FtpUsername, _configuration.FtpPassword);

			try
			{
				// begin connecting to the server
				_ftpClient.Connect();
				_ftpClient.DownloadFile(localPathAndFile, remotePathAndFile);
			}
			catch (Exception ex)
			{
				throw new FtpException(ex.Message);
			}
			finally
			{
				_ftpClient.Disconnect();
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
				_ftpClient.Dispose();
			}

			disposed = true;
		}

		~FtpService()
		{
			Dispose(false);
		}
	}

	public class FtpException : Exception
	{
		public FtpException(string message) : base(message)
		{
		}

		public FtpException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public FtpException()
		{
		}
	}
}
