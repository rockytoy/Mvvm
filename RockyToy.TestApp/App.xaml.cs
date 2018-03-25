using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Common.Extensions;

namespace RockyToy.TestApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : IApp
	{
		public App()
		{
			Bootstrap = new TestAppBootstrap("TestApp", "RandomPassword".ToSecureString(), Encoding.UTF8.GetBytes("TestApp"));
			DispatcherUnhandledException += App_DispatcherUnhandledException;
		}

		public TestAppBootstrap Bootstrap { get; }

		public IContainer Root => Bootstrap.Root;

		private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var logger = ((IApp) Current).Root.Resolve<ILogger>(new TypedParameter(typeof(Type), typeof(App)));
			logger.FatalException("unhandled exception", e.Exception);
			var msg = e.Exception.Message;
			var inner = e.Exception.InnerException;
			while (inner != null)
			{
				msg += Environment.NewLine + inner.Message;
				inner = inner.InnerException;
			}

			MessageBox.Show($"Error Message: {msg}", "Unhandled Error");
			e.Handled = true;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			Bootstrap.OnStartUp(e.Args);
			if (!Bootstrap.IsInitialized)
				Shutdown();
			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			Bootstrap.OnShutdown(e.ApplicationExitCode);
		}
	}
}