using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Resourceful.Extensions;

namespace Resourceful
{
	public class ResourceMapper
	{
		private static readonly ConcurrentDictionary<Type, TypeMapping> typeMappings = new ConcurrentDictionary<Type, TypeMapping>();

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
			return typeMappings.GetOrAdd(type, t => new TypeMapping(type));
		}

		public static void ClearMappings()
		{
			typeMappings.Clear();
		}

		public static ResourceMapping CreateMapping<T>(string hrefUriPattern)
		{
			Type resourceType = typeof (T);
			var resourceMapping = new ResourceMapping(resourceType, hrefUriPattern);
			typeMappings.AddOrUpdate(resourceType, _ => resourceMapping, (_,__) => resourceMapping);
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