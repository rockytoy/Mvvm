using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RockyToy.Contracts.Common.Config;
using RockyToy.Contracts.Common.Extensions;

namespace RockyToy.Core.Common.Config
{
	public class DeviceAppConfig : IDeviceAppConfig
	{
		public string AppName { get; }

		public string AppTempPath => Path.Combine(Path.GetTempPath(), AppName);

		public string AppDataPath =>
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

		public string AppLogPath => Path.Combine(AppDataPath, "Log");

		public string DeviceAppConfigFilePath => Path.Combine(AppDataPath, "DeviceConfig.xml");

		public void SetConfig(string key, string value)
		{
			if (Xml?.Root == null || value == null)
				return;

			var config = Xml.Root.FirstOrDefault("Config", x => x.Attr("Key").StrEmpty() == key);
			if (config == null)
			{
				config = new XElement("Config");
				config.SetAttributeValue("Key", key);
				config.SetAttributeValue("Value", value);
				Xml.Root.Add(config);
			}
			else
			{
				config.Attr("Value").SetValue(value);
			}

			Xml.Save(DeviceAppConfigFilePath);
		}

		public void SetConfig<T>(string key, T value)
		{
			if (Xml?.Root == null || value == null)
				return;
			var config = Xml.Root.FirstOrDefault("Config", x => x.Attr("Key").StrEmpty() == key);
			if (config == null)
			{
				config = new XElement("Config");
				config.SetAttributeValue("Key", key);
				config.SetAttributeValue("Value", value);
				Xml.Root.Add(config);
			}
			else
			{
				config.Attr("Value").SetValue(value);
			}

			Xml.Save(DeviceAppConfigFilePath);
		}

		/// <summary>
		/// try to get config value from <see cref="Xml"/>.
		/// if <paramref name="key"/> does not exist, add config key value pair (<paramref name="key"/>, <paramref name="def"/>) to config and save to file.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public T GetDef<T>(string key, T def = default(T)) where T : struct
		{
			if (Xml?.Root == null)
				return def;
			var ele = Xml.Descendants("Config").FirstOrDefault(x => (string) x.Attribute("Key") == key);
			if (ele == null)
			{
				SetConfig(key, def);
				return def;
			}
			else
			{
				return ele.Attr("Value").GetDef(def);
			}
		}

		/// <summary>
		/// try to get config value from <see cref="Xml"/>.
		/// if <paramref name="key"/> does not exist, add config key value pair (<paramref name="key"/>, <paramref name="def"/>) to config and save to file.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="def">null is <see cref="string.Empty"/></param>
		/// <returns></returns>
		public string StrDef(string key, string def = null)
		{
			def = def ?? string.Empty;
			if (Xml?.Root == null)
				return def;
			var ele = Xml.Descendants("Config").FirstOrDefault(x => (string) x.Attribute("Key") == key);
			if (ele == null)
			{
				SetConfig(key, def);
				return def;
			}
			else
			{
				return ele.Attr("Value").StrDef(def);
			}
		}

		public XDocument Xml { get; }

		public DeviceAppConfig(string appName)
		{
			AppName = appName;
			// ensure that app directoery exists
			Directory.CreateDirectory(AppTempPath);
			Directory.CreateDirectory(AppDataPath);
			Directory.CreateDirectory(AppLogPath);
			// clean up temp
			try
			{
				var di = new DirectoryInfo(AppTempPath);
				foreach (var file in di.GetFiles())
					file.Delete();
				foreach (var dir in di.GetDirectories())
					dir.Delete(true);
			}
			catch
			{
				// ignored
			}


			if (!File.Exists(DeviceAppConfigFilePath))
				File.WriteAllText(DeviceAppConfigFilePath, "<Root></Root>");

			Xml = XDocument.Load(DeviceAppConfigFilePath);
		}
	}
}