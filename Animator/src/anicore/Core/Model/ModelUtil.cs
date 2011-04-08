using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region ModelUtil

	internal static class ModelUtil
	{

		public static IDocument GetDocument(IDocumentItem item)
		{
			while(item != null)
			{
				if(item is IDocument)
					return (IDocument)item;
				item = item.Parent;
			}
			return null;
		}

		public static IEnumerable<IDocumentItem> DescendentsAndSelf(this IDocumentItem item)
		{
			Require.ArgNotNull(item, "item");
			return new[] { item }.Concat(item.Descendents());
		}

		public static IEnumerable<IDocumentItem> Descendents(this IDocumentItem item)
		{
			Require.ArgNotNull(item, "item");
			return item.Children.SelectMany(DescendentsAndSelf);
		}

		public static IDocumentItem FindItem(this IDocumentItem root, Guid id)
		{
			if(root == null)
				return null;
			if(id == root.Id)
				return root;
			foreach(var child in root.Children)
			{
				var item = child.FindItem(id);
				if(item != null)
					return item;
			}
			return null;
		}

		public static T FindItem<T>(this IDocumentItem root, Guid id)
			where T : class, IDocumentItem
		{
			var item = root.FindItem(id);
			if(item == null)
				return null;
			//if(!(item is T))
			//    throw new InvalidOperationException(String.Format("Incorrect document item type for id '{0}': {1}", id, item.GetType()));
			Debug.Assert(item is T);
			return (T)item;
		}

		[CanBeNull]
		internal static Dictionary<string, string> ReadParametersXElement([CanBeNull] XElement element)
		{
			if(element == null)
				return null;
			return element.Elements(Schema.params_param)
				.ToDictionary(e => (string)e.Attribute(Schema.params_param_key), e => e.Value);
		}

		internal static T FindById<T>(this IEnumerable<T> source, Guid id)
			where T : class, IGuidId
		{
			return source == null ? null : source.FirstOrDefault(x => x != null && x.Id == id);
		}

		internal static IEnumerable<XElement> WriteXElements<T>(this IEnumerable<T> source, XName name)
			where T : IXElementWritable
		{
			if(source == null)
				return null;
			return from item in source
				   select item.WriteXElement(name);
		}

		internal static XAttribute WriteOptionalAttribute(XName name, string value)
		{
			return String.IsNullOrEmpty(value) ? null : new XAttribute(name, value);
		}

	}

	#endregion

}
