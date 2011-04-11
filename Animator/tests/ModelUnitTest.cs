using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	[TestClass]
	public class ModelUnitTest
	{


		[TestMethod]
		public void ClipReadWriteXElement()
		{
			var clipA = new Clip(null, Guid.NewGuid()) { ClipType = "FooClipType", Name = "helloclip", TriggerAlignment = 4 };
			var xmlA = clipA.WriteXElement(null);
			var clipB = new Clip(null, xmlA);
			var xmlB = clipB.WriteXElement(null);
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
		}

		[TestMethod]
		public void ParametersEqual()
		{
			var x =
				new Dictionary<string, string>
				{
					{"foo", "--foo....."},
					{"baz",String.Empty},
					{"qux",null}
				};
			var y =
				new Dictionary<string, string>
				{
					{"baz",String.Empty},
					{"qux",null},
					{"foo", "--foo....."}
				};
			Assert.IsTrue(ModelUtil.ParametersEqual(x, y));
		}

	}
}
