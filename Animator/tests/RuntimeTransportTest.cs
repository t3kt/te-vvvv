using Animator.Core.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Animator.Tests
{


	///// <summary>
	/////This is a test class for RuntimeTransportTest and is intended
	/////to contain all RuntimeTransportTest Unit Tests
	/////</summary>
	//[TestClass()]
	//public class RuntimeTransportTest
	//{
	//    /// <summary>
	//    ///Gets or sets the test context which provides
	//    ///information about and functionality for the current test run.
	//    ///</summary>
	//    public TestContext TestContext { get; set; }

	//    /// <summary>
	//    ///A test for RuntimeTransport Constructor
	//    ///</summary>
	//    [TestMethod()]
	//    public void RuntimeTransportConstructorTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport();
	//        Assert.Inconclusive("TODO: Implement code to verify target");
	//    }

	//    /// <summary>
	//    ///A test for Dispose
	//    ///</summary>
	//    [TestMethod()]
	//    public void DisposeTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Dispose();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for Pause
	//    ///</summary>
	//    [TestMethod()]
	//    public void PauseTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Pause();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for PauseNoLock
	//    ///</summary>
	//    [TestMethod()]
	//    [DeploymentItem("anicore.dll")]
	//    public void PauseNoLockTest()
	//    {
	//        RuntimeTransport_Accessor target = new RuntimeTransport_Accessor(); // TODO: Initialize to an appropriate value
	//        target.PauseNoLock();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for Play
	//    ///</summary>
	//    [TestMethod()]
	//    public void PlayTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Play();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for PlayNoLock
	//    ///</summary>
	//    [TestMethod()]
	//    [DeploymentItem("anicore.dll")]
	//    public void PlayNoLockTest()
	//    {
	//        RuntimeTransport_Accessor target = new RuntimeTransport_Accessor(); // TODO: Initialize to an appropriate value
	//        target.PlayNoLock();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for Reset
	//    ///</summary>
	//    [TestMethod()]
	//    public void ResetTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Reset();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for ResetNoLock
	//    ///</summary>
	//    [TestMethod()]
	//    [DeploymentItem("anicore.dll")]
	//    public void ResetNoLockTest()
	//    {
	//        RuntimeTransport_Accessor target = new RuntimeTransport_Accessor(); // TODO: Initialize to an appropriate value
	//        target.ResetNoLock();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for Stop
	//    ///</summary>
	//    [TestMethod()]
	//    public void StopTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Stop();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for StopNoLock
	//    ///</summary>
	//    [TestMethod()]
	//    [DeploymentItem("anicore.dll")]
	//    public void StopNoLockTest()
	//    {
	//        RuntimeTransport_Accessor target = new RuntimeTransport_Accessor(); // TODO: Initialize to an appropriate value
	//        target.StopNoLock();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for Update
	//    ///</summary>
	//    [TestMethod()]
	//    public void UpdateTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        target.Update();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for UpdateNoLock
	//    ///</summary>
	//    [TestMethod()]
	//    [DeploymentItem("anicore.dll")]
	//    public void UpdateNoLockTest()
	//    {
	//        RuntimeTransport_Accessor target = new RuntimeTransport_Accessor(); // TODO: Initialize to an appropriate value
	//        target.UpdateNoLock();
	//        Assert.Inconclusive("A method that does not return a value cannot be verified.");
	//    }

	//    /// <summary>
	//    ///A test for BeatsPerMinute
	//    ///</summary>
	//    [TestMethod()]
	//    public void BeatsPerMinuteTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        float expected = 0F; // TODO: Initialize to an appropriate value
	//        float actual;
	//        target.BeatsPerMinute = expected;
	//        actual = target.BeatsPerMinute;
	//        Assert.AreEqual(expected, actual);
	//        Assert.Inconclusive("Verify the correctness of this test method.");
	//    }

	//    /// <summary>
	//    ///A test for IsPlaying
	//    ///</summary>
	//    [TestMethod()]
	//    public void IsPlayingTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        bool actual;
	//        actual = target.IsPlaying;
	//        Assert.Inconclusive("Verify the correctness of this test method.");
	//    }

	//    /// <summary>
	//    ///A test for Position
	//    ///</summary>
	//    [TestMethod()]
	//    public void PositionTest()
	//    {
	//        RuntimeTransport target = new RuntimeTransport(); // TODO: Initialize to an appropriate value
	//        Time actual;
	//        actual = target.Position;
	//        Assert.Inconclusive("Verify the correctness of this test method.");
	//    }
	//}
}
