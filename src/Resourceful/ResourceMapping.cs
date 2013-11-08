using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Resourceful.UriFormatting;

namespace Resourceful
{
	public class ResourceMapping : TypeMapping
	{
		private readonly List<ResourceLinkTemplate>  linkTemplates;
		private readonly ResourceLinkTemplate selfLinkTemplate;
		private readonly List<QueryParamMapper> queryParamMappers;
		private readonly List<Regex> propertyNamePatternsToOmit; 

		public ResourceMapping(Type resourceType, string hrefUriPattern)
			: base(resourceType)
		{
			selfLinkTemplate = new ResourceLinkTemplate("Self", hrefUriPattern);
			linkTemplates = new List<ResourceLinkTemplate>();
			queryParamMappers = new List<QueryParamMapper>();
			propertyNamePatternsToOmit = new List<Regex>();
		}

		public override IEnumerable<Property> GetProperties(object source, MappingOptions options)
		{
			var properties = base
				.GetProperties(source, options)
				.Concat(options.GetAdditionalProperties())
				.ToList();

			properties.Add(GetLinksProperty(source, options, properties));
			return properties.Where(p => !IsOmitted(p));
		}

		private Property GetLinksProperty(object source, MappingOptions options, IEnumerable<Property> properties)
		{
			var linkProperties = properties
				.Concat(options.GetAdditionalUriPlaceholderProperties())
				.ToList();
			var links = GetLinks(source, linkProperties);
			return new Property("_Relationships", typeof (object), links);
		}

		private bool IsOmitted(Property property)
		{
			return propertyNamePatternsToOmit.Any(regex => regex.IsMatch(property.Name));
		}

		public ResourceMapping AddLink(string relationship, string href)
		{
			linkTemplates.Add(new ResourceLinkTemplate(relationship, href));
			return this;
		}

		private dynamic GetLinks(object source, List<Property> properties)
		{
			var queryParams = queryParamMappers.SelectMany(b => b.BuildQueryParams(source));
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

		public ResourceMapping OmitProperty(string propertyName)
		{
			propertyNamePatternsToOmit.Add(new Regex("^" + propertyName + "$", RegexOptions.Compiled));
			return this;
		}

		public ResourceMapping OmitPropertiesLike(string propertyNameRegex)
		{
			propertyNamePatternsToOmit.Add(new Regex(propertyNameRegex, RegexOptions.Compiled));
			return this;
		}
	}
}