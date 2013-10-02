using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Resourceful.UriFormatting;

namespace Resourceful
{
	public class QueryParameterMapperBuilder<TResource>
	{
		private readonly List<QueryParamMapper> mappers;

		public QueryParameterMapperBuilder()
		{
			mappers = new List<QueryParamMapper>();
		}

		public QueryParameterMapperBuilder<TResource> MapRepeating<TListItem, TItemProperty>(
			string name,
			Expression<Func<TResource, IEnumerable<TListItem>>> listPropertySelector,
			Expression<Func<TListItem, TItemProperty>> itemPropertySelector)
		{
			var listProperty = ((MemberExpression)listPropertySelector.Body).Member as PropertyInfo;
			var itemProperty = ((MemberExpression)itemPropertySelector.Body).Member as PropertyInfo;
			mappers.Add(new RepeatingQueryParamMapper(name, listProperty, itemProperty));
			return this;
		}

		public List<QueryParamMapper> GetMappers()
		{
			return mappers;
		}
	}
}