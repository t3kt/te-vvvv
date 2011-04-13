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

		#region NullAppAction

		private sealed class NullAppAction : AppAction
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly bool _SupportsUndo;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public NullAppAction(bool supportsUndo)
			{
				this._SupportsUndo = supportsUndo;
			}

			#endregion

			#region Methods

			public override void Perform(object target, object newState, out object oldState, out bool canUndo)
			{
				oldState = null;
				canUndo = this._SupportsUndo;
			}

			public override void Undo(object target, object oldState)
			{

			}

			#endregion

		}

		#endregion

		#region Static / Constant

		internal static readonly AppAction Null = new NullAppAction(false);

		internal static readonly AppAction NullWithUndo = new NullAppAction(true);

		#endregion

		#region Fields

		#endregion

		#region Properties

		public virtual string Name
		{
			get { return "AppAction"; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public abstract void Perform(object target, object newState, out object oldState, out bool canUndo);

		public virtual void Undo(object target, object oldState)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

}
