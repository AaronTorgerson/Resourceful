using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Resourceful.Extensions;

namespace Resourceful
{
	public class ResourceMapper
	{
		private static readonly Dictionary<Type, ResourceMapping> resourceMappings = new Dictionary<Type, ResourceMapping>();
		private static readonly Dictionary<Type, TypeMapping> typeMappings = new Dictionary<Type, TypeMapping>();

		public static dynamic Map(object source, Action<MappingOptions> mappingOptions)
		{
			var options = new MappingOptions();
			mappingOptions.Invoke(options);

			return MapInternal(source, options);
		}

		public static dynamic Map(object source)
		{
			return MapInternal(source, new MappingOptions());
		}

		private static dynamic MapInternal(object source, MappingOptions options)
		{
			if (source == null) return null;

			Type sourceType = source.GetType();
			object result = source;

			if (sourceType.IsContainerCollection())
			{
				result = MapCollection(source);
			}
			else if (sourceType.IsNonFrameworkSingleValueType())
			{
				result = MapSingleValue(source, sourceType, options);
			}

			return result;
		}

		private static object MapSingleValue(object source, Type sourceType, MappingOptions options)
		{
			IDictionary<string, object> dest = new Dictionary<string, object>();
			TypeMapping typeMapping = GetOrCreateTypeMapping(sourceType);

			foreach (var property in typeMapping.GetProperties(source, options))
			{
				dest.Add(property.Name, Map(property.Value));
			}

			return dest;
		}

		private static object MapCollection(object source)
		{
			var items = new ArrayList();
			var enumerable = (IEnumerable) source;
			foreach (var item in enumerable)
			{
				items.Add(Map(item));
			}

			return items;
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

		public static void ClearMappings()
		{
			resourceMappings.Clear();
			typeMappings.Clear();
		}

		public static ResourceMapping CreateMapping<T>(string hrefUriPattern)
		{
			Type resourceType = typeof (T);
			var resourceMapping = new ResourceMapping(resourceType, hrefUriPattern);
			typeMappings[resourceType] = resourceMapping;
			return resourceMapping;
		}

		public static ResourceMapping CreateMapping<T>(string hrefUriPattern, Action<QueryParameterMapperBuilder<T>> mapQueryParams)
		{
			var mapping = CreateMapping<T>(hrefUriPattern);
			var queryParamMapperBuilder = new QueryParameterMapperBuilder<T>();
			mapQueryParams.Invoke(queryParamMapperBuilder);
			var queryParamMappers = queryParamMapperBuilder.GetMappers();
			queryParamMappers.ForEach(mapping.AddSelfUriQueryParameterMapper);
			return mapping;
		}
	}
}