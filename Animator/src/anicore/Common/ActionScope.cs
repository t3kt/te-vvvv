using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Common
{

	#region ActionScope

	internal sealed class ActionScope : IDisposable
	{

		#region Static / Constant

		public static readonly ActionScope Null = new ActionScope(null);

		#endregion

		#region Fields

		private readonly Action _Action;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ActionScope(Action action)
		{
			_Action = action;
		}

		#endregion

		#region Methods

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_Action != null)
				_Action();
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
