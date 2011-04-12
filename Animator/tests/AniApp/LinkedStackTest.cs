using System.Collections.ObjectModel;
using System.Linq;
using Animator.AppCore.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Animator.Tests.AniApp
{


	/// <summary>
	///This is a test class for LinkedStackTest and is intended
	///to contain all LinkedStackTest Unit Tests
	///</summary>
	[TestClass]
	public class LinkedStackTest
	{

		private readonly ReadOnlyCollection<int> _TestValues = new ReadOnlyCollection<int>(new List<int> { 10, 20, 30, 40 });

		public TestContext TestContext { get; set; }

		private LinkedStack<int> CreateWithTestValues()
		{
			IEnumerable<int> values = _TestValues.ToArray();
			LinkedStack<int> target = new LinkedStack<int>(values);
			return target;
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void LinkedStackConstructorWithValuesTest()
		{
			var stack = CreateWithTestValues();
			CollectionAssert.AreEqual(_TestValues, stack);
		}

		/// <summary>
		///A test for LinkedStack`1 Constructor
		///</summary>
		public void LinkedStackConstructorTest1Helper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>();
			Assert.Inconclusive("TODO: Implement code to verify target");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void LinkedStackConstructorTest1()
		{
			LinkedStackConstructorTest1Helper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Clear
		///</summary>
		public void ClearTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			target.Clear();
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void ClearTest()
		{
			ClearTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for GetEnumerator
		///</summary>
		public void GetEnumeratorTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			IEnumerator<T> expected = null; // TODO: Initialize to an appropriate value
			IEnumerator<T> actual;
			actual = target.GetEnumerator();
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void GetEnumeratorTest()
		{
			GetEnumeratorTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Peek
		///</summary>
		public void PeekTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			T expected = default(T); // TODO: Initialize to an appropriate value
			T actual;
			actual = target.Peek();
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PeekTest()
		{
			PeekTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Pop
		///</summary>
		public void PopTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			T expected = default(T); // TODO: Initialize to an appropriate value
			T actual;
			actual = target.Pop();
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PopTest()
		{
			PopTestHelper<GenericParameterHelper>();
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PushTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			T value = default(T); // TODO: Initialize to an appropriate value
			target.Push(value);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PushTest()
		{
			PushTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for PushRange
		///</summary>
		public void PushRangeTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			IEnumerable<T> values = null; // TODO: Initialize to an appropriate value
			target.PushRange(values);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PushRangeTest()
		{
			PushRangeTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for System.Collections.IEnumerable.GetEnumerator
		///</summary>
		public void GetEnumeratorTest1Helper<T>()
		{
			IEnumerable target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			IEnumerator expected = null; // TODO: Initialize to an appropriate value
			IEnumerator actual;
			actual = target.GetEnumerator();
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		[DeploymentItem("ani.exe")]
		public void GetEnumeratorTest1()
		{
			GetEnumeratorTest1Helper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for TryPeek
		///</summary>
		public void TryPeekTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			T value = default(T); // TODO: Initialize to an appropriate value
			T valueExpected = default(T); // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = target.TryPeek(out value);
			Assert.AreEqual(valueExpected, value);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void TryPeekTest()
		{
			TryPeekTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for TryPop
		///</summary>
		public void TryPopTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			T value = default(T); // TODO: Initialize to an appropriate value
			T valueExpected = default(T); // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = target.TryPop(out value);
			Assert.AreEqual(valueExpected, value);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void TryPopTest()
		{
			TryPopTestHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for IsEmpty
		///</summary>
		public void IsEmptyTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>(); // TODO: Initialize to an appropriate value
			bool actual;
			actual = target.IsEmpty;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void IsEmptyTest()
		{
			IsEmptyTestHelper<GenericParameterHelper>();
		}
	}
}
