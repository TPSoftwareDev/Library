using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Teleperformance.Collections
{
	// TODO Justin Long: This should probably change to a FileCache or something (inherit from System.Runtime.Caching.ObjectCache)

	// TODO Justin Long: Change this to use a linked list as an index of all of the items start positions to get better performance

	public class FilePersistedStack<T> : IEnumerable<T>, ICollection, IEnumerable, IDisposable
		where T : class
	{
		#region Field(s) 

		private readonly object syncRoot = new object();
		private LinkedList<long> index;
		private FileStream queueFileStream;
		private bool disposed = false;

		#endregion Field(s) 

		#region Construction/Destruction 

		/// <summary>
		/// Initializes a new instance of the <see cref="FilePersistedQueue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="collection">The collection.</param>
		public FilePersistedStack(string filePath, IEnumerable<T> collection)
			: this(filePath)
		{
			foreach (T item in collection)
			{
				this.Push(item);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilePersistedQueue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="filePath">The file path to the file that will be used to store the queued items.</param>
		/// <remarks>
		/// If the file provided already exists the queue will be re-hydrated based on the file contents.
		/// </remarks>
		public FilePersistedStack(string filePath)
		{
			this.FilePath = filePath;

			this.queueFileStream = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
		}

		~FilePersistedStack()
		{
			this.Dispose(false);
		}

		#endregion Construction/Destruction 

		#region Public Property(ies) 

		public int Count
		{
			get
			{
				if (disposed)
				{
					throw new ObjectDisposedException("FilePersistedStack<T>");
				}

				lock (syncRoot)
				{
					return this.Index.Count;
				}
			}
		}

		public string FilePath { get; protected set; }

		public bool IsSynchronized { get { return true; } }

		public object SyncRoot { get { return syncRoot; } }

		#endregion Public Property(ies) 

		#region Protected Property(ies) 

		protected LinkedList<long> Index
		{
			get
			{
				if (null == this.index)
				{
					this.index = this.BuildIndex();
				}

				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		#endregion Protected Property(ies) 

		#region Public Method(s) 

		public virtual void Clear()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			lock (syncRoot)
			{
				this.queueFileStream.SetLength(0);
				this.index = null;
			}
		}

		public virtual bool Contains(T item)
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			throw new NotImplementedException();
		}

		public virtual T Pop()
		{

			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			lock (syncRoot)
			{
				if (null == this.Index.Last || this.Index.Last.Value == this.queueFileStream.Length)
				{
					throw new InvalidOperationException("No item to pop.");
				}

				long lastItemStartPosition = this.Index.Last.Value;
				long lastItemLength;
				byte[] itemBuffer;

				this.queueFileStream.Seek(lastItemStartPosition, SeekOrigin.Begin);

				lastItemLength = this.ReadItemLength();
				itemBuffer = new byte[lastItemLength];

				this.queueFileStream.Read(itemBuffer, 0, itemBuffer.Length);

				using (MemoryStream memoryStream = new MemoryStream(itemBuffer))
				{
					BinaryFormatter binaryFormater = new BinaryFormatter();

					binaryFormater.AssemblyFormat = FormatterAssemblyStyle.Simple;

					T item = (T)binaryFormater.Deserialize(memoryStream);

					this.queueFileStream.SetLength(lastItemStartPosition);
					this.Index.RemoveLast();

					return item;
				}
			}
		}

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		public virtual void Push(T item)
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			lock (syncRoot)
			{
				BinaryFormatter binaryFormater = new BinaryFormatter();

				binaryFormater.AssemblyFormat = FormatterAssemblyStyle.Simple;

				long itemStartIndex = this.queueFileStream.Seek(0, SeekOrigin.End);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					binaryFormater.Serialize(memoryStream, item);

					this.WriteItemsLength(memoryStream.Length);

					memoryStream.WriteTo(this.queueFileStream);
				}

				this.queueFileStream.Flush();
				this.Index.AddLast(itemStartIndex);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			throw new NotImplementedException();
		}

		public virtual T Peek()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			throw new NotImplementedException();

		}

		public virtual void RebuildIndex()
		{
			lock (syncRoot)
			{
				this.Index = this.BuildIndex();
			}
		}

		#endregion Public Method(s) 

		#region Protected Method(s) 

		protected virtual LinkedList<long> BuildIndex()
		{
			LinkedList<long> index = new LinkedList<long>();

			if (this.queueFileStream.Length > 0)
			{
				this.queueFileStream.Seek(0, SeekOrigin.Begin);

				while (this.queueFileStream.Position < this.queueFileStream.Length)
				{
					index.AddLast(this.queueFileStream.Position);

					long itemLength = this.ReadItemLength();

					this.queueFileStream.Seek(itemLength, SeekOrigin.Current);
				}
			}
			return index;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (null != this.queueFileStream)
				{
					this.queueFileStream.Dispose();
				}
			}

			disposed = true;
		}

		protected virtual long ReadItemLength()
		{
			byte[] lengthBytes = new byte[8];

			this.queueFileStream.Read(lengthBytes, 0, 8);

			return BitConverter.ToInt64(lengthBytes, 0);
		}

		protected virtual void WriteItemsLength(long length)
		{
			byte[] lengthBytes = BitConverter.GetBytes(length);

			this.queueFileStream.Write(lengthBytes, 0, lengthBytes.Length);
		}

		#endregion Protected Method(s) 

		#region Private Method(s) 

		void ICollection.CopyTo(Array array, int index)
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("FilePersistedStack<T>");
			}

			throw new NotImplementedException();
		}

		#endregion Private Method(s) 
	}
}