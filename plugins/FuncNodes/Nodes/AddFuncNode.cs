using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using FuncNodes.Core;
using VVVV.PluginInterfaces.V2;

namespace FuncNodes.Nodes
{

	#region Add2FuncNode<T>

	public abstract class Add2FuncNode<T> : IPartImportsSatisfiedNotification
	{

		#region Static/Constant

		#endregion

		#region Fields

		private IFunctor _Functor;

		[Output("Function", IsSingle = true)]
		private ISpread<IFunctor> _FuncOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected abstract T Add(T arg1, T arg2);

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			//_Functor=new BasicFunctor((args)=>);
			throw new NotImplementedException();
		}

		#endregion
	}

	#endregion

}
