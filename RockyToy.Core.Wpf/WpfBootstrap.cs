using System;
using System.Linq;
using System.Windows;
using Autofac;
using Autofac.Features.Indexed;
using ReactiveUI;
using RockyToy.Contracts.Wpf;
using RockyToy.Core.Common;
using RockyToy.Core.Wpf.MetroDialog;
using Splat;

namespace RockyToy.Core.Wpf
{
	public class WpfBootstrap : Bootstrap
	{
		protected override void PrepareModules()
		{
			base.PrepareModules();

			Builder.RegisterModule<MetroDialogModule>();

			Builder.RegisterType<ViewHelper>().As<IViewHelper>().SingleInstance();

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			Builder.RegisterAssemblyTypes(assemblies)
				.Where(t => typeof(IViewModel).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
				.AsSelf()
				.Keyed<IViewModel>(t => t);

			Builder.RegisterAssemblyTypes(assemblies)
				.Where(t => typeof(IView).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
				.AsSelf()
				.Keyed<IView>(t =>
				{
					var vm = t.Assembly.GetType($"{t.FullName}Model");
					if (vm == null)
						throw new ArgumentException($"can not find view model of {t.FullName} in {t.Assembly.FullName}");
					return vm;
				});
			Builder.RegisterAssemblyTypes(assemblies)
				.Where(t => typeof(IShellView).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
				.AsSelf()
				.Keyed<IShellView>(t =>
				{
					var vm = t.Assembly.GetType($"{t.FullName}Model");
					if (vm == null)
						throw new ArgumentException($"can not find view model of {t.FullName} in {t.Assembly.FullName}");
					return vm;
				});
			Builder.RegisterAssemblyTypes(assemblies)
				.Where(t => typeof(IScreenView).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
				.AsSelf()
				.Keyed<IScreenView>(t =>
				{
					var vm = t.Assembly.GetType($"{t.FullName}Model");
					if (vm == null)
						throw new ArgumentException($"can not find view model of {t.FullName} in {t.Assembly.FullName}");
					return vm;
				});
		}

		public override void OnStartUp(string[] args)
		{
			base.OnStartUp(args);

			// set default style of window
			FrameworkElement.StyleProperty.OverrideMetadata(
				typeof(Window),
				new FrameworkPropertyMetadata
				{
					DefaultValue = Application.Current.FindResource("DefaultWindowStyle")
				}
			);

			// map view to viewmodel in splat
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var viewModels = assemblies.SelectMany(x => x.GetTypes())
				.Where(t => typeof(IViewModel).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
			var views = Root.Resolve<IIndex<Type, Func<IView>>>();
			foreach (var vm in viewModels)
				if (views.TryGetValue(vm, out var v))
					Locator.CurrentMutable.Register(v, typeof(IViewFor<>).MakeGenericType(vm));
		}
	}
}