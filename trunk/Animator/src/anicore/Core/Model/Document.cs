using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Document

	public sealed class Document : IDocumentItem, INotifyPropertyChanged
	{

		#region Static / Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(Document), canEditDetail: false, canDelete: false);

		#endregion

		#region Fields

		private readonly AniHost _Host;

		private readonly ObservableCollection<Output> _Outputs;
		private readonly ObservableCollection<DocumentSection> _Sections;
		private Guid _Id;
		private string _Name;
		private DocumentSection _ActiveSection;
		private Transport.Transport _Transport;

		#endregion

		#region Properties

		public ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

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
			this._Outputs = new ObservableCollection<Output>();
			this._Outputs.CollectionChanged += this.Outputs_CollectionChanged;
			this._Sections = new ObservableCollection<DocumentSection>();
			this._Sections.CollectionChanged += this.Sections_CollectionChanged;
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

		private void Outputs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Outputs");
		}

		private void Sections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Sections");
		}

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

		private static void DisposeIfNeeded(object obj)
		{
			var d = obj as IDisposable;
			if(d != null)
				d.Dispose();
		}

		public bool TryDeleteItem(IDocumentItem item)
		{
			if(item == null)
				return false;
			if(item is Output && this._Outputs.Remove((Output)item))
			{
				DisposeIfNeeded(item);
				return true;
			}
			if(item is DocumentSection && this._Sections.Remove((DocumentSection)item))
			{
				DisposeIfNeeded(item);
				return true;
			}
			foreach(var output in this._Outputs)
			{
				if(output.TryDeleteItem(item))
				{
					DisposeIfNeeded(item);
					return true;
				}
			}
			foreach(var section in this._Sections)
			{
				if(section.TryDeleteItem(item))
				{
					DisposeIfNeeded(item);
					return true;
				}
			}
			return false;
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

		#region INotifyPropertyChanged Members

		private void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.PropertyChanged = null;
			if(this._Transport != null)
				this._Transport.Dispose();
			this._Transport = null;
			this._Outputs.CollectionChanged -= this.Outputs_CollectionChanged;
			foreach(var output in this._Outputs)
				output.Dispose();
			this._Outputs.Clear();
		}

		#endregion

	}

	#endregion

}
