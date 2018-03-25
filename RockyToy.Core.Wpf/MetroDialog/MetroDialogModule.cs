using Autofac;

namespace RockyToy.Core.Wpf.MetroDialog
{
	public class MetroDialogModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MetroDialogCoordinator>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<MetroDialogHelper>().AsImplementedInterfaces();
		}
	}
}
