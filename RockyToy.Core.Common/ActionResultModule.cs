using Autofac;
using RockyToy.Contracts.Common;

namespace RockyToy.Core.Common
{
	public class ActionResultModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ActionResultFactory>().As<IActionResultFactory>().SingleInstance();
		}
	}
}