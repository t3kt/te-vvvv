using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;
using Animator.Osc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	// ReSharper disable RedundantArgumentName
	[TestClass]
	public class OscUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.OSC)]
		public void CreateTransmitter()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true, osc: true);
			using(var output = host.CreateOutputByKey("osc"))
			{
				Assert.IsInstanceOfType(output, typeof(OscOutput));
			}
		}
	}
	// ReSharper restore RedundantArgumentName
}
