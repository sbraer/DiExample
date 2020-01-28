using FluentFTP;
using System.Data;
using System.Data.SqlClient;
using Unity;
using Unity.Lifetime;

namespace DiExample
{
	class Program
	{
		static void Main(string[] _)
		{
			IUnityContainer container = new UnityContainer();
			container.RegisterType<IConfiguration, Configuration>(new ContainerControlledLifetimeManager());
			container.RegisterType<ILogs, Logs>(new ContainerControlledLifetimeManager());
			container.RegisterType<IHelper, Helper>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDbHelper, DbHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDal, Dal>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDbConnection, SqlConnection>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDbCommand, SqlCommand>(new ContainerControlledLifetimeManager());
			container.RegisterType<IFtpClient, FtpClient>(new ContainerControlledLifetimeManager());
			container.RegisterType<IFtpService, FtpService>(new ContainerControlledLifetimeManager());
			container.RegisterType<Job>(new ContainerControlledLifetimeManager());

			var job = container.Resolve<Job>();
			job.Start();
		}
	}
}
