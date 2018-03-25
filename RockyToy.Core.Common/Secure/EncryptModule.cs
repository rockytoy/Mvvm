using Autofac;
using RockyToy.Contracts.Common.Secure;

namespace RockyToy.Core.Common.Secure
{
	public class EncryptModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SimpleEncrypt>().As<IEncrypt>().SingleInstance();
		}
	}
}