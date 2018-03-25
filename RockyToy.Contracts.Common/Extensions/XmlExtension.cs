using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace RockyToy.Contracts.Common.Extensions
{
	public static class XmlExtension
	{
		public static XAttribute Attr(this XElement ele, string attrName)
		{
			return ele.Attribute(attrName);
		}

		public static IEnumerable<XElement> Find(this XElement ele, string childName)
		{
			return ele.Descendants().Where(x => x.Name == childName);
		}

		public static IEnumerable<XElement> Find(this XElement ele, string childName, Predicate<XElement> filter)
		{
			return filter == null ? Find(ele, childName) : ele.Descendants().Where(x => x.Name == childName && filter(x));
		}

		public static XElement FirstOrDefault(this XElement ele, string childName)
		{
			return ele.Find(childName).FirstOrDefault();
		}

		public static XElement FirstOrDefault(this XElement ele, string childName, Predicate<XElement> filter)
		{
			return ele.Find(childName, filter).FirstOrDefault();
		}

		public static XElement First(this XElement ele, string childName)
		{
			return ele.Find(childName).First();
		}

		public static XElement First(this XElement ele, string childName, Predicate<XElement> filter)
		{
			return ele.Find(childName, filter).First();
		}

		public static string Str(this XElement ele)
		{
			return ele.Value;
		}

		public static string StrEmpty(this XElement ele)
		{
			return ele?.Value ?? string.Empty;
		}

		public static string StrDef(this XElement ele, string def)
		{
			return ele?.Value ?? def;
		}

		public static TVal Get<TVal>(this XElement ele, Func<string, TVal> converter = null)
		{
			var val = ele.Value;
			if (converter != null)
				return converter.Invoke(val);
			return (TVal) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val);
		}

		public static TVal? GetNull<TVal>(this XElement ele, Func<string, TVal?> converter = null) where TVal : struct
		{
			if (ele == null)
				return null;
			var val = ele.Value;
			if (converter != null)
				return converter.Invoke(val);
			if (TypeDescriptor.GetConverter(typeof(TVal)).CanConvertFrom(typeof(string)))
				return (TVal?) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val);
			return null;
		}

		public static TVal GetDef<TVal>(this XElement ele, TVal def, Func<string, TVal> converter = null) where TVal : struct
		{
			if (ele == null)
				return def;
			var val = ele.Value;
			if (converter != null)
				return converter.Invoke(val);
			if (TypeDescriptor.GetConverter(typeof(TVal)).CanConvertFrom(typeof(string)))
				return (TVal?) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val) ?? def;
			return def;
		}

		public static string Str(this XAttribute attr)
		{
			return attr.Value;
		}

		public static string StrEmpty(this XAttribute attr)
		{
			return attr?.Value ?? string.Empty;
		}

		public static string StrDef(this XAttribute attr, string def)
		{
			return attr?.Value ?? def;
		}

		public static TVal Get<TVal>(this XAttribute attr, Func<string, TVal> converter = null)
		{
			var val = attr.Value;
			if (converter != null)
				return converter.Invoke(val);
			return (TVal) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val);
		}

		public static TVal? GetNull<TVal>(this XAttribute attr, Func<string, TVal?> converter = null) where TVal : struct
		{
			if (attr == null)
				return null;
			var val = attr.Value;
			if (converter != null)
				return converter.Invoke(val);
			if (TypeDescriptor.GetConverter(typeof(TVal)).CanConvertFrom(typeof(string)))
				return (TVal?) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val);
			return null;
		}

		public static TVal GetDef<TVal>(this XAttribute attr, TVal def, Func<string, TVal> converter = null)
			where TVal : struct
		{
			if (attr == null)
				return def;
			var val = attr.Value;
			if (converter != null)
				return converter.Invoke(val);
			if (TypeDescriptor.GetConverter(typeof(TVal)).CanConvertFrom(typeof(string)))
				return (TVal?) TypeDescriptor.GetConverter(typeof(TVal)).ConvertFromString(val) ?? def;
			return def;
		}

		public static XElement AddEle(this XElement ele, XElement child)
		{
			ele.Add(child);
			return ele;
		}

		public static XElement AddEle(this XElement ele, IEnumerable<XElement> children)
		{
			foreach (var c in children)
			{
				ele.Add(c);
			}

			return ele;
		}

		public static XElement SetVal(this XElement ele, object val)
		{
			ele.SetValue(val);
			return ele;
		}

		public static XElement SetAttr(this XElement ele, string name, object val)
		{
			ele.SetAttributeValue(name, val);
			return ele;
		}

		public static string GetContent(this XElement ele, bool prettyFormat = false)
		{
			return !prettyFormat ? ele.ToString(SaveOptions.DisableFormatting) : ele.ToString();
		}
	}
}