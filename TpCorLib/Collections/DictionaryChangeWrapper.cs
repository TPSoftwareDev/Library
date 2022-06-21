using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Teleperformance.Collections
{
	public class DictionaryChangeWrapper<TKey, TValue> : IDictionary<TKey, TValue>
	{
		public event EventHandler<DictionaryChangeEventArgs<TKey, TValue>> Changed;

		protected virtual void OnChanged(DictionaryChangeDescription change, params KeyValuePair<TKey, TValue>[] pairs)
		{
			EventHelper.RaiseEvent(this.Changed, this, new DictionaryChangeEventArgs<TKey, TValue>(change, pairs));
		}

		protected void OnChanged(DictionaryChangeDescription change, TKey key, TValue value)
		{
			this.OnChanged(change, new KeyValuePair<TKey, TValue>(key, value));
		}

		private IDictionary<TKey, TValue> dictionary;

		public DictionaryChangeWrapper(IDictionary<TKey, TValue> dictionary)
		{
			this.dictionary = dictionary;
		}

		#region IDictionary<TKey,TValue> Members

		public void Add(TKey key, TValue value)
		{
			this.dictionary.Add(key, value);

			this.OnChanged(DictionaryChangeDescription.KeyValuePairAdded, key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get { return this.dictionary.Keys; }
		}

		public bool Remove(TKey key)
		{
			TValue value = this.dictionary[key];
			bool result = this.dictionary.Remove(key);

			this.OnChanged(DictionaryChangeDescription.KeyValuePairAdded, key, value);

			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.dictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return this.dictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get { return this.dictionary[key]; }
			set
			{
				TValue oldValue = this.dictionary[key];
				TValue newValue = value;

				this.dictionary[key] = newValue;

				if (!oldValue.Equals(newValue))
				{
					this.OnChanged(DictionaryChangeDescription.KeyValuePairRemoved, key, oldValue);
					this.OnChanged(DictionaryChangeDescription.KeyValuePairAdded, key, newValue);
				}
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.dictionary.Add(item);

			this.OnChanged(DictionaryChangeDescription.KeyValuePairAdded, item.Key, item.Value);
		}

		public void Clear()
		{
			KeyValuePair<TKey, TValue>[] pairs = new KeyValuePair<TKey, TValue>[this.dictionary.Count];

			this.dictionary.CopyTo(pairs, 0);
			this.dictionary.Clear();

			this.OnChanged(DictionaryChangeDescription.Cleared, pairs);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.dictionary.Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.dictionary.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this.dictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return this.dictionary.IsReadOnly; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			bool result = this.dictionary.Remove(item);

			this.OnChanged(DictionaryChangeDescription.KeyValuePairRemoved, item.Key, item.Value);

			return result;
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this).GetEnumerator();
		}

		#endregion
	}
}