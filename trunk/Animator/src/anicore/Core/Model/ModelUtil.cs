using System;
using System.Collections;
using System.Collections.Generic;
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

		internal static XElement WithContent(this XElement element, params object[] content)
		{
			Require.ArgNotNull(element, "element");
			element.Add(content);
			return element;
		}

		internal static string OrNullIfEmpty(this string str)
		{
			return String.IsNullOrWhiteSpace(str) ? null : str;
		}

	}

	#endregion

}
