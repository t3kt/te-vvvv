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
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Model.Clips;
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

		public CompositionContainer Container
		{
			get { return this._Container; }
		}

		private IEnumerable<Lazy<ClipPropertyData, IAniExportMetadata>> ClipPropertyDatas
		{
			get { return this._Container.GetExports<ClipPropertyData, IAniExportMetadata>(); }
		}

		private IEnumerable<Lazy<Transport.Transport, IAniExportMetadata>> Transports
		{
			get { return this._Container.GetExports<Transport.Transport, IAniExportMetadata>(); }
		}

		private IEnumerable<Lazy<Output, IAniExportMetadata>> Outputs
		{
			get { return this._Container.GetExports<Output, IAniExportMetadata>(); }
		}

		private IEnumerable<Lazy<IObjectEditor, IObjectEditorMetadata>> ObjectEditors
		{
			get { return this._Container.GetExports<IObjectEditor, IObjectEditorMetadata>(); }
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

		[CanBeNull]
		internal ClipPropertyData CreateClipPropertyDataByElementName(string elementName)
		{
			return this.ClipPropertyDatas.CreateByElementName(elementName);
		}

		[NotNull]
		internal ClipPropertyData ReadClipPropertyDataXElement([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			var clipData = this.CreateClipPropertyDataByElementName(element.Name.ToString());
			Debug.Assert(clipData != null);
			clipData.ReadXElement(element);
			return clipData;
		}

		[CanBeNull]
		public ClipPropertyData CreateClipPropertyDataByKey(string key)
		{
			return this.ClipPropertyDatas.CreateByKey(key);
		}

		[NotNull]
		public Transport.Transport CreateTransportByKey(string key)
		{
			return this.Transports.CreateByKey(key, () => new Transport.NullTransport());
		}

		[NotNull]
		internal Output CreateOutputByElementName(string elementName)
		{
			var output = this.Outputs.CreateByElementName(elementName, () => new Output());
			return output;
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
		public IObjectEditor CreateObjectEditorByKey(string key)
		{
			return this.ObjectEditors.CreateByKey(key);
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
		public IObjectEditor CreateObjectEditor([CanBeNull]Type targetType, [CanBeNull]string key = null)
		{
			if(targetType != null && this.ObjectEditors != null)
			{
				foreach(var import in this.ObjectEditors)
				{
					if(import.Metadata.TargetType == targetType &&
						(key == null || AniExportUtil.KeyComparer.Equals(import.Metadata.Key, key)))
						return import.Value;
				}
			}
			return null;
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
