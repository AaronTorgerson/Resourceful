using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Resourceful
{
	public class ResourceMapper
	{
		private static readonly Dictionary<Type, ResourceMapping> resourceMappings = new Dictionary<Type, ResourceMapping>();
		private static readonly Dictionary<Type, TypeMapping> typeMappings = new Dictionary<Type, TypeMapping>();

		public static dynamic Map(object source)
		{
			Type sourceType = source.GetType();
			ResourceMapping resourceMapping = GetResourceMappingOrNull(sourceType);
			TypeMapping typeMapping = resourceMapping != null
 				? resourceMapping.TypeMapping
				: GetOrCreateTypeMapping(sourceType);

			IDictionary<string, object> dest = new ExpandoObject();

			foreach (var property in typeMapping.GetProperties(source))
			{
				dest.Add(property.Name, GetValue(property));
			}

			if (resourceMapping != null)
			{
				dest.Add("_Href", resourceMapping.GetHref(source));
				dest.Add("_Relationships", resourceMapping.GetLinks(source));
			}

			return dest;
		}

		private static object GetValue(Property property)
		{
			return PropertyMapper.Map(property);
		}

		private static ResourceMapping GetResourceMappingOrNull(Type sourceType)
		{
			return resourceMappings.ContainsKey(sourceType)
				? resourceMappings[sourceType]
				: null;
		}

		private static TypeMapping GetOrCreateTypeMapping(Type type)
		{
			TypeMapping mapping;
			if (!typeMappings.TryGetValue(type, out mapping))
			{
				mapping = new TypeMapping(type);
				typeMappings.Add(type, mapping);
			}

			return mapping;
		}

		public static ResourceMapping CreateMapping<T>(string hrefUriPattern)
		{
			Type resourceType = typeof (T);
			var typeMapping = new TypeMapping(resourceType);
			var resourceMapping = new ResourceMapping(typeMapping, hrefUriPattern);
			resourceMappings[resourceType] = resourceMapping;
			return resourceMapping;
		}

		public static void ClearMappings()
		{
			resourceMappings.Clear();
			typeMappings.Clear();
		}
	}
}