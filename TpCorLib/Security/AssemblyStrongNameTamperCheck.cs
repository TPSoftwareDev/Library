using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Teleperformance.Security
{
	public class AssemblyStrongNameTamperCheck : TamperCheck
	{
		#region Construction/Destruction 

		/* NOTE Justin Long: This is done by default by the .Net framework but it can be turned off so we want 
		 *	to use this to force it even if it's been turned off.
		 */
		public AssemblyStrongNameTamperCheck(Assembly assembly)
			: this(assembly.Location) { }

		public AssemblyStrongNameTamperCheck(string assemblyFileName)
			: base()
		{
			this.AssemblyFileName = assemblyFileName;
			this.CheckOnlyOnce = true;
		}

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public string AssemblyFileName { get; protected set; }

		public bool CheckOnlyOnce { get; set; }

		#endregion Public Property(ies) 

		#region Public Method(s) 

		public override void PerformCheck(ref List<string> results)
		{
			bool wasVerified;
			bool verificationResult = AssemblyStrongNameTamperCheck.strongNameSignatureVerificationEx(
				this.AssemblyFileName, true, out wasVerified);

			if (!verificationResult)
			{
				results.Add(string.Format("Invalid strong name signature for assembly '{0}'", this.AssemblyFileName));
			}

			if (this.CheckOnlyOnce)
			{
				this.Enabled = false;
			}
		}

		#endregion Public Method(s) 

		[DllImport("mscoree.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "StrongNameSignatureVerificationEx")]
		[return: MarshalAs(UnmanagedType.U1)]
		private static extern bool strongNameSignatureVerificationEx(
		   [MarshalAs(UnmanagedType.LPWStr)] string wszFilePath,
		   [MarshalAs(UnmanagedType.U1)] bool fForceVerification,
		   [MarshalAs(UnmanagedType.U1)] out bool pfWasVerified);
	}
}
