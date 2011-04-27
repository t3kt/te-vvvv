using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Animator.AppCore.Common;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Animator.Tests.AniApp
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local


	/// <summary>
	///This is a test class for DoubleLinkedStackTest and is intended
	///to contain all DoubleLinkedStackTest Unit Tests
	///</summary>
	[TestClass]
	public class DoubleLinkedStackTest
	{

		private readonly ReadOnlyCollection<int> _TestValues = new ReadOnlyCollection<int>(new List<int> { 10, 20, 30, 40 });

		public TestContext TestContext { get; set; }

		private int[] GetTestValuesForPush()
		{
			int[] values = _TestValues.Reverse().ToArray();
			return values;
		}

		private DoubleLinkedStack<int> CreateWithTestValues()
		{
			DoubleLinkedStack<int> target = new DoubleLinkedStack<int>();
			this.AddTestValues(target);
			Assert.AreEqual(_TestValues.Count, target.Count);
			CollectionAssert.AreEqual(_TestValues, target);
			return target;
		}

		private void AddTestValues(DoubleLinkedStack<int> stack)
		{
			Debug.Assert(stack != null);
			IEnumerable<int> values = this.GetTestValuesForPush();
			stack.PushRange(values);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void DoubleLinkedStackConstructorWithValuesTest()
		{
			var stack = CreateWithTestValues();
			CollectionAssert.AreEqual(_TestValues, stack);
		}

		/// <summary>
		///A test for DoubleLinkedStack`1 Constructor
		///</summary>
		public DoubleLinkedStack<T> CreateEmpty<T>()
		{
			DoubleLinkedStack<T> target = new DoubleLinkedStack<T>();
			Assert.AreEqual(0, target.Count);
			Assert.AreEqual(0, target.Count());
			Assert.IsFalse(target.Any());
			return target;
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void DoubleLinkedStackConstructorTest1()
		{
			this.CreateEmpty<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Clear
		///</summary>
		public void ClearTestHelper<T>()
		{
			DoubleLinkedStack<T> target = new DoubleLinkedStack<T>();
			target.Push(default(T));
			target.Push(default(T));
			target.Push(default(T));
			target.Clear();
			Assert.AreEqual(0, target.Count);
			Assert.IsFalse(target.Any());
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void ClearTest()
		{
			ClearTestHelper<GenericParameterHelper>();
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PeekTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.Peek();
																	  }, "Peek was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			int actual = target.Peek();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PeekEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.PeekEnd();
																	  }, "PeekEnd was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.Last();
			int actual = target.PeekEnd();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PopTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.Pop();
																	  }, "Pop was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			int actual = target.Pop();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount - 1, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PopEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.PopEnd();
																	  }, "PopEnd was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.Last();
			int actual = target.PopEnd();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount - 1, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PushTest()
		{
			DoubleLinkedStack<int> target = this.CreateWithTestValues();
			var origCount = target.Count;
			int value = 999;
			target.Push(value);
			int top = target.Peek();
			Assert.AreEqual(value, top);
			Assert.AreEqual(origCount + 1, target.Count);
			CollectionAssert.AreEqual(new[] { value }.Concat(_TestValues).ToArray(), target);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PushEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateWithTestValues();
			var origCount = target.Count;
			int value = 999;
			target.PushEnd(value);
			int bottom = target.PeekEnd();
			Assert.AreEqual(value, bottom);
			Assert.AreEqual(origCount + 1, target.Count);
			CollectionAssert.AreEqual(_TestValues.Concat(new[] { value }).ToArray(), target);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PushRangeTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			var testValuesForPush = this.GetTestValuesForPush();
			target.PushRange(testValuesForPush);
			CollectionAssert.AreEqual(_TestValues, target);
			target.PushRange(testValuesForPush);
			Assert.AreEqual(_TestValues.Count * 2, target.Count);
			CollectionAssert.AreEqual(_TestValues.Concat(_TestValues).ToArray(), target);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void PushRangeEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			var testValuesForPush = this.GetTestValuesForPush();
			target.PushRangeEnd(testValuesForPush);
			CollectionAssert.AreEqual(_TestValues.Reverse().ToList(), target);
			target.PushRangeEnd(testValuesForPush);
			Assert.AreEqual(_TestValues.Count * 2, target.Count);
			CollectionAssert.AreEqual(_TestValues.Concat(_TestValues).Reverse().ToArray(), target);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void GetEnumeratorEmpty()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			IEnumerator<int> enumerator = target.GetEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on non-started enumerator");
			Assert.IsFalse(enumerator.MoveNext());
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on empty enumerator");
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void GetReversedEnumeratorEmpty()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			IEnumerator<int> enumerator = target.GetReverseEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on non-started reversed enumerator");
			Assert.IsFalse(enumerator.MoveNext());
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on empty reversed enumerator");
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		[DeploymentItem("ani.exe")]
		public void GetEnumeratorNonEmptyTest()
		{
			DoubleLinkedStack<int> target = this.CreateWithTestValues();
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
			List<int> expectedContents = _TestValues.ToList();
			CollectionAssert.AreEqual(expectedContents, actualContents);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		[DeploymentItem("ani.exe")]
		public void GetReversedEnumeratorNonEmptyTest()
		{
			DoubleLinkedStack<int> target = this.CreateWithTestValues();
			IEnumerator<int> enumerator = target.GetReverseEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on non-started reversed enumerator");
			List<int> actualContents = new List<int>();
			while(enumerator.MoveNext())
			{
				actualContents.Add(enumerator.Current);
			}
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on finished reversed enumerator");
			List<int> expectedContents = _TestValues.Reverse().ToList();
			CollectionAssert.AreEqual(expectedContents, actualContents);
			CollectionAssert.AreEqual(expectedContents, target.Reverse().ToArray());
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void TryPeekTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPeek(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPeek(out value));
			Assert.AreEqual(target.First(), value);
			Assert.AreEqual(_TestValues.First(), value);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void TryPeekEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPeekEnd(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPeekEnd(out value));
			Assert.AreEqual(target.Last(), value);
			Assert.AreEqual(_TestValues.Last(), value);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void TryPopTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPop(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPop(out value));
			Assert.AreEqual(_TestValues.First(), value);
			Assert.AreEqual(origCount - 1, target.Count);
			Assert.AreEqual(_TestValues[1], target.First());
		}

		[TestMethod]
		[TestCategory("DoubleLinkedStack")]
		public void TryPopEndTest()
		{
			DoubleLinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPopEnd(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPopEnd(out value));
			Assert.AreEqual(_TestValues.Last(), value);
			Assert.AreEqual(origCount - 1, target.Count);
			Assert.AreEqual(_TestValues[_TestValues.Count - 2], target.Last());
		}

	}

	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
