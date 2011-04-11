using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model
{

	#region ItemInstantiationEventArgs

	internal class ItemInstantiationEventArgs : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Document _Document;
		private readonly IDocumentItem _Parent;
		private readonly DocumentItem _Item;

		#endregion

		#region Properties

		public Document Document
		{
			get { return this._Document; }
		}

		public IDocumentItem Parent
		{
			get { return this._Parent; }
		}

		public DocumentItem Item
		{
			get { return this._Item; }
		}

		#endregion

		#region Constructors

		public ItemInstantiationEventArgs(Document document, IDocumentItem parent, DocumentItem item)
		{
			this._Document = document;
			this._Parent = parent;
			this._Item = item;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region ItemInstantiationEventArgs<T>

	internal sealed class ItemInstantiationEventArgs<T> : ItemInstantiationEventArgs
		where T : DocumentItem
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public new T Item { get { return (T)base.Item; } }

		#endregion

		#region Constructors

		public ItemInstantiationEventArgs(Document document, IDocumentItem parent, DocumentItem item)
			: base(document, parent, item) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion


}
