using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.AppCore.Common
{

	#region DoubleLinkedStack<T>

	public class DoubleLinkedStack<T> : StackBase<T>
	{

		#region Node

		private sealed class Node
		{

			#region Static / Constant

			#endregion

			#region Fields

			public Node Prev;
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

			public Enumerator(Node head)
			{
				this._HasStarted = false;
				this._Node = head;
			}

			#endregion

			#region Methods

			#endregion

			#region IEnumerator<T> Members

			public T Current
			{
				get
				{
					if(!this._HasStarted || this._Node == null)
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

		#region ReverseEnumerator

		private struct ReverseEnumerator : IEnumerator<T>
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

			public ReverseEnumerator(Node tail)
			{
				this._HasStarted = false;
				this._Node = tail;
			}

			#endregion

			#region Methods

			#endregion

			#region IEnumerator<T> Members

			public T Current
			{
				get
				{
					if(!this._HasStarted || this._Node == null)
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
				this._Node = this._Node.Prev;
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

		#region ReverseEnumerable

		private struct ReverseEnumerable : IEnumerable<T>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly ReverseEnumerator _Enumerator;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public ReverseEnumerable(ReverseEnumerator enumerator)
			{
				this._Enumerator = enumerator;
			}

			#endregion

			#region Methods

			#endregion

			#region IEnumerable<T> Members

			public IEnumerator<T> GetEnumerator()
			{
				return this._Enumerator;
			}

			#endregion

			#region IEnumerable Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._Enumerator;
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private int _Count;
		private Node _Head;
		private Node _Tail;

		#endregion

		#region Properties

		public override int Count
		{
			get { return this._Count; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void Clear()
		{
			this._Head = this._Tail = null;
			this._Count = 0;
		}

		public override bool TryPeek(out T value)
		{
			if(this._Head == null)
			{
				value = default(T);
				return false;
			}
			value = this._Head.Value;
			return true;
		}

		public override bool TryPop(out T value)
		{
			if(!this.TryPeek(out value))
				return false;
			this._Head = this._Head.Next;
			if(this._Head != null)
				this._Head.Prev = null;
			this._Count--;
			return true;
		}

		public override void Push(T value)
		{
			var node = new Node(value) { Next = this._Head };
			if(this._Head != null)
				this._Head.Prev = node;
			else
				this._Tail = node;
			this._Head = node;
			this._Count++;
		}

		public virtual bool TryPeekEnd(out T value)
		{
			if(this._Tail == null)
			{
				value = default(T);
				return false;
			}
			value = this._Tail.Value;
			return true;
		}

		public virtual bool TryPopEnd(out T value)
		{
			if(!this.TryPeekEnd(out value))
				return false;
			this._Tail = this._Tail.Prev;
			if(this._Tail != null)
				this._Tail.Next = null;
			this._Count--;
			return true;
		}

		public virtual void PushEnd(T value)
		{
			var node = new Node(value) { Prev = this._Tail };
			if(this._Tail != null)
				this._Tail.Next = node;
			else
				this._Head = node;
			this._Tail = node;
			this._Count++;
		}

		public T PeekEnd()
		{
			T value;
			if(!this.TryPeekEnd(out value))
				throw new InvalidOperationException();
			return value;
		}

		public T PopEnd()
		{
			T value;
			if(!this.TryPopEnd(out value))
				throw new InvalidOperationException();
			return value;
		}

		public virtual void PushRangeEnd(IEnumerable<T> values)
		{
			Require.ArgNotNull(values, "values");
			foreach(var value in values)
				this.PushEnd(value);
		}

		public IEnumerator<T> GetReverseEnumerator()
		{
			return new ReverseEnumerator(this._Tail);
		}

		public IEnumerable<T> AsReversedEnumerable()
		{
			return new ReverseEnumerable(new ReverseEnumerator(this._Tail));
		}

		public override IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(this._Head);
		}

		#endregion

	}

	#endregion

}
