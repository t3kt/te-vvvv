using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentItem

	public abstract class DocumentItem : IDocumentItem, INotifyPropertyChanged,
		IEquatable<IDocumentItem>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid _Id;
		private string _Name;

		#endregion

		#region Properties

		//[Browsable(false)]
		public Type ItemType
		{
			get { return this.GetType(); }
		}

		public Guid Id
		{
			get { return this._Id; }
		}

		[Category(TEShared.Names.Category_Common)]
		public string Name
		{
			get { return this._Name; }
			set
			{
				if(value != this._Name)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		#endregion

		#region Constructors

		protected DocumentItem(Guid id)
		{
			this._Id = id;
		}

		protected DocumentItem([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.ReadCommonXAttributes(element);
		}

		//~DocumentItem()
		//{
		//    this.Dispose(false);
		//}

		#endregion

		#region Methods

		protected void ReadCommonXAttributes([NotNull]XElement element)
		{
			Require.ArgNotNull(element, "element");
			this._Id = (Guid)element.Attribute(Schema.common_id);
			this.Name = (string)element.Attribute(Schema.common_name);
		}

		protected XAttribute[] WriteCommonXAttributes()
		{
			return
				new[]
				{
					new XAttribute(Schema.common_id, this._Id),
					ModelUtil.WriteOptionalAttribute(Schema.common_name, this._Name)
				};
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as IDocumentItem);
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
				this.PropertyChanged = null;
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
