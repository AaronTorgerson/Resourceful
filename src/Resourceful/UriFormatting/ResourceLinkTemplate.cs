using System.Collections.Generic;

namespace Resourceful.UriFormatting
{
	public class ResourceLinkTemplate
	{
		private readonly string rel;
		private readonly UriTemplate hrefTemplate;

		public ResourceLinkTemplate(string relationship, string hrefTemplate)
		{
			rel = relationship;
			this.hrefTemplate = new UriTemplate(hrefTemplate);
		}

		public ResourceLink GenerateLink(IEnumerable<Property> resourceProperties, IEnumerable<QueryParam> queryParams = null)
		{
			return new ResourceLink
			{
				Rel = rel,
				Href = hrefTemplate.GenerateUri(resourceProperties, queryParams)
			};
		}
	}
}