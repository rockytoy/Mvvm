﻿using System;
using System.IO;
using System.Security;
using Autofac;
using NLog;
using NLog.Config;
using NLog.Targets;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Common.Config;
using RockyToy.Core.Common.Config;
using RockyToy.Core.Common.Secure;

namespace RockyToy.Core.Common
{
	public class Bootstrap : IBootstrap
	{
		private readonly string _appName;
		private readonly SecureString _securePwd;
		private readonly byte[] _secureToken;

		public Bootstrap(string appName, SecureString securePwd, byte[] secureToken)
		{
			_appName = appName;
			_securePwd = securePwd;
			_secureToken = secureToken;
		}

		protected readonly ContainerBuilder Builder = new ContainerBuilder();

		public bool IsInitialized => Root != null;

		public IContainer Root { get; private set; }

		public virtual void OnStartUp(string[] args)
		{
			PrepareModules();

			Root = Builder.Build();

			SetupNLog();
		}

		public virtual void OnShutdown(int exitCode)
		{
			Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void SetupNLog()
		{
			var appConfig = Root.Resolve<IDeviceAppConfig>();
			var logConfig = new LoggingConfiguration();

#if DEBUG
			var debugTarget = new DebuggerTarget();
			logConfig.AddTarget("debug", debugTarget);
			logConfig.AddRule(LogLevel.Trace, LogLevel.Fatal, debugTarget);
#endif

			var today = DateTimeOffset.UtcNow.ToString("yyyy'_'MM'_'dd");
			var logFilePath = Path.Combine(appConfig.AppLogPath, $"{today}.log");
			var fileTarget = new FileTarget
			{
				FileName = logFilePath
			};

			logConfig.AddTarget("file", fileTarget);
			logConfig.AddRule(LogLevel.Warn, LogLevel.Fatal, fileTarget);

			LogManager.Configuration = logConfig;
		}

		protected virtual void PrepareModules()
		{
			Builder
				.RegisterModule(new ConfigModule(_appName))
				.RegisterModule(new EncryptModule(_securePwd, _secureToken))
				.RegisterModule<ActionResultModule>()
				.RegisterModule<LoggerModule>();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing) Root?.Dispose();
		}
	}
}