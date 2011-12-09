using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncNodes.Core
{

	#region BasicFunctor

	internal sealed class BasicFunctor : IFunctor
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly Func<object[], object[]> _Evaluate;
		private readonly int _ArgCount;
		private readonly int _ResultCount;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public BasicFunctor(Func<object[], object[]> evaluate, int argCount, int resultCount = 1)
		{
			this._Evaluate = evaluate;
			this._ArgCount = argCount;
			this._ResultCount = resultCount;
		}

		#endregion

		#region Methods

		#endregion

		#region IFunctor Members

		public int ArgCount
		{
			get { return _ArgCount; }
		}

		public int ResultCount
		{
			get { return _ResultCount; }
		}

		public object[] Evaluate(object[] args)
		{
			return _Evaluate(args);
		}

		#endregion
	}

	#endregion

}
