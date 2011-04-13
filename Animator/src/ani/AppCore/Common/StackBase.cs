using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animator.Common.Diagnostics;

namespace Animator.AppCore.Common
{

	#region StackBase<T>

	public abstract class StackBase<T> : IEnumerable<T>, ICollection
	{

		#region Static/Constant

		#endregion

		#region Fields

		private object _SyncRoot;

		#endregion

		#region Properties

		public abstract int Count { get; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public abstract void Clear();

		public abstract bool TryPeek(out T value);

		public abstract bool TryPop(out T value);

		public abstract void Push(T value);

		public T Peek()
		{
			T value;
			if(!this.TryPeek(out value))
				throw new InvalidOperationException();
			return value;
		}

		public T Pop()
		{
			T value;
			if(!this.TryPop(out value))
				throw new InvalidOperationException();
			return value;
		}

		public virtual void PushRange(IEnumerable<T> values)
		{
			Require.ArgNotNull(values, "values");
			foreach(var value in values)
				this.Push(value);
		}

		#endregion

		#region IEnumerable Members

		public abstract IEnumerator<T> GetEnumerator();

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