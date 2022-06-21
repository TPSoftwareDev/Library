using System.Reflection;

namespace Teleperformance.Reflection
{
	public static class AssemblyHelper
	{
		public static AssemblyProductAttribute GetAssemblyProductAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyProductAttribute>(assembly);
		}

		public static AssemblyVersionAttribute GetAssemblyVersionAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyVersionAttribute>(assembly);
		}

		public static AssemblyCompanyAttribute GetAssemblyCompanyAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyCompanyAttribute>(assembly);
		}

		public static AssemblyDescriptionAttribute GetAssemblyDescriptionAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyDescriptionAttribute>(assembly);
		}

		public static AssemblyCopyrightAttribute GetAssemblyCopyrightAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyCopyrightAttribute>(assembly);
		}

		public static AssemblyTitleAttribute GetAssemblyTitleAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyTitleAttribute>(assembly);
		}

		public static AssemblyConfigurationAttribute GetAssemblyConfigurationAttribute(this Assembly assembly)
		{
			return AttributeHelper.GetFirstAttribute<AssemblyConfigurationAttribute>(assembly);
		}
	}
}
