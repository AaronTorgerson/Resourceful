using System;
using System.Collections.Generic;
using System.Reflection;
using FastMember;

namespace Resourceful
{
	public class TypeMapping
	{
		private readonly TypeAccessor accessor;
		private readonly PropertyInfo[] publicProperties;

		public TypeMapping(Type type)
		{
			publicProperties = PropertyInspector.GetProperties(type);
			accessor = TypeAccessor.Create(type);
		}

		public virtual IEnumerable<Property> GetProperties(object source, MappingOptions options)
		{
			foreach (var p in publicProperties)
			{
				yield return new Property(p.Name, p.PropertyType, accessor[source, p.Name]);	
			}
		}
	}
}