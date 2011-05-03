using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Animator.Core.Composition
{

	#region AniHost

	public sealed class AniHost
	{

		#region Static / Constant

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

		#endregion

		#region Constructors

		public AniHost()
		{
			this._Catalog = new AggregateCatalog();
			this._Container = new CompositionContainer(this._Catalog);
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

		#endregion

	}

	#endregion

}
