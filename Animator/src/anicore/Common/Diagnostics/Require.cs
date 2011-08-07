using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Animator.Resources;
using TESharedAnnotations;

namespace Animator.Common.Diagnostics
{

	#region Require

	[DebuggerNonUserCode]
	public static class Require
	{

		[AssertionMethod]
		public static void ArgNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object value, [InvokerParameterName]string name)
		{
			if(value == null)
				throw new ArgumentNullException(name);
		}

		[AssertionMethod]
		[Conditional("DEBUG")]
		internal static void DBG_ArgNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object value, [InvokerParameterName]string name)
		{
			ArgNotNull(value, name);
		}

		[AssertionMethod]
		public static void ArgNotNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]string value, [InvokerParameterName]string name)
		{
			if(value == null)
				throw new ArgumentNullException(name);
			if(value.Length == 0)
				throw new ArgumentException(CoreStrings.Require_ArgNotNullOrEmpty, name);
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

		public static void ArgPositive(double value, [InvokerParameterName]string name)
		{
			if(value <= 0)
				throw new ArgumentOutOfRangeException(name);
		}

		public static void ArgNotNegative(TimeSpan value, [InvokerParameterName]string name)
		{
			if(value < TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(name);
		}

		public static void ArgPositive(TimeSpan value, [InvokerParameterName]string name)
		{
			if(value <= TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(name);
		}

		public static void ArgTypeIs<TExpected>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object value, [InvokerParameterName]string name)
		{
			if(!(value is TExpected))
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgMustBeOfType, typeof(TExpected)), name);
		}

		public static void ArgNotZero(double value, [InvokerParameterName]string name)
		{
			if(value == 0)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgNotZero, name), name);
		}

		public static void ArgNotZero(long value, [InvokerParameterName]string name)
		{
			if(value == 0)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgNotZero, name), name);
		}

		public static void ArgGreaterThan(double value, [InvokerParameterName]string name, double comparand)
		{
			if(value <= comparand)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgGreaterThan, name, value, comparand), name);
		}

		public static void ArgLessThan(double value, [InvokerParameterName]string name, double comparand)
		{
			if(value >= comparand)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgLessThan, name, value, comparand), name);
		}

		public static void ArgGreaterThanOrEqualTo(double value, [InvokerParameterName]string name, double comparand)
		{
			if(value < comparand)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgGreaterThanOrEqualTo, name, value, comparand), name);
		}

		public static void ArgLessThanOrEqualTo(double value, [InvokerParameterName]string name, double comparand)
		{
			if(value > comparand)
				throw new ArgumentException(String.Format(CoreStrings.Require_ArgLessThanOrEqualTo, name, value, comparand), name);
		}

		[Conditional("DEBUG")]
		public static void DBG_ArgGreaterThanOrEqualTo(double value, [InvokerParameterName]string name, double comparand)
		{
			ArgGreaterThanOrEqualTo(value, name, comparand);
		}

		[Conditional("DEBUG")]
		public static void DBG_ArgLessThanOrEqualTo(double value, [InvokerParameterName]string name, double comparand)
		{
			ArgLessThanOrEqualTo(value, name, comparand);
		}

		[Conditional("DEBUG")]
		public static void DBG_ArgGreaterThan(double value, [InvokerParameterName]string name, double comparand)
		{
			ArgGreaterThan(value, name, comparand);
		}

		[Conditional("DEBUG")]
		public static void DBG_ArgLessThan(double value, [InvokerParameterName]string name, double comparand)
		{
			ArgLessThan(value, name, comparand);
		}

	}

	#endregion

}
