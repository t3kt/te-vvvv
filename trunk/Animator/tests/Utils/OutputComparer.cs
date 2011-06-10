using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region OutputComparer

	internal sealed class OutputComparer : DocumentItemComparer<Output>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly bool _UnrecognizedEqual;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public OutputComparer(bool unrecognizedEqual)
		{
			this._UnrecognizedEqual = unrecognizedEqual;
		}

		#endregion

		#region Methods

		protected override bool EqualsInternal(Output x, Output y)
		{
			if(!ModelUtil.ItemsEqual(x.Targets, y.Targets, TargetObjectComparer.Default))
				return false;
			if(x is TraceOutput && y is TraceOutput)
			{
				var xx = (TraceOutput)x;
				var yy = (TraceOutput)y;
				return xx.Category == yy.Category &&
					   xx.Prefix == yy.Prefix;
			}
			if(x is LogOutput && y is LogOutput)
			{
				var xx = (LogOutput)x;
				var yy = (LogOutput)y;
				return xx.Path == yy.Path &&
					   xx.Append == yy.Append;
			}
			if(x is LogOutput && y is LogOutput)
			{
				var xx = (LogOutput)x;
				var yy = (LogOutput)y;
				return xx.Path == yy.Path &&
					   xx.Append == yy.Append;
			}
			return this._UnrecognizedEqual;
		}

		#endregion

	}

	#endregion

}
