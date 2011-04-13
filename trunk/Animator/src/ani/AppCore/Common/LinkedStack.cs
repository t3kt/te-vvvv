using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.AppCore.Common
{

	#region LinkedStack<T>

	public class LinkedStack<T> : StackBase<T>
	{

		#region Node

		private sealed class Node
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

		#region Static / Constant

		#endregion

		#region Fields

		private Node _TopNode;
		private int _Count;

		#endregion

		#region Properties

		public override int Count
		{
			get { return _Count; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void Clear()
		{
			this._TopNode = null;
			this._Count = 0;
		}

		public override bool TryPeek(out T value)
		{
			if(this._TopNode == null)
			{
				value = default(T);
				return false;
			}
			value = this._TopNode.Value;
			return true;
		}

		public override bool TryPop(out T value)
		{
			if(!this.TryPeek(out value))
				return false;
			this._TopNode = this._TopNode.Next;
			this._Count--;
			return true;
		}

		public override void Push(T value)
		{
			var node = new Node(value) { Next = this._TopNode };
			this._TopNode = node;
			this._Count++;
		}

		#endregion

		#region IEnumerable<T> Members

		//internal IEnumerator<T> GetEnumeratorInternal()
		//{
		//    return new Enumerator(this._TopNode);
		//}

		//internal IEnumerator<T> GetEnumeratorIterator()
		//{
		//    var node = this._TopNode;
		//    while(node != null)
		//    {
		//        yield return node.Value;
		//        node = node.Next;
		//    }
		//}

		public override IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(this._TopNode);
		}

		#endregion

	}

	#endregion

}
