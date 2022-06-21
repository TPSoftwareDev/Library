using System;

namespace Teleperformance.Collections
{
	internal static class GenericCollectionHelper
	{
		public static void CheckType<T>(object item)
		{
			if (null != item)
			{
				if (typeof(T).IsValueType)
				{
					throw new ArgumentException("Item can't be null because this is a list of value types.", "item");
				}
			}
			else if (!(item is T))
			{
				throw new ArgumentException(string.Format("Type mismatch, expected '{0}', received '{1}'.",
					typeof(T).FullName, item.GetType().FullName), "item");
			}
		}
	}
}