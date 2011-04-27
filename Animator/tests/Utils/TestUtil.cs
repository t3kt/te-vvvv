using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.Utils
{

	#region TestUtil

	internal static class TestUtil
	{

		internal static void AssertThrowsException<TException>(Action action, string failMessage=null)
			where TException : Exception
		{
			try
			{
				action();
				Assert.Fail(failMessage);
			}
			catch(TException)
			{
			}
		}

		internal static void AssertThrowsException(Action action, string failMessage=null)
		{
			try
			{
				action();
				Assert.Fail(failMessage);
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch
			// ReSharper restore EmptyGeneralCatchClause
			{
			}
		}

	}

	#endregion

}
