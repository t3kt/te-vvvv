using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Document

	public sealed class Document : DocumentNode, IDocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly AniHost _Host;

		private readonly DocumentOutputCollection _Outputs;
		private readonly DocumentNodeCollection<DocumentSection> _Sections;
		private Guid _Id;
		private string _Name;
		private DocumentSection _ActiveSection;
		private ITransportController _Transport;

		#endregion

		#region Properties

		internal AniHost Host
		{
			get { return this._Host; }
		}

		public Guid Id
		{
			get { return _Id; }
			private set
			{
				if(value != _Id)
				{
					_Id = value;
					OnPropertyChanged("Id");
				}
			}
		}

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

		public ObservableCollection<Output> Outputs
		{
			get { return this._Outputs; }
		}

		public IEnumerable<Sequence> Sequences
		{
			get { return this._Sections.OfType<Sequence>(); }
		}

		public IEnumerable<Session> Sessions
		{
			get { return this._Sections.OfType<Session>(); }
		}

		public ObservableCollection<DocumentSection> Sections
		{
			get { return this._Sections; }
		}

		public DocumentSection ActiveSection
		{
			get { return this._ActiveSection; }
			set
			{
				if(value != this._ActiveSection)
				{
					if(!this._Sections.Contains(value))
						throw new ModelException("Section is not from this document");
					this._ActiveSection = value;
					this.OnPropertyChanged("ActiveSection");
				}
			}
		}

		#endregion

		#region Constructors

		public Document()
			: this(null) { }

		public Document([CanBeNull] AniHost host)
		{
			this._Host = host ?? AniHost.Current;
			this._Outputs = new DocumentOutputCollection(this);
			this.ObserveChildCollection("Outputs", this._Outputs);
			this._Sections = new DocumentNodeCollection<DocumentSection>(this);
			this.ObserveChildCollection("Sections", this._Sections);
		}

		public Document(Guid id, [CanBeNull] AniHost host = null)
			: this(host)
		{
			this._Id = id;
		}

		public Document([NotNull] XElement element, [CanBeNull] AniHost host = null)
			: this(host)
		{
			this.ReadXElement(element, host);
		}

		public Document([NotNull] XDocument document, [CanBeNull]AniHost host = null)
			: this(host)
		{
			Require.ArgNotNull(document, "document");
			this.ReadXElement(document.Root, host);
		}

		#endregion

		#region Methods

		public Output GetOutput(Guid id)
		{
			return this.Outputs.FindById(id);
		}

		public Sequence GetSequence(Guid id)
		{
			return this.Sequences.FindById(id);
		}

		internal TargetObject GetTargetObject(Guid id)
		{
			foreach(var output in this._Outputs)
			{
				var target = output.GetTargetObject(id);
				if(target != null)
					return target;
			}
			return null;
		}

		private void ReadXElement(XElement element, AniHost host)
		{
			Require.ArgNotNull(element, "element");
			if(host == null)
				host = AniHost.Current;

			this.Id = (Guid)element.Attribute(Schema.anidoc_id);
			this.Name = (string)element.Attribute(Schema.anidoc_name);

			var outputsElement = element.Element(Schema.anidoc_outputs);
			if(outputsElement != null)
				this.Outputs.AddRange(outputsElement.Elements().Select(host.ReadOutputXElement));

			var sectionsElement = element.Element(Schema.anidoc_sections);
			if(sectionsElement != null)
				this._Sections.AddRange(sectionsElement.Elements().Select(e => DocumentSection.ReadSectionXElement(e, this, host)).Where(s => s != null));
		}

		#endregion

		#region IXElementWritable Members

		public XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.anidoc,
				new XAttribute(Schema.anidoc_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.anidoc_name, this.Name),
				ModelUtil.WriteListXElement(this._Outputs, Schema.anidoc_outputs),
				ModelUtil.WriteListXElement(this._Sections, Schema.anidoc_sections));
		}

		public XDocument WriteXDocument()
		{
			return new XDocument(this.WriteXElement());
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.ClearPropertyChangedListeners();
			if(this._Transport != null)
				this._Transport.Dispose();
			this._Transport = null;
			foreach(var output in this._Outputs)
				output.Dispose();
			this._Outputs.Clear();
		}

		#endregion

	}

	#endregion

}
