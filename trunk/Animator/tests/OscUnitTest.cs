using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Osc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	[TestClass]
	public class OscUnitTest
	{

		public TestContext TestContext { get; set; }

		[TestMethod]
		[TestCategory("OSC")]
		public void CreateTransmitter()
		{
			OutputTransmitter.RegisterTypes(typeof(OscTransmitter).Assembly);
			var model =
				new Output(null, Guid.NewGuid())
				{
					OutputType = "osc",
					Parameters =
						new Dictionary<string, string>
						{
							{"host", "localhost"},
							{"port", "9000"}
						}
				};
			using(var transmitter = OutputTransmitter.CreateTransmitter(model))
			{
				Assert.IsInstanceOfType(transmitter, typeof(OscTransmitter));
			}
		}
	}
}
