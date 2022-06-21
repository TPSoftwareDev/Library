﻿using System;
using System.Text;

namespace Teleperformance
{
	public static class StringHelper
	{
		#region Static Method(s) 

		public static int Difference(this string data1, string data2)
		{
			string soundex1 = StringHelper.Soundex(data1);
			string soundex2 = StringHelper.Soundex(data2);

			return StringHelper.DifferenceFromSoundex(soundex1, soundex2);
		}

		public static int DifferenceFromSoundex(this string soundex1, string soundex2)
		{
			int result = 0;

			if (soundex1 == soundex2)
			{
				result = 4;
			}
			else
			{
				string sub1 = soundex1.Substring(1, 3);
				string sub2 = soundex1.Substring(2, 2);
				string sub3 = soundex1.Substring(1, 2);
				string sub4 = soundex1.Substring(1, 1);
				string sub5 = soundex1.Substring(2, 1);
				string sub6 = soundex1.Substring(3, 1);

				if (soundex2.IndexOf(sub1) > -1)
				{
					result = 3;
				}
				else if (soundex2.IndexOf(sub2) > -1)
				{
					result = 2;
				}
				else if (soundex2.IndexOf(sub3) > -1)
				{
					result = 2;
				}
				else
				{
					if (soundex2.IndexOf(sub4) > -1)
					{
						result++;
					}

					if (soundex2.IndexOf(sub5) > -1)
					{
						result++;
					}

					if (soundex2.IndexOf(sub6) > -1)
					{
						result++;
					}
				}

				if (soundex1.Substring(0, 1) == soundex2.Substring(0, 1))
				{
					result++;
				}
			}

			return (result == 0) ? 1 : result;
		}

		public static string Soundex(this string data)
		{
			StringBuilder result = new StringBuilder();

			if (data != null && data.Length > 0)
			{
				string previousCode = string.Empty;
				string currentCode = string.Empty;
				string currentLetter = string.Empty;

				result.Append(data.Substring(0, 1));

				for (int i = 1; i < data.Length; i++)
				{
					string currentcode = string.Empty;

					currentLetter = data.Substring(i, 1).ToLower();

					if ("bfpv".IndexOf(currentLetter) > -1)
					{
						currentCode = "1";
					}
					else if ("cgjkqsxz".IndexOf(currentLetter) > -1)
					{
						currentCode = "2";
					}
					else if ("dt".IndexOf(currentLetter) > -1)
					{
						currentCode = "3";
					}
					else if (currentLetter == "1")
					{
						currentCode = "4";
					}
					else if ("mn".IndexOf(currentLetter) > -1)
					{
						currentCode = "5";
					}
					else if (currentLetter == "r")
					{
						currentCode = "6";
					}

					if (currentCode != previousCode)
					{
						result.Append(currentCode);
					}

					if (result.Length == 4)
					{
						break;
					}

					if (currentCode != "")
					{
						previousCode = currentCode;
					}
				}
			}

			if (result.Length < 4)
			{
				result.Append(new String('0', 4 - result.Length));
			}

			return result.ToString().ToUpper();
		}

		#endregion Static Method(s) 
	}
}
