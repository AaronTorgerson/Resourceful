using System;
using System.Collections;
using System.Dynamic;

namespace Resourceful.Extensions
{
	public static class TypeExtensions
	{
		public static bool IsContainerCollection(this Type t)
		{
			return typeof (IEnumerable).IsAssignableFrom(t)
			       && t != typeof (string)
					 && t != typeof(Relationships)
					 && t != typeof(ExpandoObject);
		}

		public static bool IsNonFrameworkSingleValueType(this Type t)
		{
			return !t.IsFrameworkType()
			       && !t.IsPrimitive
					 && t != typeof(Relationships);
		}

		public static bool IsFrameworkType(this Type t)
		{
			return (t.Namespace ?? "").StartsWith("System");
		}
	}
}
