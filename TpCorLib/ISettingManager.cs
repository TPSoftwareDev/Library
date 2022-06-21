using System.Collections.Generic;

namespace Teleperformance
{
	public interface ISettingManager
	{
		#region Method(s)

		IEnumerable<string> GetPaths();

		object GetSetting(string path, string name);

		IEnumerable<string> GetSettingNames(string path);

		void SetSetting(string path, string name, object value);

		#endregion Method(s)
	}
}
