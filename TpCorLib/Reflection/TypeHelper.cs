using System;
using System.Reflection;

namespace Teleperformance.Reflection
{
	public static class TypeHelper
	{
		public static string GetAssemblyQualifiedNameWithoutVersion(this Type type)
		{
			AssemblyName assemblyName = type.Assembly.GetName();

			return type.FullName + ", " + assemblyName.Name;
		}
	}
}
