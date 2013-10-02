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
			IDictionary<string, object> dest = new ExpandoObject();
			ResourceMapping resourceMapping = GetResourceMappingOrNull(sourceType);
			TypeMapping typeMapping = resourceMapping != null
				? resourceMapping.TypeMapping
				: GetOrCreateTypeMapping(sourceType);

			foreach (var property in typeMapping.GetProperties(source))
			{
				dest.Add(property.Name, Map(property.Value));
			}

			if (resourceMapping != null)
			{
				dest.Add("_Relationships", resourceMapping.GetLinks(source, options.GetAditionalProperties()));
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