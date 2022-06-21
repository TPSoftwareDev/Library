using System.Configuration;

namespace Teleperformance.Configuration
{
	public class SimpleSettingElementCollection : GenericElementCollection<SimpleSettingElement>
	{
		#region Construction/Destruction 

		public SimpleSettingElementCollection()
			: base() { }

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMap; }
		}

		#endregion Public Property(ies) 

		#region Protected Property(ies) 

		protected override string ElementName { get { return "setting"; } }

		#endregion Protected Property(ies) 

		#region Public Method(s) 

		public string GetSetting(string key)
		{
			return this.GetSetting(key, null);
		}

		public string GetSetting(string key, string defaultValue)
		{
			string result = defaultValue;

			if (null != this[key])
			{
				result = this[key].Value;
			}

			return result;
		}

		#endregion Public Method(s) 

		#region Protected Method(s) 

		protected override ConfigurationElement CreateNewElement()
		{
			return new SimpleSettingElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SimpleSettingElement)element).Key;
		}

		#endregion Protected Method(s) 
	}
}