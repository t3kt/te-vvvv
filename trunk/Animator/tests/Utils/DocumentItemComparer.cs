using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region DocumentItemComparer<T>

	internal abstract class DocumentItemComparer<T> : EqualityComparer<T>
		where T : class, IDocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override bool Equals(T x, T y)
		{
			if(x == null)
				return y == null;
			if(y == null)
				return false;
			if(x.Id != y.Id)
				return false;
			if(x.GetType() != y.GetType())
				return false;
			if(x.Name != y.Name)
				return false;
			return this.EqualsInternal(x, y);
		}

		protected abstract bool EqualsInternal(T x, T y);

		public override int GetHashCode(T obj)
		{
			if(obj == null)
				return 0;
			return obj.Id.GetHashCode();
		}

		#endregion

	}

	#endregion

}
