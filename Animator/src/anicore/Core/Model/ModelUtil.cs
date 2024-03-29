using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region ModelUtil

	public static class ModelUtil
	{

		[CanBeNull]
		internal static Dictionary<string, string> ReadParametersXElement([CanBeNull] XElement element)
		{
			if(element == null)
				return null;
			return element.Elements(Schema.params_param)
				.ToDictionary(e => (string)e.Attribute(Schema.params_param_key), e => e.Value);
		}

		[CanBeNull]
		internal static XElement WriteParametersXElement(XName name, [CanBeNull] Dictionary<string, string> parameters)
		{
			if(parameters == null || parameters.Count == 0)
				return null;
			return new XElement(name ?? Schema.@params,
				from entry in parameters
				select new XElement(Schema.params_param,
					new XAttribute(Schema.params_param_key, entry.Key),
					entry.Value));
		}

		[CanBeNull]
		internal static T FindById<T>(this IEnumerable<T> source, Guid id)
			where T : class, IGuidId
		{
			return source == null ? null : source.FirstOrDefault(x => x != null && x.Id == id);
		}

		internal static IEnumerable<XElement> WriteXElements(IEnumerable<IXElementWritable> source, XName name = null)
		{
			if(source == null)
				return null;
			return from item in source
				   select item.WriteXElement(name);
		}

		internal static XElement WriteListXElement(IEnumerable<IXElementWritable> source, XName listName, XName itemName = null)
		{
			Require.ArgNotNull(listName, "listName");
			if(source == null || !source.Any())
				return null;
			return new XElement(listName, WriteXElements(source, itemName));
		}

		internal static XAttribute WriteOptionalAttribute(XName name, string value)
		{
			return String.IsNullOrEmpty(value) ? null : new XAttribute(name, value);
		}

		internal static XAttribute WriteOptionalValueAttribute<T>(XName name, T? value)
			where T : struct
		{
			return value == null ? null : new XAttribute(name, value);
		}

		internal static bool ParametersEqual(Dictionary<string, string> x, Dictionary<string, string> y)
		{
			if(x == null || x.Count == 0)
				return y == null || y.Count == 0;
			if(y == null || y.Count == 0)
				return false;
			if(x.Count != y.Count)
				return false;
			foreach(var xEntry in x)
			{
				string yValue;
				if(!y.TryGetValue(xEntry.Key, out yValue) || yValue != xEntry.Value)
					return false;
			}
			return true;
		}

		public static XElement WithContent(this XElement element, params object[] content)
		{
			Require.ArgNotNull(element, "element");
			element.Add(content);
			return element;
		}

		internal static bool ItemsEqual(this IEnumerable<IDocumentItem> itemsA, IEnumerable<IDocumentItem> itemsB)
		{
			itemsA = itemsA == null ? Enumerable.Empty<IDocumentItem>() : itemsA.OrderBy(x => x.Id);
			itemsB = itemsB == null ? Enumerable.Empty<IDocumentItem>() : itemsB.OrderBy(x => x.Id);
			return itemsA.OrderBy(x => x.Id).SequenceEqual(itemsB.OrderBy(x => x.Id));
		}

		internal static bool ItemsEqual<T>(IEnumerable<T> itemsA, IEnumerable<T> itemsB, IEqualityComparer<T> comparer)
			where T : IGuidId
		{
			itemsA = itemsA == null ? Enumerable.Empty<T>() : itemsA.OrderBy(x => x.Id);
			itemsB = itemsB == null ? Enumerable.Empty<T>() : itemsB.OrderBy(x => x.Id);
			return itemsA.OrderBy(x => x.Id).SequenceEqual(itemsB.OrderBy(x => x.Id), comparer);
		}

		internal static TimeSpan ParseTimeSpan(XAttribute attribute)
		{
			Require.ArgNotNull(attribute, "attribute");
			var seconds = (double)attribute;
			return TimeSpan.FromSeconds(seconds);
		}

		internal static TimeSpan? ParseNullableTimeSpan(XAttribute attribute)
		{
			return attribute == null ? (TimeSpan?)null : ParseTimeSpan(attribute);
		}

		internal static XAttribute WriteXAttribute([NotNull]XName name, TimeSpan value)
		{
			return new XAttribute(name, value.TotalSeconds);
		}
	}

	#endregion

}
