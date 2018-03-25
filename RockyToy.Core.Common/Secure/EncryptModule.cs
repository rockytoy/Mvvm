using System.Security;
using Autofac;
using RockyToy.Contracts.Common.Secure;

namespace RockyToy.Core.Common.Secure
{
	public class EncryptModule : Module
	{
		private readonly SecureString _defaultPwd;
		private readonly byte[] _defaultToken;

		public EncryptModule(SecureString defaultPwd, byte[] defaultToken)
		{
			_defaultPwd = defaultPwd;
			_defaultToken = defaultToken;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new SimpleEncrypt(_defaultPwd, _defaultToken)).As<IEncrypt>().SingleInstance();
		}
	}
}