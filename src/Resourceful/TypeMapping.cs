using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Resourceful
{
	public class TypeMapping
	{
		public Type Type { get; set; }
		public PropertyInfo[] PublicProperties { get; set; }

		public TypeMapping(Type type)
		{
			Type = type;
			PublicProperties = PropertyInspector.GetProperties(type);
		}

		public virtual IEnumerable<Property> GetProperties(object source, MappingOptions options)
		{
			return PublicProperties
				.Select(p => new Property(p.Name, p.PropertyType, p.GetValue(source)))
				.ToList();
		}
	}
}