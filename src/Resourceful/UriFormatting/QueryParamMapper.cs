using System.Collections.Generic;

namespace Resourceful.UriFormatting
{
	public abstract class QueryParamMapper
	{
		public abstract List<QueryParam> BuildQueryParams(object source);
	}
}