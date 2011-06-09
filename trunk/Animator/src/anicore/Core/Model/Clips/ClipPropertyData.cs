using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model.Clips
{

	#region ClipPropertyData

	public abstract class ClipPropertyData : INotifyPropertyChanged, IXElementWritable
	{

		#region Static / Constant

		private static readonly StringComparer NameComparer = StringComparer.OrdinalIgnoreCase;

		#endregion

		#region Fields

		private string _Name;

		#endregion

		#region Properties

		public string Name
		{
			get { return this._Name; }
			set
			{
				if(!NameComparer.Equals(value, this._Name))
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public virtual void ReadXElement([NotNull]XElement element)
		{
			Require.ArgNotNull(element, "element");
			this._Name = (string)element.Attribute(Schema.common_name);
		}

		public abstract object GetValue(float position);

		protected IEnumerable<XAttribute> WriteCommonXAttributes()
		{
			return
				new[]
				{
					new XAttribute(Schema.common_name, this._Name)
				};
		}

		#endregion

		#region IXElementWritable Members

		public abstract XElement WriteXElement(XName name = null);

		#endregion

		#region INotifyPropertyChanged Members

		protected void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

	}

	#endregion

}
