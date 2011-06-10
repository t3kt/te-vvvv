using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.Utils
{

	#region AniAssert

	internal static class AniAssert
	{

		public static void AreEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message = null, params object[] args)
		{
			if(comparer == null)
				comparer = EqualityComparer<T>.Default;
			if(!comparer.Equals(expected, actual))
			{
				if(message != null && args != null && args.Length > 0)
					message = String.Format(message, args);
				Assert.Fail(String.Format("AreEqual failed. Expected: <{0}> Actual: <{1}> {2}", expected, actual, message));
			}
		}

	}

	#endregion

}
