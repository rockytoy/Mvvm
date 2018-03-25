using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Splat;
using ILogger = RockyToy.Contracts.Common.ILogger;

namespace RockyToy.Core.Common
{
	public class LoggerModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			DependencyResolverMixins.RegisterConstant(Locator.CurrentMutable, new FuncLogManager(type => new NLogLogger(type)),
				typeof(IFullLogger));

			builder
				.Register((c, p) => new NLogLogger(p.TypedAs<Type>()))
				.As<ILogger>();
		}

		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
			IComponentRegistration registration)
		{
			registration.Preparing +=
				(sender, args) =>
				{
					var forType = args.Component.Activator.LimitType;

					var logParameter = new ResolvedParameter(
						(p, c) => p.ParameterType == typeof(ILogger),
						(p, c) => c.Resolve<ILogger>(TypedParameter.From(forType)));

					args.Parameters = args.Parameters.Union(new[] {logParameter});
				};
		}
	}
}