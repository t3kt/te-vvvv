using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;

namespace Animator.Core.Runtime
{

	#region RuntimeDocumentItem

	internal abstract class RuntimeDocumentItem : IDisposable, INotifyPropertyChanged
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly RuntimeDocument _RuntimeDocument;

		#endregion

		#region Properties

		public RuntimeDocument RuntimeDocument
		{
			get { return this._RuntimeDocument; }
		}

		public abstract DocumentItem Item { get; }

		#endregion

		#region Constructors

		protected RuntimeDocumentItem(RuntimeDocument runtimeDocument)
		{
			Require.ArgNotNull(runtimeDocument, "runtimeDocument");
			this._RuntimeDocument = runtimeDocument;
		}

		#endregion

		#region Methods

		protected void AttachItem(DocumentItem item)
		{
			Require.ArgNotNull(item, "item");
			item.PropertyChanged += this.Item_PropertyChanged;
		}

		protected virtual void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(String.IsNullOrEmpty(e.PropertyName) ? String.Empty : "Item." + e.PropertyName);
		}

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				this.Item.PropertyChanged -= this.Item_PropertyChanged;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

	}

	#endregion

}
