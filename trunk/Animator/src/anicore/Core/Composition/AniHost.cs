using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Composition
{

	#region AniHost

	public sealed class AniHost
	{

		#region ImportSet

		internal sealed class ImportSet
		{

			[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
			public IEnumerable<Lazy<Clip, IClipMetadata>> Clips { get; set; }

			[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
			public IEnumerable<Lazy<ITransport, ITransportMetadata>> Transports { get; set; }

			[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
			public IEnumerable<Lazy<IOutputTransmitter, IOutputTransmitterMetadata>> OutputTransmitters { get; set; }

		}

		#endregion

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

		private static T GetByKey<T, TMetadata>(IEnumerable<Lazy<T, TMetadata>> imports, string key)
			where T : class
			where TMetadata : IKeyedExportMetadata
		{
			if(String.IsNullOrEmpty(key))
				return null;
			var import = imports.FirstOrDefault(i => i.Metadata.Key == key);
			return import == null ? null : import.Value;
		}

		#endregion

		#region Fields

		private readonly AggregateCatalog _Catalog;
		private readonly CompositionContainer _Container;
		private readonly ImportSet _Imports;

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

		internal ImportSet Imports
		{
			get { return this._Imports; }
		}

		#endregion

		#region Constructors

		public AniHost()
		{
			this._Catalog = new AggregateCatalog();
			this._Container = new CompositionContainer(this._Catalog);
			this._Imports = new ImportSet();
		}

		#endregion

		#region Methods

		public void LoadImports()
		{
			foreach(var catalog in this._Catalog.Catalogs.OfType<DirectoryCatalog>())
				catalog.Refresh();
			this._Container.ComposeParts(this._Imports);
		}

		internal Clip CreateClipByElementName(string elementName)
		{
			if(!String.IsNullOrEmpty(elementName))
			{
				if(this._Imports.Clips != null)
				{
					var entry = this._Imports.Clips.FirstOrDefault(x => x.Metadata.ElementName == elementName);
					if(entry != null)
					{
						return entry.Value;
					}
				}
			}
			return new Clip();
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

		internal Clip CreateClipByKey(string key)
		{
			return GetByKey(this._Imports.Clips, key) ?? new Clip();
		}

		internal ITransport CreateTransportByKey(string key)
		{
			return GetByKey(this._Imports.Transports, key) ?? new Transport.Transport.NullTransport();
		}

		internal IOutputTransmitter CreateOutputTransmitterByKey(string key)
		{
			return GetByKey(this._Imports.OutputTransmitters, key) ?? new OutputTransmitter.NullTransmitter();
		}

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

		#endregion

	}

	#endregion

}
