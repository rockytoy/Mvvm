using Autofac;
using RockyToy.Contracts.Common.Config;

namespace RockyToy.Core.Common.Config
{
	public class ConfigModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new DeviceAppConfig(c.ResolveNamed<string>("AppName"))).As<IDeviceAppConfig>().SingleInstance();
		}
	}
}