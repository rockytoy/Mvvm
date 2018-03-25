using Autofac;

namespace RockyToy.Contracts.Common
{
	public interface IApp
	{
		IContainer Root { get; }
	}
}