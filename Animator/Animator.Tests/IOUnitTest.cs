using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test", typeof(IOUnitTest.TestTransmitter))]

namespace Animator.Tests
{
	[TestClass]
	public class IOUnitTest
	{

		#region TestTransmitter

		internal class TestTransmitter : OutputTransmitter
		{

			protected override bool PostMessage(OutputMessage message)
			{
				return false;
			}

		}

		#endregion


		public IOUnitTest()
		{
		}

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestTransmitterCreateNullImplementation()
		{
			var outputModel = new Output(null, Guid.Empty) { OutputType = null };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(OutputTransmitter.NullTransmitter));
		}

		[TestMethod]
		public void TestTransmitterRegistration()
		{
			OutputTransmitter.RegisterTypes(this.GetType().Assembly);
			var outputModel = new Output(null, Guid.Empty) { OutputType = "test" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(TestTransmitter));
		}

	}
}
