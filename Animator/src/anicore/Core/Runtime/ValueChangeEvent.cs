using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Runtime
{

	//#region ValueChangeEvent

	//public class ValueChangeEvent
	//{

	//    #region Static / Constant

	//    #endregion

	//    #region Fields

	//    #endregion

	//    #region Properties

	//    #endregion

	//    #region Constructors

	//    #endregion

	//    #region Methods

	//    #endregion

	//}

	//#endregion

	#region ValueChangedEventArgs<T>

	internal sealed class ValueChangedEventArgs<T> : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly T _OriginalValue;
		private readonly T _NewValue;

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

		#endregion

		#region Constructors

		public ValueChangedEventArgs(T originalValue, T newValue)
		{
			this._OriginalValue = originalValue;
			this._NewValue = newValue;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
