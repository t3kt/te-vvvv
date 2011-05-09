using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
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
		[TestCategory("OSC")]
		public void CreateTransmitter()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true, osc: true);
			var model =
				new Output(Guid.NewGuid())
				{
					OutputType = "osc",
					Parameters =
						new Dictionary<string, string>
						{
							{"host", "localhost"},
							{"port", "9000"}
						}
				};
			using(var transmitter = host.CreateTransmitter(model))
			{
				Assert.IsInstanceOfType(transmitter, typeof(OscTransmitter));
			}
		}
	}
	// ReSharper restore RedundantArgumentName
}
