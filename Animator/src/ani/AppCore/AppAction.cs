using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.AppCore
{

	#region AppAction

	internal abstract class AppAction : IAppAction
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public virtual bool SupportsUndo
		{
			get { return false; }
		}

		public virtual string Name
		{
			get { return "AppAction"; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public abstract object Perform(object target, object newState);

		public virtual void Undo(object target, object oldState)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

}
