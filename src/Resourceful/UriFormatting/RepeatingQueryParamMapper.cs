using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Resourceful.UriFormatting
{
	public class RepeatingQueryParamMapper : QueryParamMapper
	{
		private readonly string paramName;
		private readonly PropertyInfo listProperty;
		private readonly PropertyInfo listItemProperty;

		public RepeatingQueryParamMapper(string paramName,
			PropertyInfo listProperty,
			PropertyInfo listItemProperty)
		{
			this.paramName = paramName;
			this.listProperty = listProperty;
			this.listItemProperty = listItemProperty;
		}

		public override List<QueryParam> BuildQueryParams(object source)
		{
			var items = (IEnumerable)listProperty.GetValue(source);
			var @params = new List<QueryParam>();
			foreach (var item in items)
			{
				var itemPropertyValue = listItemProperty.GetValue(item);
				@params.Add(new QueryParam(paramName, itemPropertyValue.ToString()));
			}

			return @params;
		}
	}
}