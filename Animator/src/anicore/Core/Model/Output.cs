using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region Output

	public class Output : DocumentItem, IOutput
	{

		#region Static / Constant

		internal static IOutput ReadOutputXElement(IDocument document, IDocumentItem parent, XElement element)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		private string _OutputType;
		private Dictionary<string, string> _Parameters;

		#endregion

		#region Properties

		public string OutputType
		{
			get { return _OutputType; }
			internal set
			{
				if(value != _OutputType)
				{
					_OutputType = value;
					OnOutputTypeChanged();
				}
			}
		}

		public Dictionary<string, string> Parameters
		{
			get { return _Parameters ?? (_Parameters = new Dictionary<string, string>()); }
			protected set { _Parameters = value; }
		}

		#endregion

		#region Constructors

		public Output(IDocumentItem parent, Guid id)
		{
			this.Parent = parent;
			this.Id = id;
		}

		public Output(IDocumentItem parent, XElement element)
		{
			this.Parent = parent;
			ReadXElement(element);
		}

		#endregion

		#region Methods

		internal void NotifyParametersChanged()
		{
			OnPropertyChanged("Parameters");
		}

		protected virtual void OnOutputTypeChanged()
		{
			OnPropertyChanged("OutputType");
		}

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			try
			{
				SuspendNotify();
				this.Id = (Guid)element.Attribute(Schema.output_id);
				this.Name = (string)element.Attribute(Schema.output_name);
				this.OutputType = (string)element.Attribute(Schema.output_type);
				_Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.output_params));
			}
			finally
			{
				ResumeNotify();
			}
		}

		#endregion

	}

	#endregion

}
