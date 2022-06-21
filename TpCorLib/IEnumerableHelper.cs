using System;
using System.Collections;
using System.Collections.Generic;

namespace Teleperformance
{
	public static class IEnumerableHelper
	{
		#region Static Method(s) 

		public static void DisposeAll(this IEnumerable enumerable)
		{
			foreach (object o in enumerable)
			{
				if (o is IDisposable)
				{
					((IDisposable)o).Dispose();
				}
			}
		}

		public static void DisposeAll<T>(this IEnumerable<T> enumerable)
		{
			foreach (T o in enumerable)
			{
				if (o is IDisposable)
				{
					((IDisposable)o).Dispose();
				}
			}
		}

		#endregion Static Method(s) 
	}
}
