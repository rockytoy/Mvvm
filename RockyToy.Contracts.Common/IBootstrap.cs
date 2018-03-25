using System;
using Autofac;

namespace RockyToy.Contracts.Common
{
	public interface IBootstrap : IDisposable
	{
		bool IsInitialized { get; }

		IContainer Root { get; }

		void OnStartUp(string[] args);
		void OnShutdown(int exitCode);
	}
}