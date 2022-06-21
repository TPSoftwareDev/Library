using System.Collections.Generic;
using System.Reflection;
using Teleperformance.Text;

namespace Teleperformance.Security
{
	public class AssemblyPublicKeyTokenTamperCheck : TamperCheck
	{
		#region Construction/Destruction

		public AssemblyPublicKeyTokenTamperCheck(Assembly assembly, string expectedPublicKeyTokenString)
			: this(assembly, HexHelper.ToByteArray(expectedPublicKeyTokenString)) { }

		public AssemblyPublicKeyTokenTamperCheck(Assembly assembly, byte[] expectedPublicKeyToken)
			: base()
		{
			this.Assembly = assembly;
			this.ExpectedPublicKeyToken = expectedPublicKeyToken;
		}

		#endregion Construction/Destruction

		#region Public Property(ies)

		public Assembly Assembly { get; protected set; }

		public bool CheckOnlyOnce { get; set; }

		public byte[] ExpectedPublicKeyToken { get; protected set; }

		#endregion Public Property(ies)

		#region Public Method(s)

		public override void PerformCheck(ref List<string> results)
		{
			AssemblyName assemblyName = this.Assembly.GetName();
			byte[] assemblyPublicKeyToken = assemblyName.GetPublicKeyToken();
			bool match = (assemblyPublicKeyToken.Length == this.ExpectedPublicKeyToken.Length);

			if (match)
			{
				for (int i = 0; i < assemblyPublicKeyToken.Length; i++)
				{
					if (assemblyPublicKeyToken[i] != this.ExpectedPublicKeyToken[i])
					{
						match = false;

						break;
					}
				}
			}

			if (!match)
			{
				results.Add(string.Format("Unexpected public key token for assembly '{0}'.",
					this.Assembly.Location));
			}

			if (this.CheckOnlyOnce)
			{
				this.Enabled = false;
			}
		}

		#endregion Public Method(s)
	}
}
