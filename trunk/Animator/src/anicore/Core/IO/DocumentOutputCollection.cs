using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.Core.Model;

namespace Animator.Core.IO
{

	#region DocumentOutputCollection

	internal sealed class DocumentOutputCollection : DocumentNodeCollection<Output>
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public DocumentOutputCollection(Document parent)
			: base(parent) { }

		#endregion

		#region Methods

		protected override void Detach(Output item)
		{
			base.Detach(item);
			CommonUtil.DisposeIfNeeded(item);
		}

		#endregion

	}

	#endregion

}
