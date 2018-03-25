namespace RockyToy.Contracts.Common.Config
{
	public interface IConfigData
	{
		T GetDef<T>(string key, T def = default(T)) where T : struct;
		string StrDef(string key, string def = null);
		void SetConfig(string key, string value);
		void SetConfig<T>(string key, T value);
	}
}