using System.Xml.Linq;

namespace RockyToy.Contracts.Common.Config
{
	public interface IDeviceAppConfig : IConfigData
	{
		string AppTempPath { get; }
		string AppDataPath { get; }
		string AppLogPath { get; }
		string DeviceAppConfigFilePath { get; }

		XDocument Xml { get; }

	}
}