using System;
using System.Collections.Generic;

namespace Teleperformance.Collections
{
	public class DictionaryChangeEventArgs<TKey, TValue> : EventArgs
	{
		#region Construction/Destruction 

		public DictionaryChangeEventArgs(DictionaryChangeDescription change, params KeyValuePair<TKey, TValue>[] pairs)
			: base()
		{
			this.Change = change;
			this.Pairs = pairs;
		}

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public DictionaryChangeDescription Change { get; protected set; }

		public KeyValuePair<TKey, TValue>[] Pairs { get; protected set; }

		#endregion Public Property(ies) 
	}
}