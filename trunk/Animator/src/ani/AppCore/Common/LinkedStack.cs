using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Animator.Common.Diagnostics;

namespace Animator.AppCore.Common
{

	#region LinkedStack<T>

	public class LinkedStack<T> : IEnumerable<T>, ICollection
	{

		#region Node

		protected sealed class Node
		{

			#region Static / Constant

			#endregion

			#region Fields

			public readonly T Value;
			public Node Next;

			#endregion

			#region Properties

			public Node(T value)
			{
				this.Value = value;
			}

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion
		}

		#endregion

		#region Enumerator

		private struct Enumerator : IEnumerator<T>
		{

			#region Static / Constant

			#endregion

			#region Fields

			private bool _HasStarted;
			private Node _Node;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public Enumerator(Node start)
			{
				this._HasStarted = false;
				this._Node = start;
			}

			#endregion

			#region Methods

			#endregion

			#region IEnumerator<T> Members

			public T Current
			{
				get
				{
					if(this._HasStarted || this._Node == null)
						throw new InvalidOperationException();
					return this._Node.Value;
				}
			}

			#endregion

			#region IEnumerator Members

			object IEnumerator.Current
			{
				get { return this.Current; }
			}

			public bool MoveNext()
			{
				if(!this._HasStarted)
				{
					this._HasStarted = true;
					return this._Node != null;
				}
				if(this._Node == null)
					return false;
				this._Node = this._Node.Next;
				return this._Node != null;
			}

			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				this._Node = null;
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private Node _TopNode;
		private object _SyncRoot;

		#endregion

		#region Properties

		public bool IsEmpty
		{
			get { return this._TopNode != null; }
		}

		#endregion

		#region Constructors

		public LinkedStack()
		{
		}

		public LinkedStack(IEnumerable<T> values)
		{
			this.PushRange(values);
		}

		#endregion

		#region Methods

		public virtual void Clear()
		{
			this._TopNode = null;
		}

		public virtual bool TryPeek(out T value)
		{
			if(this._TopNode == null)
			{
				value = default(T);
				return false;
			}
			value = this._TopNode.Value;
			return true;
		}

		public T Peek()
		{
			T value;
			if(!this.TryPeek(out value))
				throw new InvalidOperationException();
			return value;
		}

		public virtual bool TryPop(out T value)
		{
			if(!this.TryPeek(out value))
				return false;
			this._TopNode = this._TopNode.Next;
			return true;
		}

		public T Pop()
		{
			T value;
			if(!this.TryPop(out value))
				throw new InvalidOperationException();
			return value;
		}

		public virtual void Push(T value)
		{
			var node = new Node(value);
			node.Next = this._TopNode;
			this._TopNode = node;
		}

		public void PushRange(IEnumerable<T> values)
		{
			Require.ArgNotNull(values, "values");
			foreach(var value in values)
				this.Push(value);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(this._TopNode);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			this.ToArray().CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return this.Count(); }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get
			{
				if(this._SyncRoot == null)
					Interlocked.CompareExchange(ref this._SyncRoot, new object(), null);
				return this._SyncRoot;
			}
		}

		#endregion

	}

	#endregion

}
