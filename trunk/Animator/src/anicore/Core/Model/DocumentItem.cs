using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;
using Animator.Common;

namespace Animator.Core.Model
{

	#region DocumentItem

	public abstract class DocumentItem : IDocumentItem, INotifyPropertyChanged,
		IEquatable<IDocumentItem>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private string _Name;

		#endregion

		#region Properties

		public Guid Id { get; protected set; }

		[Category(TEShared.Names.Category_Common)]
		public string Name
		{
			get { return _Name; }
			set
			{
				if(value != _Name)
				{
					_Name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		#endregion

		#region Constructors

		//~DocumentItem()
		//{
		//    this.Dispose(false);
		//}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return Equals(obj as IDocumentItem);
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		#endregion

		#region IEquatable<DocumentItem> Members

		public bool Equals(IDocumentItem other)
		{
			return other != null &&
				   other.Id == this.Id &&
				   other.Name == this.Name;
		}

		#endregion

		#region INotifyPropertyChanged Members

		protected virtual void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IXElementWritable Members

		public abstract XElement WriteXElement(XName name = null);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				this.PropertyChanged = null;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
