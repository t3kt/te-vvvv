using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Animator.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.Utils
{

	#region TestUtil

	internal static class TestUtil
	{

		internal static void AssertThrowsException<TException>(Action action, string failMessage = null)
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

		internal static void AssertThrowsException(Action action, string failMessage = null)
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

		internal static void AssertCounterEquals(int expected, EventCounter counter, bool reset = false)
		{
			Assert.AreEqual(expected, counter.Count);
			if(reset)
				counter.Reset();
		}

	}

	#endregion

}
