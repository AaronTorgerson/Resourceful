using System.Collections.Generic;
using System.Linq;

namespace Resourceful
{
	public class ResourceMapping
	{
		private readonly List<ResourceLinkTemplate>  linkTemplates;
		private readonly UriTemplate hrefUriTemplate;

		public ResourceMapping(TypeMapping typeMapping, string hrefUriPattern)
		{
			TypeMapping = typeMapping;
			HrefUriPattern = hrefUriPattern;
			hrefUriTemplate = new UriTemplate(hrefUriPattern);
			linkTemplates = new List<ResourceLinkTemplate>();
		}

		public string HrefUriPattern { get; set; }
		public TypeMapping TypeMapping { get; set; }

		public string GetHref(object source)
		{
			return hrefUriTemplate.GenerateUri(TypeMapping.GetProperties(source));
		}

		public ResourceMapping AddLink(string relationship, string href)
		{
			linkTemplates.Add(new ResourceLinkTemplate(relationship, href));
			return this;
		}

		public IEnumerable<ResourceLink> GetLinks(object source)
		{
			var resourceProperties = TypeMapping.GetProperties(source);
			return linkTemplates
				.Select(t => t.GenerateLink(resourceProperties))
				.ToList();
		}
	}
}