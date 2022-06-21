using System;
using System.Collections.Generic;

namespace Teleperformance.Security
{
	public class TamperingDetectedEventArgs : EventArgs
	{
		#region Construction/Destruction

		public TamperingDetectedEventArgs(params TamperCheckResult[] results)
			: this((IEnumerable<TamperCheckResult>)results) { }

		public TamperingDetectedEventArgs(IEnumerable<TamperCheckResult> results)
		{
			this.CheckResults = results;

			List<TamperCheckResult> triggeredCheckResults = new List<TamperCheckResult>();
			List<string> reasons = new List<string>();

			foreach (TamperCheckResult result in this.CheckResults)
			{
				if (result.TamperDetected)
				{
					triggeredCheckResults.Add(result);
					reasons.AddRange(result.Reasons);
				}
			}

			this.TriggeredCheckResults = triggeredCheckResults.ToArray();
			this.Reasons = reasons.ToArray();
		}

		#endregion Construction/Destruction

		#region Public Property(ies)

		public IEnumerable<TamperCheckResult> CheckResults { get; protected set; }

		public IEnumerable<TamperCheckResult> TriggeredCheckResults { get; protected set; }

		public IEnumerable<string> Reasons { get; protected set; }

		#endregion Public Property(ies)
	}
}
