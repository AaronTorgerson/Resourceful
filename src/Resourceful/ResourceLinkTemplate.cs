using System.Collections.Generic;

namespace Resourceful
{
	public class ResourceLinkTemplate
	{
		public ResourceLinkTemplate(string relationship, string hrefTemplate)
		{
			Rel = relationship;
			HrefTemplate = new UriTemplate(hrefTemplate);
		}

		public string Rel { get; set; }
		public UriTemplate HrefTemplate { get; set; }

		public ResourceLink GenerateLink(IEnumerable<Property> resourceProperties)
		{
			return new ResourceLink
			{
				Rel = Rel,
				Href = HrefTemplate.GenerateUri(resourceProperties)
			};
		}
	}
}