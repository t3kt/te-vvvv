using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.AppCore.Common
{

	#region LimitedStack<T>

	public class LimitedStack<T> : DoubleLinkedStack<T>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int _Capacity;

		#endregion

		#region Properties

		public int Capacity
		{
			get { return _Capacity; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(value != this._Capacity)
				{
					this._Capacity = value;
					this.Trim();
				}
			}
		}

		#endregion

		#region Constructors

		public LimitedStack(int capacity)
		{
			Require.ArgNotNegative(capacity, "capacity");
			this._Capacity = capacity;
		}

		#endregion

		#region Methods

		private void Trim()
		{
			if(this._Capacity == 0)
			{
				this.Clear();
			}
			else
			{
				while(this.Count > this._Capacity)
					this.PopEnd();
			}
		}

		public override void Push(T value)
		{
			base.Push(value);
			this.Trim();
		}

		public override void PushEnd(T value)
		{
			throw new NotSupportedException();
		}

		private new T PopEnd()
		{
			T value;
			if(!base.TryPopEnd(out value))
				throw new InvalidOperationException();
			return value;
		}

		public override bool TryPopEnd(out T value)
		{
			throw new NotSupportedException();
		}

		public override void PushRange(IEnumerable<T> values)
		{
			Require.ArgNotNull(values, "values");
			foreach(var value in values)
				base.Push(value);
			this.Trim();
		}

		public override void PushRangeEnd(IEnumerable<T> values)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

}
