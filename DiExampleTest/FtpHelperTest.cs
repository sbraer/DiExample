using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiExample;
using FluentFTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiExampleTest
{
	[TestClass]
	public class FtpHelperTest
	{
		private readonly IFtpService _ftp;
		public FtpHelperTest()
		{
			_ftp = new FtpService(new MyFtpService1(), new MyConfiguration1());
		}

		[TestInitialize]
		public void StartEverySingleTest() { }

		[TestCleanup]
		public void CleanAfterEveryTest() { }

		[TestMethod]
		public void DownloadFile()
		{
			_ftp.DownloadFile("/filename.txt", "/local/filename.txt");
		}

		[TestMethod]
		public void TestException()
		{
			try
			{
				_ftp.DownloadFile("/filename2.txt", "/local/filename2.txt");
				Assert.Fail("File was downloaded but must returns generic error");
			}
			catch (DiExample.FtpException ex)
			{
				Assert.AreEqual(ex.Message, "Wrong parameter in test", "Wrong exception message");
			}
		}

		[TestMethod]
		public void DisposeObject()
		{
			_ftp.DownloadFile("/filename.txt", "/local/filename.txt");
		}
	}

	#region Support class
	public class MyFtpService1 : IFtpClient
	{

		public string Host { get; set; }
		public bool IsConnected { get; set; } = false;
		public NetworkCredential Credentials { get; set; }

		public void Connect()
		{
			Assert.AreEqual(this.Host, "myftphost.com");
			Assert.AreEqual(this.Credentials.UserName, "myusername");
			Assert.AreEqual(this.Credentials.Password, "mypassword");
			this.IsConnected = true;
		}

		public void Disconnect()
		{
			Assert.AreEqual(this.IsConnected, true);
			this.IsConnected = false;
		}

		public void Dispose()
		{
			Assert.AreEqual(this.IsConnected, false, "Connection is open but must be closed");
		}

		public bool DownloadFile(string localPath, string remotePath, FtpLocalExists existsMode = FtpLocalExists.Overwrite, FtpVerify verifyOptions = FtpVerify.None, Action<FtpProgress> progress = null)
		{
			Assert.AreEqual(this.IsConnected, true);
			if (localPath == "/local/filename.txt" && remotePath == "/filename.txt")
			{
				return true;
			}

			if (localPath == "/local/filename2.txt" && remotePath == "/filename2.txt")
			{
				throw new DiExample.FtpException("Wrong parameter in test");
			}

			Assert.Fail($"Wrong parameters: {localPath} {remotePath}");
			return false;
		}

		#region NotImplemented
		public bool IsDisposed => throw new NotImplementedException();

		public FtpIpVersion InternetProtocolVersions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int SocketPollInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool StaleDataCheck { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


		public bool EnableThreadSafeDataConnections { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Encoding Encoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int Port { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int MaximumDereferenceCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public X509CertificateCollection ClientCertificates => throw new NotImplementedException();

		public Func<string> AddressResolver { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public IEnumerable<int> ActivePorts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public FtpDataConnectionType DataConnectionType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool UngracefullDisconnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int ConnectTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int ReadTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int DataConnectionConnectTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int DataConnectionReadTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool SocketKeepAlive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public List<FtpCapability> Capabilities => throw new NotImplementedException();

		public FtpHashAlgorithm HashAlgorithms => throw new NotImplementedException();

		public FtpEncryptionMode EncryptionMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool DataConnectionEncryption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public SslProtocols SslProtocols { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public string SystemType => throw new NotImplementedException();

		public string ConnectionType => throw new NotImplementedException();

		public FtpParser ListingParser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public CultureInfo ListingCulture { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public double TimeOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool RecursiveList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool BulkListing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int BulkListingLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int TransferChunkSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int RetryAttempts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public uint UploadRateLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public uint DownloadRateLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public FtpDataType UploadDataType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public FtpDataType DownloadDataType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#pragma warning disable CS0067
		public event FtpSslValidation ValidateCertificate;
#pragma warning restore CS0067

		public FtpProfile AutoConnect()
		{
			throw new NotImplementedException();
		}

		public Task<FtpProfile> AutoConnectAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public List<FtpProfile> AutoDetect(bool firstOnly)
		{
			throw new NotImplementedException();
		}

		public void Chmod(string path, int permissions)
		{
			throw new NotImplementedException();
		}

		public void Chmod(string path, FtpPermission owner, FtpPermission group, FtpPermission other)
		{
			throw new NotImplementedException();
		}

		public Task ChmodAsync(string path, int permissions, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task ChmodAsync(string path, FtpPermission owner, FtpPermission group, FtpPermission other, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void Connect(FtpProfile profile)
		{
			throw new NotImplementedException();
		}

		public Task ConnectAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task ConnectAsync(FtpProfile profile, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void CreateDirectory(string path)
		{
			throw new NotImplementedException();
		}

		public void CreateDirectory(string path, bool force)
		{
			throw new NotImplementedException();
		}

		public Task CreateDirectoryAsync(string path, bool force, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task CreateDirectoryAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void DeleteDirectory(string path)
		{
			throw new NotImplementedException();
		}

		public void DeleteDirectory(string path, FtpListOption options)
		{
			throw new NotImplementedException();
		}

		public Task DeleteDirectoryAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task DeleteDirectoryAsync(string path, FtpListOption options, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void DeleteFile(string path)
		{
			throw new NotImplementedException();
		}

		public Task DeleteFileAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpListItem DereferenceLink(FtpListItem item)
		{
			throw new NotImplementedException();
		}

		public FtpListItem DereferenceLink(FtpListItem item, int recMax)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem> DereferenceLinkAsync(FtpListItem item, int recMax, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem> DereferenceLinkAsync(FtpListItem item, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool DirectoryExists(string path)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DirectoryExistsAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void DisableUTF8()
		{
			throw new NotImplementedException();
		}

		public Task DisconnectAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool Download(Stream outStream, string remotePath, long restartPosition, Action<FtpProgress> progress = null)
		{
			throw new NotImplementedException();
		}

		public bool Download(out byte[] outBytes, string remotePath, long restartPosition, Action<FtpProgress> progress = null)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DownloadAsync(Stream outStream, string remotePath, long restartPosition = 0, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<byte[]> DownloadAsync(string remotePath, long restartPosition = 0, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DownloadFileAsync(string localPath, string remotePath, FtpLocalExists existsMode = FtpLocalExists.Overwrite, FtpVerify verifyOptions = FtpVerify.None, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public int DownloadFiles(string localDir, IEnumerable<string> remotePaths, FtpLocalExists existsMode = FtpLocalExists.Overwrite, FtpVerify verifyOptions = FtpVerify.None, FtpError errorHandling = FtpError.None)
		{
			throw new NotImplementedException();
		}

		public Task<int> DownloadFilesAsync(string localDir, IEnumerable<string> remotePaths, FtpLocalExists existsMode = FtpLocalExists.Overwrite, FtpVerify verifyOptions = FtpVerify.None, FtpError errorHandling = FtpError.None, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpReply Execute(string command)
		{
			throw new NotImplementedException();
		}

		public Task<FtpReply> ExecuteAsync(string command, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool FileExists(string path)
		{
			throw new NotImplementedException();
		}

		public Task<bool> FileExistsAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpHash GetChecksum(string path)
		{
			throw new NotImplementedException();
		}

		public Task<FtpHash> GetChecksumAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public int GetChmod(string path)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetChmodAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpListItem GetFilePermissions(string path)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem> GetFilePermissionsAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public long GetFileSize(string path)
		{
			throw new NotImplementedException();
		}

		public Task<long> GetFileSizeAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpHash GetHash(string path)
		{
			throw new NotImplementedException();
		}

		public FtpHashAlgorithm GetHashAlgorithm()
		{
			throw new NotImplementedException();
		}

		public Task<FtpHashAlgorithm> GetHashAlgorithmAsync()
		{
			throw new NotImplementedException();
		}

		public Task<FtpHash> GetHashAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpListItem[] GetListing()
		{
			throw new NotImplementedException();
		}

		public FtpListItem[] GetListing(string path)
		{
			throw new NotImplementedException();
		}

		public FtpListItem[] GetListing(string path, FtpListOption options)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem[]> GetListingAsync(string path, FtpListOption options, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem[]> GetListingAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem[]> GetListingAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetMD5(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetMD5Async(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public DateTime GetModifiedTime(string path, FtpDate type = FtpDate.Original)
		{
			throw new NotImplementedException();
		}

		public Task<DateTime> GetModifiedTimeAsync(string path, FtpDate type = FtpDate.Original, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string[] GetNameListing()
		{
			throw new NotImplementedException();
		}

		public string[] GetNameListing(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string[]> GetNameListingAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<string[]> GetNameListingAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public FtpListItem GetObjectInfo(string path, bool dateModified = false)
		{
			throw new NotImplementedException();
		}

		public Task<FtpListItem> GetObjectInfoAsync(string path, bool dateModified = false)
		{
			throw new NotImplementedException();
		}

		public FtpReply GetReply()
		{
			throw new NotImplementedException();
		}

		public Task<FtpReply> GetReplyAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetWorkingDirectory()
		{
			throw new NotImplementedException();
		}

		public Task<string> GetWorkingDirectoryAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetXCRC(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetXCRCAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetXMD5(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetXMD5Async(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetXSHA1(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetXSHA1Async(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetXSHA256(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetXSHA256Async(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public string GetXSHA512(string path)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetXSHA512Async(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool HasFeature(FtpCapability cap)
		{
			throw new NotImplementedException();
		}

		public bool MoveDirectory(string path, string dest, FtpExists existsMode = FtpExists.Overwrite)
		{
			throw new NotImplementedException();
		}

		public Task<bool> MoveDirectoryAsync(string path, string dest, FtpExists existsMode = FtpExists.Overwrite, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool MoveFile(string path, string dest, FtpExists existsMode = FtpExists.Overwrite)
		{
			throw new NotImplementedException();
		}

		public Task<bool> MoveFileAsync(string path, string dest, FtpExists existsMode = FtpExists.Overwrite, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Stream OpenAppend(string path)
		{
			throw new NotImplementedException();
		}

		public Stream OpenAppend(string path, FtpDataType type)
		{
			throw new NotImplementedException();
		}

		public Stream OpenAppend(string path, FtpDataType type, bool checkIfFileExists)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenAppendAsync(string path, FtpDataType type, bool checkIfFileExists, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenAppendAsync(string path, FtpDataType type, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenAppendAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, FtpDataType type)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, FtpDataType type, bool checkIfFileExists)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, FtpDataType type, long restart)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, long restart)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, long restart, bool checkIfFileExists)
		{
			throw new NotImplementedException();
		}

		public Stream OpenRead(string path, FtpDataType type, long restart, bool checkIfFileExists)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string path, FtpDataType type, long restart, bool checkIfFileExists, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string path, FtpDataType type, long restart, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string path, FtpDataType type, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string path, long restart, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Stream OpenWrite(string path)
		{
			throw new NotImplementedException();
		}

		public Stream OpenWrite(string path, FtpDataType type)
		{
			throw new NotImplementedException();
		}

		public Stream OpenWrite(string path, FtpDataType type, bool checkIfFileExists)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenWriteAsync(string path, FtpDataType type, bool checkIfFileExists, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenWriteAsync(string path, FtpDataType type, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenWriteAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void Rename(string path, string dest)
		{
			throw new NotImplementedException();
		}

		public Task RenameAsync(string path, string dest, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void SetFilePermissions(string path, int permissions)
		{
			throw new NotImplementedException();
		}

		public void SetFilePermissions(string path, FtpPermission owner, FtpPermission group, FtpPermission other)
		{
			throw new NotImplementedException();
		}

		public Task SetFilePermissionsAsync(string path, int permissions, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task SetFilePermissionsAsync(string path, FtpPermission owner, FtpPermission group, FtpPermission other, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void SetHashAlgorithm(FtpHashAlgorithm type)
		{
			throw new NotImplementedException();
		}

		public Task SetHashAlgorithmAsync(FtpHashAlgorithm type)
		{
			throw new NotImplementedException();
		}

		public void SetModifiedTime(string path, DateTime date, FtpDate type = FtpDate.Original)
		{
			throw new NotImplementedException();
		}

		public Task SetModifiedTimeAsync(string path, DateTime date, FtpDate type = FtpDate.Original, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public void SetWorkingDirectory(string path)
		{
			throw new NotImplementedException();
		}

		public Task SetWorkingDirectoryAsync(string path, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool Upload(Stream fileStream, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, Action<FtpProgress> progress = null)
		{
			throw new NotImplementedException();
		}

		public bool Upload(byte[] fileData, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, Action<FtpProgress> progress = null)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UploadAsync(Stream fileStream, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UploadAsync(byte[] fileData, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public bool UploadFile(string localPath, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, FtpVerify verifyOptions = FtpVerify.None, Action<FtpProgress> progress = null)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UploadFileAsync(string localPath, string remotePath, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = false, FtpVerify verifyOptions = FtpVerify.None, IProgress<FtpProgress> progress = null, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public int UploadFiles(IEnumerable<string> localPaths, string remoteDir, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = true, FtpVerify verifyOptions = FtpVerify.None, FtpError errorHandling = FtpError.None)
		{
			throw new NotImplementedException();
		}

		public int UploadFiles(IEnumerable<FileInfo> localFiles, string remoteDir, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = true, FtpVerify verifyOptions = FtpVerify.None, FtpError errorHandling = FtpError.None)
		{
			throw new NotImplementedException();
		}

		public Task<int> UploadFilesAsync(IEnumerable<string> localPaths, string remoteDir, FtpExists existsMode = FtpExists.Overwrite, bool createRemoteDir = true, FtpVerify verifyOptions = FtpVerify.None, FtpError errorHandling = FtpError.None, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class MyConfiguration1 : IConfiguration
	{
		public string FtpHost => "myftphost.com";

		public string FtpUsername => "myusername";

		public string FtpPassword => "mypassword";

		#region NotImplemented
		public string FileSourceWithPath => throw new NotImplementedException();

		public string DbConnectionString => throw new NotImplementedException();

		public string FileLogWithPath => throw new NotImplementedException();

		public bool SkipFirstLine => throw new NotImplementedException();

		public string FileSourceDateColumnFormat => throw new NotImplementedException();
		public string FtpFileNameWithPath => throw new NotImplementedException();

		public int RowsNumberBuffer => throw new NotImplementedException();

		public string SqlInsertString => throw new NotImplementedException();
		#endregion
	}
	#endregion
}

