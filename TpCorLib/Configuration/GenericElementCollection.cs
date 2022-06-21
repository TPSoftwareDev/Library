using System.Configuration;

namespace Teleperformance.Configuration
{
	public abstract class GenericElementCollection<ElementT> : ConfigurationElementCollection
		where ElementT : ConfigurationElement, new()
	{
		#region Construction/Destruction 

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericElementCollection&lt;ElementT&gt;"/> class.
		/// </summary>
		public GenericElementCollection() { }

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		/// <summary>
		/// Gets or sets the <see cref="ElementT"/> at the specified index.
		/// </summary>
		/// <value></value>
		public ElementT this[int index]
		{
			get { return (ElementT)base.BaseGet(index); }
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}

				base.BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Gets the <see cref="ElementT"/> with the specified key.
		/// </summary>
		/// <value></value>
		public new ElementT this[string key]
		{
			get { return (ElementT)base.BaseGet(key); }
		}

		#endregion Public Property(ies) 

		#region Public Method(s) 

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public void Add(ElementT element)
		{
			base.BaseAdd(element);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			base.BaseClear();
		}

		public string[] GetAllKeys()
		{
			return (string[])base.BaseGetAllKeys();
		}

		public int IndexOf(ElementT element)
		{
			return base.BaseIndexOf(element);
		}

		public void Insert(int index, ElementT element)
		{
			base.BaseAdd(index, element);
		}

		/// <summary>
		/// Removes the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		#endregion Public Method(s) 

		#region Protected Method(s) 

		/// <summary>
		/// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new ElementT();
		}

		#endregion Protected Method(s) 
	}
}