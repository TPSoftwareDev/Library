using System;
using System.Reflection;

namespace Teleperformance.Reflection
{
	public static class AttributeHelper
	{
		#region Static Method(s)

		public static T[] GetAttributes<T>(this ICustomAttributeProvider customAttributeProvider)
			where T : Attribute
		{
			return AttributeHelper.GetAttributes<T>(customAttributeProvider, true);
		}

		public static T[] GetAttributes<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
			where T : Attribute
		{
			T[] attributes = (T[])customAttributeProvider.GetCustomAttributes(typeof(T), inherit);

			return attributes;
		}

		public static T GetFirstAttribute<T>(this ICustomAttributeProvider customAttributeProvider)
			where T : Attribute
		{
			return AttributeHelper.GetFirstAttribute<T>(customAttributeProvider, true);
		}

		public static T GetFirstAttribute<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
			where T : Attribute
		{
			T firstAttribute = null;

			if (null != customAttributeProvider)
			{
				object[] attributes = customAttributeProvider.GetCustomAttributes(typeof(T), inherit);

				if (null != attributes && attributes.Length > 0)
				{
					firstAttribute = (T)attributes[0];
				}
			}

			return firstAttribute;
		}

		public static bool HasAttribute<T>(this ICustomAttributeProvider customAttributeProvider)
			where T : Attribute
		{
			return (null != AttributeHelper.GetFirstAttribute<T>(customAttributeProvider));
		}

		public static bool HasAttribute<T>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
			where T : Attribute
		{
			return (null != AttributeHelper.GetFirstAttribute<T>(customAttributeProvider, inherit));
		}

		#endregion Static Method(s)
	}
}
