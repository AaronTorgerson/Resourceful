using System.Web;

namespace Resourceful.UriFormatting
{
	public class QueryParam
	{
		public QueryParam(string paramName, string value)
		{
			Key = HttpUtility.UrlEncode(paramName);
			Value = HttpUtility.UrlEncode(value);
		}

		public string Key { get; private set; }
		public string Value { get; private set; }
	}
}