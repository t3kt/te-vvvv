using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Animator.Core.Composition
{

	#region ImportManager<T, TMetadata>

	[Export]
	public class ImportManager<T, TMetadata>
		where TMetadata : IAniExportMetadata
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		[ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
		internal IEnumerable<Lazy<T, TMetadata>> Imports { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal virtual T CreateDefault()
		{
			return default(T);
		}

		public T CreateByKey(string key)
		{
			if(!String.IsNullOrEmpty(key) && this.Imports != null)
			{
				foreach(var import in this.Imports)
				{
					if(import.Metadata.Key == key)
						return import.Value;
				}
			}
			return this.CreateDefault();
		}

		#endregion

	}

	#endregion

}
