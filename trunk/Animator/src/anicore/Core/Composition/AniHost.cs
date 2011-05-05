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
			public IEnumerable<Lazy<Clip, IAniExportMetadata>> Clips { get; set; }

			[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
			public IEnumerable<Lazy<ITransport, IAniExportMetadata>> Transports { get; set; }

			[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
			public IEnumerable<Lazy<IOutputTransmitter, IAniExportMetadata>> OutputTransmitters { get; set; }

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
			this._Container = new CompositionContainer(this._Catalog, true);
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
			return this._Imports.Clips.CreateByElementName(elementName, () => new Clip());
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

		public Clip CreateClipByKey(string key)
		{
			return this._Imports.Clips.CreateByKey(key, () => new Clip());
		}

		public ITransport CreateTransportByKey(string key)
		{
			return this._Imports.Transports.CreateByKey(key, () => new Transport.Transport.NullTransport());
		}

		public IOutputTransmitter CreateTransmitterByKey(string key)
		{
			return this._Imports.OutputTransmitters.CreateByKey(key, () => new OutputTransmitter.NullTransmitter());
		}

		[NotNull]
		internal IOutputTransmitter CreateTransmitter([NotNull] Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
			var transmitter = this.CreateTransmitterByKey(outputModel.OutputType);
			Debug.Assert(transmitter != null);
			transmitter.Initialize(outputModel);
			return transmitter;
		}

		[NotNull]
		internal ITransport CreateTransport([CanBeNull] string transportType, [CanBeNull]IDictionary<string, string> parameters)
		{
			var transport = this.CreateTransportByKey(transportType);
			Debug.Assert(transport != null);
			transport.SetParameters(parameters);
			return transport;
		}

		public IEnumerable<KeyValuePair<string, string>> GetClipTypeDescriptionsByKey()
		{
			return this._Imports.Clips.GetTypeDescriptionsByKey();
		}

		public IEnumerable<KeyValuePair<string, string>> GetTransmitterTypeDescriptionsByKey()
		{
			return this._Imports.OutputTransmitters.GetTypeDescriptionsByKey();
		}

		public IEnumerable<KeyValuePair<string, string>> GetTransportTypeDescriptionsByKey()
		{
			return this._Imports.Transports.GetTypeDescriptionsByKey();
		}

		#endregion

	}

	#endregion

}
