using Autofac;
using RockyToy.Contracts.Common.Config;

namespace RockyToy.Core.Common.Config
{
	public class ConfigModule : Module
	{
		private readonly string _appName;

		public ConfigModule(string appName)
		{
			_appName = appName;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new DeviceAppConfig(_appName)).As<IDeviceAppConfig>().SingleInstance();
		}
	}
}