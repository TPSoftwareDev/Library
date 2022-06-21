using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Teleperformance.ComponentModel
{
	public abstract class ProgramBase<TProgram> : IServiceProvider
		where TProgram : ProgramBase<TProgram>, new()
	{
		#region Field(s) 

		private static TProgram instance = new TProgram();
		private IServiceContainer serviceContainer;

		#endregion Field(s) 

		#region Static Property(ies) 

		internal static TProgram Instance
		{
			get { return ProgramBase<TProgram>.instance; }
		}

		#endregion Static Property(ies) 

		#region Protected Method(s) 

		protected virtual void AddServices(IServiceContainer serviceContainer)
		{
			serviceContainer.AddService(typeof(IServiceProvider), this);
		}

		protected virtual IServiceContainer CreateServiceContainer()
		{
			return new ProgramServiceContainer();
		}

		protected void HandleUnhandledException(Exception exception)
		{
			if (null != exception)
			{
				Console.WriteLine($"{exception}An unhandled exception has occurred");
			}
		}

		protected abstract void RunApplication();

		#endregion Protected Method(s) 

		#region Private Method(s) 

		object IServiceProvider.GetService(Type serviceType)
		{
			return ProgramBase<TProgram>.GetService(serviceType, false);
		}

		private void run()
		{
			this.serviceContainer = this.CreateServiceContainer();

			try
			{
				this.AddServices(this.serviceContainer);
				this.RunApplication();
			}
			finally
			{
				if (null != this.serviceContainer && this.serviceContainer is IDisposable)
				{
					((IDisposable)this.serviceContainer).Dispose();
				}
			}
		}

		#endregion Private Method(s) 

		#region Static Method(s) 

		public static object GetService(Type serviceType, bool throwIfMissing = false)
		{
			object serviceInstance = ProgramBase<TProgram>.instance.serviceContainer.GetService(serviceType);

			if (throwIfMissing && null == serviceInstance)
			{
				throw new KeyNotFoundException();
			}

			return serviceInstance;
		}

		public static TService GetService<TService>(bool throwIfMissing = false)
			where TService : class
		{
			return ProgramBase<TProgram>.GetService(typeof(TService), throwIfMissing) as TService;
		}

		protected static void Run()
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

				ProgramBase<TProgram>.Instance.run();
			}
			catch (Exception exception)
			{
				ProgramBase<TProgram>.Instance.HandleUnhandledException(exception);
			}
		}
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ProgramBase<TProgram>.Instance.HandleUnhandledException(e.ExceptionObject as Exception);
		}

		#endregion Static Method(s) 
	}
}