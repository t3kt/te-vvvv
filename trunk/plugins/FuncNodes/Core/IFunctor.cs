using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncNodes.Core
{

	#region IFunctor

	public interface IFunctor
	{

		int ArgCount { get; }

		int ResultCount { get; }

		object[] Evaluate(object[] args);

	}

	#endregion

}
