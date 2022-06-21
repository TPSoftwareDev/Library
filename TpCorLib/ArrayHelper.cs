namespace Teleperformance
{
	public static class ArrayHelper
	{
		#region Static Method(s) 

		public static T[] GetSegment<T>(this T[] parent, int start, int length)
		{
			int end = (start + length);
			T[] segment = new T[length];

			for (int i = start; i < end; i++)
			{
				segment[i - start] = parent[i];
			}

			return segment;
		}

		public static byte[] GetSegment(this byte[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<byte>(parent, start, length);
		}

		public static char[] GetSegment(this char[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<char>(parent, start, length);
		}

		public static short[] GetSegment(this short[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<short>(parent, start, length);
		}

		public static int[] GetSegment(this int[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<int>(parent, start, length);
		}

		public static long[] GetSegment(this long[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<long>(parent, start, length);
		}

		public static float[] GetSegment(this float[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<float>(parent, start, length);
		}

		public static double[] GetSegment(this double[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<double>(parent, start, length);
		}

		public static bool[] GetSegment(this bool[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<bool>(parent, start, length);
		}

		public static object[] GetSegment(this object[] parent, int start, int length)
		{
			return ArrayHelper.GetSegment<object>(parent, start, length);
		}

		#endregion Static Method(s) 
	}
}