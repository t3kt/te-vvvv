using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncNodes.Core;
using VVVV.PluginInterfaces.V2;

namespace FuncNodes.Nodes
{

	#region BasicApply2ArgFuncNode<T>

	public abstract class BasicApply2ArgFuncNode<T> : IPluginEvaluate
	{

		#region Static/Constant

		#endregion

		#region Fields

		[Input("Argument 1")]
		private IDiffSpread<T> _Arg1;

		[Input("Argument 2")]
		private IDiffSpread<T> _Arg2;

		[Input("Function", IsSingle = true)]
		private IDiffSpread<IFunctor> _Func;

		[Output("Return")]
		private ISpread<T> _Result;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual T ProcessSingleResult(object result)
		{
			if(result is T)
				return (T)result;
			return default(T);
		}

		private T ProcessResult(object[] results)
		{
			object result;
			if(results != null && results.Length > 0)
				result = results[0];
			else
				result = null;
			return this.ProcessSingleResult(result);
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_Arg1.IsChanged || _Arg2.IsChanged || _Func.IsChanged)
			{
				var fn = _Func[0];
				if(fn == null)
				{
					_Result.SliceCount = 1;
					_Result[0] = default(T);
				}
				else
				{
					var callCount = Math.Max(_Arg1.SliceCount, _Arg2.SliceCount);
					_Result.SliceCount = callCount;
					for(var i = 0; i < callCount; i++)
					{
						var result = fn.Evaluate(new object[] { _Arg1[i], _Arg2[i] });
						if(result == null || result.Length == 0)
							_Result[i] = default(T);
						else
							_Result[i] = this.ProcessResult(result);
					}
				}
			}
		}

		#endregion

	}

	#endregion

	#region BasicValueApply2ArgFuncNode

	[PluginInfo(Name = "Apply",
		Category = "Func",
		Version = "Value 2Arg Basic")]
	public class BasicValueApply2ArgFuncNode : BasicApply2ArgFuncNode<double>
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
