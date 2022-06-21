using System;
namespace Teleperformance
{
	public static class EventHelper
	{
		#region Static Method(s) 

		public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
			where TEventArgs : EventArgs
		{
			if (null != handler)
			{
				handler(sender, e);
			}
		}

		public static void RaiseEvent(this EventHandler handler, object sender, EventArgs e)
		{
			if (null != handler)
			{
				handler(sender, e);
			}
		}

		public static void RaiseEvent(this EventHandler handler, object sender)
		{
			EventHelper.RaiseEvent(handler, sender, EventArgs.Empty);
		}

		#endregion Static Method(s) 
	}
}
