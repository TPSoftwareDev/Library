using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Teleperformance.Configuration
{
	public class Registry
	{
		public static RegistryKey GetOrCreateKey(string key, bool write = false)
		{
			RegistryKey baseKey;
			RegistryKey registryKey;

			if (Environment.Is64BitOperatingSystem == true)
			{
				baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
			}
			else
			{
				baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
			}
			try
			{
				registryKey = baseKey.OpenSubKey(key, write);
			}
			catch
			{
				registryKey = baseKey.CreateSubKey(key);
			}
			if (registryKey == null)
			{
				registryKey = baseKey.CreateSubKey(key);
			}
			return registryKey;
		}
		public static void SetRegistryKey(string name, string value, string baseKey)
		{
			RegistryKey rk = Registry.GetOrCreateKey(baseKey, true);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}
		public static string GetRegistryValue(string regValue, string baseKey)
		{
			string returnValue = null;
			RegistryKey rk = Registry.GetOrCreateKey(baseKey);
			if (rk != null)
			{
				returnValue = Registry.GetRegistryValue(regValue, rk);
			}
			return returnValue;
		}
		public static string GetRegistryValue(string regValue, RegistryKey baseKey)
		{
			string ret = null;
			if (baseKey != null)
			{
				string[] valueKeys = baseKey.GetValueNames();
				if (valueKeys.Contains(regValue))
				{
					int term;
					ret = (string)baseKey.GetValue(regValue);
					// Trim extra stuff to get around a bug in TPC
					term = ret.IndexOf('\0');
					if (term != -1)
					{
						ret = ret.Substring(0, term);
					}
				}
			}
			return ret;
		}
		public static RegistryKey GetKey(string key, bool write = false)
		{
			RegistryKey baseKey;
			RegistryKey registryKey = null;

			if (Environment.Is64BitOperatingSystem == true)
			{
				baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
			}
			else
			{
				baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
			}

			if (baseKey != null)
			{
				try
				{
					registryKey = baseKey.OpenSubKey(key, write);
				}
				catch { }
				// If its not there in the 64bit hive might be an old app your reading, try the 32
				if (registryKey == null && Environment.Is64BitOperatingSystem == true)
				{
					baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
					try
					{
						registryKey = baseKey.OpenSubKey(key, write);
					}
					catch { }
				}
			}

			return registryKey;
		}
		private static void ReadKey(RegistryKey key, ref Dictionary<string, string> data)
		{
			string[] valuenames = key.GetValueNames();
			foreach (string valuename in valuenames)
			{
				string newkeyname = valuename;
				object value = key.GetValue(valuename);
				if (value != null)
				{
					if (key.Name == "Install")
					{
						newkeyname = "Install" + valuename;
					}
					data.Add(newkeyname, value.ToString());
				}
			}
			string[] subkeynames = key.GetSubKeyNames();
			foreach (string subkeyname in subkeynames)
			{
				RegistryKey subkey = key.OpenSubKey(subkeyname);
				Registry.ReadKey(subkey, ref data);
			}
		}
		public static Dictionary<string, string> GetAllValuesFromKey(string key)
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			RegistryKey regkey = Registry.GetKey(key, false);
			Registry.ReadKey(regkey, ref returnValue);
			return returnValue;
		}
	}
}
