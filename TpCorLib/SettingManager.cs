using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teleperformance
{
	public abstract class SettingManager : ISettingManager
	{
		#region Field(s) 

		private readonly object syncRoot = new object();
		private Dictionary<string, ISettingProvider> settingProviders = new Dictionary<string, ISettingProvider>(StringComparer.OrdinalIgnoreCase);

		#endregion Field(s) 

		#region Public Method(s) 

		public IEnumerable<string> GetPaths()
		{
			return this.settingProviders.Keys.ToArray();
		}

		public object GetSetting(string path, string name)
		{
			ISettingProvider settingProvider = this.GetSettingProvider(path, true);

			return settingProvider.GetSetting(name);
		}

		public IEnumerable<string> GetSettingNames(string path)
		{
			ISettingProvider settingProvider = this.GetSettingProvider(path, true);

			return settingProvider.GetSettingNames();
		}

		public void SetSetting(string path, string name, object value)
		{
			ISettingProvider settingProvider = this.GetSettingProvider(path, true);

			settingProvider.SetSetting(name, value);
		}

		#endregion Public Method(s) 

		#region Protected Method(s) 

		protected ISettingProvider GetSettingProvider(string path, bool throwIfMissing)
		{
			ISettingProvider settingProvider = null;

			if (this.settingProviders.ContainsKey(path))
			{
				settingProvider = this.settingProviders[path];
			}

			if (throwIfMissing && null == settingProvider)
			{
				throw new ArgumentException("Unknown path.");
			}

			return settingProvider;
		}

		protected void AddSettingProvider(string path, ISettingProvider settingProvider)
		{
			lock(this.syncRoot)
			{
				this.settingProviders.Add(path, settingProvider);
			}
		}

		protected void RemoveSettingProvider(string path)
		{
			lock(this.syncRoot)
			{
				this.settingProviders.Remove(path);
			}
		}

		#endregion Protected Method(s) 
	}
}