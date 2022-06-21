using System.Collections.Generic;

namespace Teleperformance.Security
{
	public abstract class TamperCheck
	{
		#region Construction/Destruction 

		public TamperCheck()
		{
			this.Enabled = true;
		}

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public bool Enabled { get; set; }

		#endregion Public Property(ies) 

		#region Public Method(s) 

		public abstract void PerformCheck(ref List<string> results);

		#endregion Public Method(s) 
	}
}
