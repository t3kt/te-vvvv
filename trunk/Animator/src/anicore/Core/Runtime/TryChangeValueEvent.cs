using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Resources;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region TryChangeValueDecision

	internal enum TryChangeValueDecision
	{
		None,
		Approved,
		Denied,
		ModifiedApproved
	}

	#endregion

	#region TryChangeValueEventArgs<T>

	internal sealed class TryChangeValueEventArgs<T> : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly T _OriginalValue;
		private TryChangeValueDecision _Decision;
		private T _NewValue;

		#endregion

		#region Properties

		public T OriginalValue
		{
			get { return this._OriginalValue; }
		}

		public T NewValue
		{
			get { return this._NewValue; }
		}

		public TryChangeValueDecision Decision
		{
			get { return this._Decision; }
		}

		#endregion

		#region Constructors

		public TryChangeValueEventArgs(T originalValue, T newValue)
		{
			this._OriginalValue = originalValue;
			this._NewValue = newValue;
		}

		private TryChangeValueEventArgs([NotNull] TryChangeValueEventArgs<T> other, bool withDecision)
		{
			this._OriginalValue = other._OriginalValue;
			this._NewValue = other._NewValue;
			if(withDecision)
				this._Decision = other._Decision;
		}

		#endregion

		#region Methods

		internal TryChangeValueEventArgs<T> CloneWithoutDecision()
		{
			return new TryChangeValueEventArgs<T>(this, false);
		}

		public void Deny()
		{
			this._Decision = TryChangeValueDecision.Denied;
		}

		public void Approve()
		{
			this._Decision = TryChangeValueDecision.Approved;
		}

		public void ApproveModified(T modValue)
		{
			this._Decision = TryChangeValueDecision.ModifiedApproved;
			this._NewValue = modValue;
		}

		#endregion

	}

	#endregion

	#region TryChangeValueUtil

	internal static class TryChangeValueUtil
	{

		internal static bool IsApproved(TryChangeValueDecision decision)
		{
			return decision == TryChangeValueDecision.Approved || decision == TryChangeValueDecision.ModifiedApproved;
		}

		internal static TryChangeValueDecision ApplyHandler<T>(EventHandler<TryChangeValueEventArgs<T>> handler, object sender, T origValue, ref T newValue, bool defaultApproved = true)
		{
			TryChangeValueEventArgs<T> e;
			ApplyHandler(handler, sender, origValue, ref newValue, out e, defaultApproved);
			return e.Decision;
		}

		internal static bool ApplyHandler<T>(EventHandler<TryChangeValueEventArgs<T>> handler, object sender, T origValue, ref T newValue, out TryChangeValueEventArgs<T> e, bool defaultApproved = true)
		{
			e = new TryChangeValueEventArgs<T>(origValue, newValue);
			if(handler != null)
				handler(sender, e);
			if(e.Decision == TryChangeValueDecision.None)
			{
				if(defaultApproved)
					e.Approve();
				else
					e.Deny();
			}
			switch(e.Decision)
			{
			case TryChangeValueDecision.Denied:
				return false;
			case TryChangeValueDecision.Approved:
				return true;
			case TryChangeValueDecision.ModifiedApproved:
				newValue = e.NewValue;
				return true;
			//case TryChangeValueDecision.None:
			default:
				throw new InvalidOperationException();
			}
		}

	}

	#endregion

}
