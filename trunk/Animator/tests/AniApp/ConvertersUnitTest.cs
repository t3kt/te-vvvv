using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.UI.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable InvokeAsExtensionMethod
	// ReSharper disable ClassNeverInstantiated.Local
	// ReSharper disable ClassCanBeSealed.Local
	// ReSharper disable FieldCanBeMadeReadOnly.Local
	// ReSharper disable JoinDeclarationAndInitializer
	// ReSharper disable ConditionIsAlwaysTrueOrFalse

	[TestClass]
	public class ConvertersUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.App_Converters)]
		public void MultiplyConverterTest()
		{
			var conv = new MultiplyConverter();

			{
				double v = 3.1;
				double m = 2.3;
				double? p = null;
				conv.DefaultMultiplier = m;
				Assert.AreEqual(v * m, conv.Convert(v, null, p, null));
			}

			{
				double v = 3.1;
				double m = 0;
				double? p = 4.2;
				conv.DefaultMultiplier = m;
				Assert.AreEqual(v * p, conv.Convert(v, null, p, null));
			}

			Assert.AreEqual("foo", conv.Convert("foo", null, null, null), "Non numeric pass-through");
		}

	}

	// ReSharper restore ConditionIsAlwaysTrueOrFalse
	// ReSharper restore JoinDeclarationAndInitializer
	// ReSharper restore FieldCanBeMadeReadOnly.Local
	// ReSharper restore ClassCanBeSealed.Local
	// ReSharper restore ClassNeverInstantiated.Local
	// ReSharper restore InvokeAsExtensionMethod
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
