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

		public IEnumerable<ResourceProperty> GetProperties(object source)
		{
			return PublicProperties
				.Select(p => new ResourceProperty(p.Name, p.PropertyType, p.GetValue(source)))
				.ToList();
		}
	}
}