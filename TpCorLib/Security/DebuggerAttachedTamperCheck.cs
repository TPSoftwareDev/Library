using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Teleperformance.Security
{
	public class DebuggerAttachedTamperCheck : TamperCheck
	{
		#region Construction/Destruction 

		public DebuggerAttachedTamperCheck()
			: base() { }

		#endregion Construction/Destruction 

		#region Public Method(s) 

		public override void PerformCheck(ref List<string> results)
		{
			if (Debugger.IsAttached || DebuggerAttachedTamperCheck.isDebuggerPresent() == 1)
			{
				results.Add("Debugger is attached.");
			}
		}

		#endregion Public Method(s) 

		#region Static Method(s) 

		[DllImport("kernel32.dll", SetLastError = true, EntryPoint = "IsDebuggerPresent")]
		private static extern int isDebuggerPresent();

		#endregion Static Method(s) 
	}
}
