using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using Teleperformance.Threading;

namespace Teleperformance.Security
{
	public enum TamperState
	{
		/// <summary>
		/// No tampering has been detected.
		/// </summary>
		NeverDetected,
		/// <summary>
		/// Tampering has been detected.
		/// </summary>
		CurrentlyDetected,
		/// <summary>
		/// Tampering was detected but is no longer detected.
		/// </summary>
		HasBeenDetected
	}

	public class TamperCheckResult
	{
		#region Construction/Destruction 

		public TamperCheckResult(TamperCheck check, string[] reasons)
		{
			this.Check = check;
			this.Reasons = reasons;
		}

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public TamperCheck Check { get; protected set; }

		public string[] Reasons { get; protected set; }

		public bool TamperDetected { get { return (this.Reasons.Length > 0); } }

		#endregion Public Property(ies) 
	}

	public class TamperMonitor : Component
	{
		// NOTE Justin Long: This could be architected much better (add a push instead of a pull for checks?), I was rushing things...

		#region Field(s) 

		protected System.Timers.Timer CheckTimer;
		protected List<TamperCheck> TamperChecks = new List<TamperCheck>();
		private TamperState tamperState;
		private readonly object syncRoot = new object();
		private IContainer components = null;

		#endregion Field(s) 

		#region Event(s) 

		public event EventHandler<TamperingDetectedEventArgs> TamperingDetected;

		public event EventHandler TamperStateChanged;

		#endregion Event(s) 

		#region Construction/Destruction 

		public TamperMonitor(IContainer container)
		{
			if (null != container)
			{
				container.Add(this);
			}

			this.InitializeComponent();

			this.TamperChecks.Add(new DebuggerAttachedTamperCheck());
//			this.TamperChecks.Add(new CodeAccessSecurityDisabledTamperCheck());
			this.TamperChecks.Add(new AssemblyStrongNameTamperCheck(Assembly.GetExecutingAssembly()));
			this.TamperChecks.Add(new AssemblyPublicKeyTokenTamperCheck(Assembly.GetExecutingAssembly(), "5bddb7898390b3fc"));
		}

		public TamperMonitor()
			: this(null) { }

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public int CheckInterval
		{
			get { return (int)this.CheckTimer.Interval; }
			set { this.CheckTimer.Interval = value; }
		}

		public TamperState TamperState
		{
			get
			{
				lock(this.syncRoot)
				{
					return tamperState;
				}
			}
			private set
			{
				if (tamperState != value)
				{
					lock(this.syncRoot)
					{
						if (tamperState != value)
						{
							tamperState = value;
						}
					}
				}
			}
		}

		#endregion Public Property(ies) 

		#region Public Method(s) 

		public void AddTamperCheck(TamperCheck tamperCheck)
		{
			this.TamperChecks.Add(tamperCheck);
		}

		public bool HasTamperCheck(TamperCheck tamperCheck)
		{
			return this.TamperChecks.Contains(tamperCheck);
		}

		public void PerformTamperChecks()
		{
			List<TamperCheckResult> results = new List<TamperCheckResult>();

			this.PerformTamperChecks(ref results);
		}

		public void PerformTamperChecks(ref List<TamperCheckResult> results)
		{
			foreach (TamperCheck tamperCheck in this.TamperChecks)
			{
				if (tamperCheck.Enabled)
				{
					List<string> reasons = new List<string>();

					tamperCheck.PerformCheck(ref reasons);

					results.Add(new TamperCheckResult(tamperCheck, reasons.ToArray()));
				}
			}

			if (results.Count > 0)
			{
				this.OnTamperingDetected(results);
			}
		}

		public void RemoveTamperCheck(TamperCheck tamperCheck)
		{
			this.TamperChecks.Remove(tamperCheck);
		}

		public void RemoveTamperChecksByType<T>()
		{
			this.TamperChecks.ForEach(delegate (TamperCheck tamperCheck)
			{
				if (tamperCheck is T)
				{
					this.TamperChecks.Remove(tamperCheck);
				}
			});
		}

		public void Start()
		{
			this.CheckTimer.Start();
			this.PerformTamperChecks();
		}

		public void Stop()
		{
			this.CheckTimer.Stop();
		}

		#endregion Public Method(s) 

		#region Protected Method(s) 

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}

			base.Dispose(disposing);
		}

		protected virtual void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.CheckTimer = new System.Timers.Timer();//System.Windows.Forms.Timer(this.components);
			// 
			// CheckTimer
			// 
			this.CheckTimer.Interval = 1000;
			this.CheckTimer.Elapsed += this.CheckTimer_Tick;
			this.CheckTimer.AutoReset = true;

		}

		protected virtual void OnTamperingDetected(IEnumerable<TamperCheckResult> results)
		{
			EventHelper.RaiseEvent(this.TamperingDetected, this, new TamperingDetectedEventArgs(results));
		}

		#endregion Protected Method(s) 

		#region Private Method(s) 

		private void CheckTimer_Tick(object state, EventArgs e)
		{
			this.PerformTamperChecks();
		}

		#endregion Private Method(s) 
	}
}
