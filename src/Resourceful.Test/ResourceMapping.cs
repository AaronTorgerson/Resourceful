using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Resourceful.Test
{
	public class ResourceMapping
	{
		private readonly List<ResourceLinkTemplate>  linkTemplates;

		public ResourceMapping(TypeMapping typeMapping, string hrefUriPattern)
		{
			TypeMapping = typeMapping;
			HrefUriPattern = hrefUriPattern;
			linkTemplates = new List<ResourceLinkTemplate>();
		}

		public string HrefUriPattern { get; set; }
		public TypeMapping TypeMapping { get; set; }

		public string GetHref(object source)
		{
			var nameProperty = TypeMapping
				.GetProperties(source)
				.First(p => p.Name == "Name");

			string name = nameProperty.Value.ToString();
			return HrefUriPattern.Replace("{Name}", name);
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

	public class ResourceLink
	{
		public string Rel { get; set; }
		public string Href { get; set; }
	}

	public class ResourceLinkTemplate
	{
		public ResourceLinkTemplate(string relationship, string hrefTemplate)
		{
			Rel = relationship;
			HrefTemplate = new UriTemplate(hrefTemplate);
		}

		public string Rel { get; set; }
		public UriTemplate HrefTemplate { get; set; }

		public ResourceLink GenerateLink(IEnumerable<ResourceProperty> resourceProperties)
		{
			return new ResourceLink
			{
				Rel = Rel,
				Href = HrefTemplate.GenerateUri(resourceProperties)
			};
		}
	}
}