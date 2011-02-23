using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region StructTypeEventArgs

	internal class StructTypeEventArgs : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly StructTypeDefinition _TypeDefinition;

		#endregion

		#region Properties

		public StructTypeDefinition TypeDefinition
		{
			get { return _TypeDefinition; }
		}

		#endregion

		#region Constructors

		public StructTypeEventArgs(StructTypeDefinition typeDefinition)
		{
			_TypeDefinition = typeDefinition;
		}

		#endregion

		#region Methods

		#endregion
	}

	#endregion

}
