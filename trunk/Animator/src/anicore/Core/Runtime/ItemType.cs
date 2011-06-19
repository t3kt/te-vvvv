using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region ItemTypeInfo

	public sealed class ItemTypeInfo
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Type _Type;
		private readonly bool _CanEditDetail;
		private readonly bool _CanDelete;

		#endregion

		#region Properties

		public Type Type
		{
			get { return this._Type; }
		}

		public bool CanEditDetail
		{
			get { return this._CanEditDetail; }
		}

		public bool CanDelete
		{
			get { return this._CanDelete; }
		}

		#endregion

		#region Constructors

		public ItemTypeInfo(Type type, bool canEditDetail = true, bool canDelete = true)
		{
			Require.DBG_ArgNotNull(type, "type");
			this._Type = type;
			this._CanEditDetail = canEditDetail;
			this._CanDelete = canDelete;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
