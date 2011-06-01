using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region ModelException

	[Serializable]
	public class ModelException : Exception
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ModelException() { }

		public ModelException(string message)
			: base(message) { }

		public ModelException(string message, Exception innerException)
			: base(message, innerException) { }

		protected ModelException([NotNull] SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
