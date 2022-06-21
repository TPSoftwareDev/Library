using System;
using System.Collections.Generic;
using System.Text;

namespace Teleperformance
{
	public interface ISettingProvider
	{
		#region Method(s) 

		object GetSetting(string name);

		IEnumerable<string> GetSettingNames();

		void SetSetting(string name, object value);

		#endregion Method(s) 
	}
}
