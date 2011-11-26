using System;
using System.Collections.Generic;
using System.Linq;
using TESharedAnnotations;

namespace TEShared
{

	internal static class Require
	{

		[AssertionMethod]
		public static void ArgNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object value, [InvokerParameterName]string name)
		{
			if(value == null)
				throw new ArgumentNullException(name);
		}

		[AssertionMethod]
		public static void ArgNotNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string value, [InvokerParameterName]string name)
		{
			if(value == null)
				throw new ArgumentNullException(name);
			if(value.Length == 0)
				throw new ArgumentException("Value cannot be empty", name);
		}

	}

}
