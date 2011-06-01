using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Composition
{

	#region AniHost

	public sealed class AniHost
	{

		#region Static / Constant

		private static AniHost _Current;

		internal static AniHost Current
		{
			get
			{
				if(_Current == null)
					Interlocked.CompareExchange(ref _Current, new AniHost(), null);
				return _Current;
			}
		}

		#endregion

		#region Fields

		private readonly AggregateCatalog _Catalog;
		private readonly CompositionContainer _Container;

		#endregion

		#region Properties

		public AggregateCatalog Catalog
		{
			get { return this._Catalog; }
		}

		public CompositionContainer Container
		{
			get { return this._Container; }
		}

		internal IEnumerable<Lazy<Clip, IAniExportMetadata>> Clips
		{
			get { return this._Container.GetExports<Clip, IAniExportMetadata>(); }
		}

		internal IEnumerable<Lazy<Transport.Transport, IAniExportMetadata>> Transports
		{
			get { return this._Container.GetExports<Transport.Transport, IAniExportMetadata>(); }
		}

		internal IEnumerable<Lazy<Output, IAniExportMetadata>> Outputs
		{
			get { return this._Container.GetExports<Output, IAniExportMetadata>(); }
		}

		internal IEnumerable<Lazy<IClipDataEditor, IClipDataEditorMetadata>> ClipDataEditors
		{
			get { return this._Container.GetExports<IClipDataEditor, IClipDataEditorMetadata>(); }
		}

		internal IEnumerable<Lazy<IPropertyEditor, IPropertyEditorMetadata>> PropertyEditors
		{
			get { return this._Container.GetExports<IPropertyEditor, IPropertyEditorMetadata>(); }
		}

		#endregion

		#region Constructors

		public AniHost()
		{
			this._Catalog = new AggregateCatalog();
			this._Container = new CompositionContainer(this._Catalog, true);
			Interlocked.CompareExchange(ref _Current, this, null);
		}

		#endregion

		#region Methods

		public void LoadDirectory(string path, string searchPattern = "*.dll")
		{
			var catalog = new DirectoryCatalog(path, searchPattern);
			this._Catalog.Catalogs.Add(catalog);
		}

		public void LoadAssembly(Assembly assembly)
		{
			this._Catalog.Catalogs.Add(new AssemblyCatalog(assembly));
		}

		public void LoadTypes(IEnumerable<Type> types)
		{
			this._Catalog.Catalogs.Add(new TypeCatalog(types));
		}

		internal void LoadCoreAssembly()
		{
			this.LoadAssembly(typeof(AniHost).Assembly);
		}

		internal Clip CreateClipByElementName(string elementName)
		{
			return this.Clips.CreateByElementName(elementName, () => new Clip());
		}

		[NotNull]
		internal Clip ReadClipXElement([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			var clip = this.CreateClipByElementName(element.Name.ToString());
			Debug.Assert(clip != null);
			clip.ReadXElement(element);
			return clip;
		}

		[NotNull]
		public Clip CreateClipByKey(string key)
		{
			return this.Clips.CreateByKey(key, () => new Clip());
		}

		[NotNull]
		public Transport.Transport CreateTransportByKey(string key)
		{
			return this.Transports.CreateByKey(key, () => new Transport.Transport.NullTransport());
		}

		internal Output CreateOutputByElementName(string elementName)
		{
			return this.Outputs.CreateByElementName(elementName, () => new Output());
		}

		[NotNull]
		internal Output ReadOutputXElement([NotNull]XElement element)
		{
			Require.ArgNotNull(element, "element");
			var output = this.CreateOutputByElementName(element.Name.ToString());
			Debug.Assert(output != null);
			output.ReadXElement(element);
			return output;
		}

		[NotNull]
		public Output CreateOutputByKey(string key)
		{
			return this.Outputs.CreateByKey(key, () => new Output());
		}

		[CanBeNull]
		public IPropertyEditor CreatePropertyEditorByKey(string key)
		{
			return this.PropertyEditors.CreateByKey(key);
		}

		[NotNull]
		internal Transport.Transport CreateTransport([CanBeNull] string transportType, [CanBeNull]IDictionary<string, string> parameters)
		{
			var transport = this.CreateTransportByKey(transportType);
			Debug.Assert(transport != null);
			transport.SetParameters(parameters);
			return transport;
		}

		[CanBeNull]
		public IClipDataEditor CreateClipDataEditor([CanBeNull]Type clipType)
		{
			return clipType == null ? null : this.ClipDataEditors.CreateByPredicate(i => i.ClipType == clipType);
		}

		[CanBeNull]
		public IClipDataEditor CreateClipDataEditor([CanBeNull]Type clipType, out bool reusable)
		{
			if(clipType != null && this.ClipDataEditors != null)
			{
				foreach(var import in this.ClipDataEditors)
				{
					if(import.Metadata.ClipType == clipType)
					{
						reusable = import.Metadata.Reusable;
						return import.Value;
					}
				}
			}
			reusable = false;
			return null;
		}

		[CanBeNull]
		public IPropertyEditor CreatePropertyEditor([CanBeNull]Type targetType, [CanBeNull]string key = null)
		{
			if(targetType != null && this.PropertyEditors != null)
			{
				foreach(var import in this.PropertyEditors)
				{
					if(import.Metadata.TargetType == targetType &&
						(key == null || AniExportUtil.KeyComparer.Equals(import.Metadata.Key, key)))
						return import.Value;
				}
			}
			return null;
		}

		public IEnumerable<KeyValuePair<string, string>> GetClipTypeDescriptionsByKey()
		{
			return this.Clips.GetTypeDescriptionsByKey();
		}

		public IEnumerable<KeyValuePair<string, string>> GetOutputTypeDescriptionsByKey()
		{
			return this.Outputs.GetTypeDescriptionsByKey();
		}

		public IEnumerable<KeyValuePair<string, string>> GetTransportTypeDescriptionsByKey()
		{
			return this.Transports.GetTypeDescriptionsByKey();
		}

		#endregion

	}

	#endregion

}
