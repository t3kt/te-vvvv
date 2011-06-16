using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Animator.Core.Runtime
{

	#region IGuidId

	public interface IGuidId
	{

		[Browsable(false)]
		Guid Id { get; }

	}

	#endregion

	#region GuidIdComparer

	internal sealed class GuidIdComparer : EqualityComparer<IGuidId>
	{

		#region Static/Constant

		private static readonly GuidIdComparer _Default = new GuidIdComparer();

		internal static new GuidIdComparer Default
		{
			get { return _Default; }
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		private GuidIdComparer() { }

		#endregion

		#region Methods

		public override bool Equals(IGuidId x, IGuidId y)
		{
			if(x == y)
				return true;
			if(x == null || y == null)
				return false;
			return x.Id == y.Id;
		}

		public override int GetHashCode(IGuidId obj)
		{
			return obj == null ? 0 : obj.Id.GetHashCode();
		}

		#endregion

	}

	#endregion

}
