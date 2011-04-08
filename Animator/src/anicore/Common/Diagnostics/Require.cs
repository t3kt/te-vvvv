using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Common.Diagnostics
{

	#region Require

	public static class Require
	{

		[AssertionMethod]
		public static void ArgNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object value, [InvokerParameterName]string name)
		{
			if(value == null)
				throw new ArgumentNullException(name);
		}

		public static void ArgNotNegative(int value, [InvokerParameterName]string name)
		{
			if(value < 0)
				throw new ArgumentOutOfRangeException(name);
		}

		public static void ArgPositive(int value, [InvokerParameterName]string name)
		{
			if(value <= 0)
				throw new ArgumentOutOfRangeException(name);
		}

	}

	#endregion

}
