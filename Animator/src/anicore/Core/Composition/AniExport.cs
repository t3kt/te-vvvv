using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using TESharedAnnotations;

namespace Animator.Core.Composition
{

	#region AniExportAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class AniExportAttribute : ExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string Key { get; set; }

		public string Description { get; set; }

		public string ElementName { get; set; }

		#endregion

		#region Constructors

		public AniExportAttribute(Type contractType)
			: base(contractType) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region IAniExportMetadata

	public interface IAniExportMetadata
	{
		string Key { get; }
		string Description { get; }
		string ElementName { get; }
	}

	#endregion

	#region AniExportUtil

	internal static class AniExportUtil
	{

		internal static readonly StringComparer KeyComparer = StringComparer.OrdinalIgnoreCase;
		internal static readonly StringComparer ElementNameComparer = StringComparer.Ordinal;

		[CanBeNull]
		internal static T CreateByKey<T, TMetadata>([CanBeNull] this IEnumerable<Lazy<T, TMetadata>> imports, [CanBeNull] string key, [CanBeNull] Func<T> defaultFactory = null)
			where T : class
			where TMetadata : IAniExportMetadata
		{
			if(imports != null && !String.IsNullOrEmpty(key))
			{
				foreach(var import in imports)
				{
					if(KeyComparer.Equals(import.Metadata.Key, key))
						return import.Value;
				}
			}
			if(defaultFactory != null)
				return defaultFactory();
			return null;
		}

		[CanBeNull]
		internal static T CreateByElementName<T, TMetadata>([CanBeNull] this IEnumerable<Lazy<T, TMetadata>> imports, [CanBeNull] string elementName, [CanBeNull] Func<T> defaultFactory = null)
			where T : class
			where TMetadata : IAniExportMetadata
		{
			if(imports != null && !String.IsNullOrEmpty(elementName))
			{
				foreach(var import in imports)
				{
					if(ElementNameComparer.Equals(import.Metadata.ElementName, elementName))
						return import.Value;
				}
			}
			if(defaultFactory != null)
				return defaultFactory();
			return null;
		}

		[NotNull]
		internal static IEnumerable<KeyValuePair<string, string>> GetTypeDescriptionsByKey<T, TMetadata>([CanBeNull]this IEnumerable<Lazy<T, TMetadata>> imports)
			where T : class
			where TMetadata : IAniExportMetadata
		{
			if(imports == null)
				return Enumerable.Empty<KeyValuePair<string, string>>();
			return from import in imports
				   select new KeyValuePair<string, string>(import.Metadata.Key, import.Metadata.Description ?? import.Metadata.Key);
		}

	}

	#endregion

}
