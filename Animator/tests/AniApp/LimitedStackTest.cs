using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore.Common;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression


	/// <summary>
	///This is a test class for LimitedStack and is intended
	///to contain all LimitedStackTest Unit Tests
	///</summary>
	[TestClass]
	public class LimitedStackTest
	{

		private readonly ReadOnlyCollection<int> _TestValues = new ReadOnlyCollection<int>(new List<int> { 10, 20, 30, 40 });

		public TestContext TestContext { get; set; }

		private int[] GetTestValuesForPush()
		{
			int[] values = _TestValues.Reverse().ToArray();
			return values;
		}

		private LimitedStack<int> CreateWithTestValues(int capacity)
		{
			LimitedStack<int> target = new LimitedStack<int>(capacity);
			this.AddTestValues(target);
			Assert.AreEqual(Math.Min(_TestValues.Count, capacity), target.Count);
			CollectionAssert.AreEqual(_TestValues.Take(capacity).ToList(), target);
			return target;
		}

		private void AddTestValues(LimitedStack<int> stack)
		{
			Debug.Assert(stack != null);
			IEnumerable<int> values = this.GetTestValuesForPush();
			stack.PushRange(values);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void CreateLimitedStackWithValuesTest()
		{
			var stackInf = CreateWithTestValues(Int32.MaxValue);
			CollectionAssert.AreEqual(_TestValues, stackInf);
			var capacityA = _TestValues.Count;
			var stackLimA = this.CreateWithTestValues(capacityA);
			CollectionAssert.AreEqual(_TestValues, stackLimA);
			var capacityB = _TestValues.Count - 1;
			var stackLimB = this.CreateWithTestValues(capacityB);
			CollectionAssert.AreEqual(_TestValues.Take(capacityB).ToList(), stackLimB);
		}

		private LimitedStack<T> CreateEmpty<T>(int capacity)
		{
			LimitedStack<T> target = new LimitedStack<T>(capacity);
			Assert.AreEqual(0, target.Count);
			Assert.AreEqual(0, target.Count());
			Assert.IsFalse(target.Any());
			return target;
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void CreateEmptyLimitedStackTest()
		{
			this.CreateEmpty<GenericParameterHelper>(Int32.MaxValue);
			this.CreateEmpty<GenericParameterHelper>(2);
			TestUtil.AssertThrowsException<ArgumentException>(() =>
															  {
																  this.CreateEmpty<GenericParameterHelper>(-2);
															  });
		}

		private void ClearTestHelper<T>(int capacity)
		{
			LimitedStack<T> target = new LimitedStack<T>(capacity);
			target.Push(default(T));
			target.Push(default(T));
			target.Push(default(T));
			target.Clear();
			Assert.AreEqual(0, target.Count);
			Assert.IsFalse(target.Any());
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void ClearTest()
		{
			this.ClearTestHelper<GenericParameterHelper>(Int32.MaxValue);
			this.ClearTestHelper<GenericParameterHelper>(1);
			this.ClearTestHelper<GenericParameterHelper>(0);
		}

		private void PeekTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.Peek();
																	  }, "Peek was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			int top;
			if(target.TryPeek(out top))
				Assert.AreEqual(expected, top, "top != expected [capacity: {0}]", capacity);
			Assert.AreEqual(origCount, target.Count, "count changed [capacity: {0}]", capacity);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void PeekTest()
		{
			this.PeekTestHelper(Int32.MaxValue);
			this.PeekTestHelper(_TestValues.Count);
			this.PeekTestHelper(_TestValues.Count - 1);
			this.PeekTestHelper(_TestValues.Count - 2);
			this.PeekTestHelper(0);
		}

		private void PeekEndTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			{
				var z = target.PeekEnd();
			}, "PeekEnd was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.Take(capacity).Last();
			int bottom;
			if(target.TryPeekEnd(out bottom))
				Assert.AreEqual(expected, bottom, "bottom != expected [capacity: {0}]", capacity);
			Assert.AreEqual(origCount, target.Count, "count changed [capacity: {0}]", capacity);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void PeekEndTest()
		{
			this.PeekEndTestHelper(Int32.MaxValue);
			this.PeekEndTestHelper(_TestValues.Count);
			this.PeekEndTestHelper(_TestValues.Count - 1);
			this.PeekEndTestHelper(_TestValues.Count - 2);
			this.PeekTestHelper(0);
		}

		private void PopTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			{
				var z = target.Pop();
			}, "Pop was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			if(this.CanPeek(target))
			{
				int actual = target.Pop();
				Assert.AreEqual(expected, actual, "top != expected [capacity: {0}]", capacity);
				Assert.AreEqual(origCount - 1, target.Count, "incorrect count change [capacity: {0}]", capacity);
			}
		}

		private bool CanPeek<T>(LimitedStack<T> stack)
		{
			T value;
			return stack.TryPeek(out value);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void PopTest()
		{
			this.PopTestHelper(Int32.MaxValue);
			this.PopTestHelper(_TestValues.Count);
			this.PopTestHelper(_TestValues.Count - 1);
			this.PopTestHelper(_TestValues.Count - 2);
			this.PopTestHelper(0);
		}

		private bool CanPeekEnd<T>(LimitedStack<T> stack)
		{
			T value;
			return stack.TryPeekEnd(out value);
		}

		private void PopEndTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			{
				var z = target.PopEnd();
			}, "PopEnd was allowed on empty stack");
			this.AddTestValues(target);
			if(capacity <= _TestValues.Count && this.CanPeekEnd(target))
			{
				var origCount = target.Count;
				int expected = this._TestValues.Take(capacity).Last();
				int actual = target.PopEnd();
				Assert.AreEqual(expected, actual, "bottom != expected [capacity: {0}]", capacity);
				Assert.AreEqual(origCount - 1, target.Count, "incorrect count change [capacity: {0}]", capacity);
			}
		}

		//[TestMethod]
		//[TestCategory("LimitedStack")]
		//public void PopEndTest()
		//{
		//    this.PopEndTestHelper(Int32.MaxValue);
		//    this.PopEndTestHelper(_TestValues.Count);
		//    this.PopEndTestHelper(_TestValues.Count - 1);
		//    this.PopEndTestHelper(_TestValues.Count - 2);
		//    this.PopEndTestHelper(0);
		//}

		private void PushTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateWithTestValues(capacity);
			var origCount = target.Count;
			int value = 999;
			target.Push(value);
			int top;
			if(target.TryPeek(out top))
				Assert.AreEqual(value, top);
			Assert.AreEqual(Math.Min(origCount + 1, capacity), target.Count);
			CollectionAssert.AreEqual(new[] { value }.Concat(_TestValues).Take(capacity).ToArray(), target);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void PushTest()
		{
			this.PushTestHelper(Int32.MaxValue);
			this.PushTestHelper(_TestValues.Count);
			this.PushTestHelper(_TestValues.Count - 1);
			this.PushTestHelper(_TestValues.Count - 2);
			this.PushTestHelper(0);
		}

		//private void PushEndTestHelper(int capacity)
		//{
		//    LimitedStack<int> target = this.CreateWithTestValues(capacity);
		//    var origCount = target.Count;
		//    int value = 999;
		//    target.PushEnd(value);
		//    int actual;
		//    if(target.TryPeekEnd(out actual))
		//        Assert.AreEqual(value, actual);
		//    Assert.AreEqual(Math.Min(origCount + 1, capacity), target.Count);
		//    CollectionAssert.AreEqual(_TestValues.Concat(new[] { value }).Take(capacity).ToArray(), target);
		//}

		//[TestMethod]
		//[TestCategory("LimitedStack")]
		//public void PushEndTest()
		//{
		//    LimitedStack<int> target = this.CreateWithTestValues(Int32.MaxValue);
		//    var origCount = target.Count;
		//    int value = 999;
		//    target.PushEnd(value);
		//    int bottom = target.PeekEnd();
		//    Assert.AreEqual(value, bottom);
		//    Assert.AreEqual(origCount + 1, target.Count);
		//    CollectionAssert.AreEqual(_TestValues.Concat(new[] { value }).ToArray(), target);
		//    this.PushEndTestHelper(Int32.MaxValue);
		//    this.PushEndTestHelper(_TestValues.Count);
		//    this.PushEndTestHelper(_TestValues.Count - 1);
		//    this.PushEndTestHelper(_TestValues.Count - 2);
		//    this.PushEndTestHelper(0);
		//}

		private void PushRangeTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			var testValuesForPush = this.GetTestValuesForPush();
			target.PushRange(testValuesForPush);
			CollectionAssert.AreEqual(_TestValues.Take(capacity).ToList(), target);
			target.PushRange(testValuesForPush);
			Assert.AreEqual(Math.Min(capacity, _TestValues.Count * 2), target.Count);
			CollectionAssert.AreEqual(_TestValues.Concat(_TestValues).Take(capacity).ToArray(), target);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void PushRangeTest()
		{
			this.PushRangeTestHelper(Int32.MaxValue);
			this.PushRangeTestHelper(_TestValues.Count);
			this.PushRangeTestHelper(_TestValues.Count * 2);
			this.PushRangeTestHelper(_TestValues.Count * 2 + 1);
			this.PushRangeTestHelper(_TestValues.Count * 2 - 1);
			this.PushRangeTestHelper(_TestValues.Count - 1);
			this.PushRangeTestHelper(_TestValues.Count - 2);
			this.PushRangeTestHelper(0);
		}

		//[TestMethod]
		//[TestCategory(CategoryNames.LimitedStack)]
		//public void PushRangeEndTest()
		//{
		//    LimitedStack<int> target = this.CreateEmpty<int>(Int32.MaxValue);
		//    var testValuesForPush = this.GetTestValuesForPush();
		//    target.PushRangeEnd(testValuesForPush);
		//    CollectionAssert.AreEqual(_TestValues.Reverse().ToList(), target);
		//    target.PushRangeEnd(testValuesForPush);
		//    Assert.AreEqual(_TestValues.Count * 2, target.Count);
		//    CollectionAssert.AreEqual(_TestValues.Concat(_TestValues).Reverse().ToArray(), target);
		//}

		private void GetEnumeratorNonEmptyTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateWithTestValues(capacity);
			IEnumerator<int> enumerator = target.GetEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			{
				var z = enumerator.Current;
			}, "Current allowed on non-started enumerator");
			List<int> actualContents = new List<int>();
			while(enumerator.MoveNext())
			{
				actualContents.Add(enumerator.Current);
			}
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			{
				var z = enumerator.Current;
			}, "Current allowed on finished enumerator");
			List<int> expectedContents = _TestValues.Take(capacity).ToList();
			CollectionAssert.AreEqual(expectedContents, actualContents);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		[DeploymentItem("ani.exe")]
		public void GetEnumeratorNonEmptyTest()
		{
			this.GetEnumeratorNonEmptyTestHelper(Int32.MaxValue);
			this.GetEnumeratorNonEmptyTestHelper(_TestValues.Count);
			this.GetEnumeratorNonEmptyTestHelper(_TestValues.Count - 1);
			this.GetEnumeratorNonEmptyTestHelper(_TestValues.Count - 2);
			this.GetEnumeratorNonEmptyTestHelper(0);
		}

		private void TryPeekTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			int value;
			Assert.IsFalse(target.TryPeek(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			if(capacity > 0)
			{
				Assert.IsTrue(target.TryPeek(out value));
				Assert.AreEqual(target.First(), value);
				Assert.AreEqual(_TestValues.First(), value);
			}
			Assert.AreEqual(Math.Min(capacity, origCount), target.Count);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void TryPeekTest()
		{
			this.TryPeekTestHelper(Int32.MaxValue);
			this.TryPeekTestHelper(_TestValues.Count);
			this.TryPeekTestHelper(_TestValues.Count - 1);
			this.TryPeekTestHelper(_TestValues.Count - 2);
			this.TryPeekTestHelper(0);
		}

		//[TestMethod]
		//[TestCategory("LimitedStack")]
		//public void TryPeekEndTest()
		//{
		//    LimitedStack<int> target = this.CreateEmpty<int>(Int32.MaxValue);
		//    int value;
		//    Assert.IsFalse(target.TryPeekEnd(out value));
		//    this.AddTestValues(target);
		//    var origCount = target.Count;
		//    Assert.IsTrue(target.TryPeekEnd(out value));
		//    Assert.AreEqual(target.Last(), value);
		//    Assert.AreEqual(_TestValues.Last(), value);
		//    Assert.AreEqual(origCount, target.Count);
		//}

		private void TryPopTestHelper(int capacity)
		{
			LimitedStack<int> target = this.CreateEmpty<int>(capacity);
			int value;
			Assert.IsFalse(target.TryPop(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			if(capacity > 0)
			{
				Assert.IsTrue(target.TryPop(out value));
				Assert.AreEqual(_TestValues.First(), value);
				Assert.AreEqual(_TestValues[1], target.First());
			}
			Assert.AreEqual(Math.Max(0, Math.Min(capacity, origCount - 1)), target.Count);
		}

		[TestMethod]
		[TestCategory(CategoryNames.LimitedStack)]
		public void TryPopTest()
		{
			this.TryPopTestHelper(Int32.MaxValue);
			this.TryPopTestHelper(_TestValues.Count);
			this.TryPopTestHelper(_TestValues.Count - 1);
			this.TryPopTestHelper(_TestValues.Count - 2);
			this.TryPopTestHelper(0);
		}

		//[TestMethod]
		//[TestCategory("LimitedStack")]
		//public void TryPopEndTest()
		//{
		//    LimitedStack<int> target = this.CreateEmpty<int>(Int32.MaxValue);
		//    int value;
		//    Assert.IsFalse(target.TryPopEnd(out value));
		//    this.AddTestValues(target);
		//    var origCount = target.Count;
		//    Assert.IsTrue(target.TryPopEnd(out value));
		//    Assert.AreEqual(_TestValues.Last(), value);
		//    Assert.AreEqual(origCount - 1, target.Count);
		//    Assert.AreEqual(_TestValues[_TestValues.Count - 2], target.Last());
		//}

	}

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
