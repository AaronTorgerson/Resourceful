using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Resourceful.UriFormatting;

namespace Resourceful
{
	public class ResourceMapping
	{
		private readonly List<ResourceLinkTemplate>  linkTemplates;
		private readonly ResourceLinkTemplate selfLinkTemplate;
		private readonly List<QueryParamMapper> queryParamMappers;

		public ResourceMapping(TypeMapping typeMapping, string hrefUriPattern)
		{
			TypeMapping = typeMapping;
			selfLinkTemplate = new ResourceLinkTemplate("Self", hrefUriPattern);
			linkTemplates = new List<ResourceLinkTemplate>();
			queryParamMappers = new List<QueryParamMapper>();
		}

		public TypeMapping TypeMapping { get; set; }

		public ResourceMapping AddLink(string relationship, string href)
		{
			linkTemplates.Add(new ResourceLinkTemplate(relationship, href));
			return this;
		}

		public dynamic GetLinks(object source, IEnumerable<Property> additionalProperties = null)
		{
			var queryParams = queryParamMappers.SelectMany(b => b.BuildQueryParams(source));
			additionalProperties = (additionalProperties ?? new List<Property>()).ToList();
			var properties = TypeMapping.GetProperties(source).Concat(additionalProperties).ToArray();

			IDictionary<string, object> links = new ExpandoObject();
			var selfLink = selfLinkTemplate.GenerateLink(properties, queryParams);
			links[selfLink.Rel] = selfLink.Href;
			
			foreach (var t in linkTemplates)
			{
				var link = t.GenerateLink(properties);
				links[link.Rel] = link.Href;
			}
			
			return links;
		}

		public void AddSelfUriQueryParameterMapper(QueryParamMapper queryParamMapper)
		{
			queryParamMappers.Add(queryParamMapper);
		}
	}
}