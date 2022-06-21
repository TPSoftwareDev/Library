using System.Configuration;

namespace Teleperformance.Configuration
{
	public class SimpleSettingElement : ConfigurationElement
	{
		public SimpleSettingElement()
		{

		}

		[ConfigurationProperty("key", IsKey = true, IsRequired = true)]
		public string Key
		{
			get { return (string)this["key"]; }
			set { this["key"] = value; }
		}

		[ConfigurationProperty("value", IsRequired = false, DefaultValue = null)]
		public string Value
		{
			get { return (string)this["value"]; }
			set { this["value"] = value; }
		}
	}
}