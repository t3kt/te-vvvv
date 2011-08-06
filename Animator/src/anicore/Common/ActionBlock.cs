using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Common
{

	#region ActionBlock

	internal sealed class ActionBlock : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private bool _Disposed;
		private readonly Action _EndAction;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ActionBlock(Action endAction)
			: this(null, endAction) { }

		public ActionBlock(Action startAction, Action endAction)
		{
			if(startAction != null)
				startAction();
			this._EndAction = endAction;
		}

		#endregion

		#region Methods

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(!this._Disposed && this._EndAction != null)
				this._EndAction();
			this._Disposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
