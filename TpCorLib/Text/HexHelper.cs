using System;
using System.Text;

namespace Teleperformance.Text
{
	public static class HexHelper
	{
		#region Static Method(s) 

		/* NOTE Justin Long: The HEX string methods are from:
		 *	http://www.nathanm.com/csharp-convert-hex-string-tofrom-byte-array-fast/
		 *	because they where better than mine.
		 */

		private const string hexAlphabet = "0123456789ABCDEF";
		private static readonly int[] hexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
			0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

		public static string ToHexString(this byte[] bytes)
		{
			StringBuilder result = new StringBuilder();
			string hexAlphabet = "0123456789ABCDEF";

			foreach (byte B in bytes)
			{
				result.Append(hexAlphabet[(int)(B >> 4)]);
				result.Append(hexAlphabet[(int)(B & 0xF)]);
			}

			return result.ToString();
		}

		public static byte[] ToByteArray(this string hex)
		{
			byte[] bytes = new byte[hex.Length / 2];

			for (int x = 0, i = 0; i < hex.Length; i += 2, x += 1)
			{
				bytes[x] = (byte)(HexHelper.hexValue[Char.ToUpper(hex[i + 0]) - '0'] << 4
					| HexHelper.hexValue[Char.ToUpper(hex[i + 1]) - '0']);
			}

			return bytes;
		}

		#endregion Static Method(s) 
	}
}
