using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Resourceful.Test
{
	public class UriTemplate
	{
		private readonly string uriTemplateString;
		private readonly List<string> placeholders;
		private readonly string originalTemplate;

		/// <summary>
		/// Parse the placeholders out of a URI of 
		/// the format: /path/to/{Placeholder}
		/// </summary>
		public UriTemplate(string uriTemplateString)
		{
			originalTemplate = uriTemplateString;
			placeholders = new List<string>();

			MatchCollection matches = Regex.Matches(uriTemplateString, "{(.+)}");
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				uriTemplateString = uriTemplateString.Replace(match.Value, "{" + i + "}");
				var placeholderName = matches[0].Groups[1].Value;
				placeholders.Add(placeholderName);
			}

			this.uriTemplateString = uriTemplateString;
		}

		public string GenerateUri(IEnumerable<ResourceProperty> resourceProperties)
		{
			var properties = resourceProperties.ToList();
			var insertions = new List<object>();

			foreach (var placeholder in placeholders)
			{
				var property = properties.FirstOrDefault(
					p => p.Name.Equals(placeholder, StringComparison.InvariantCultureIgnoreCase));

				if (property != null)
					insertions.Add(property.Value);
				else 
					ThrowResourceUriFormattingException(placeholder);
			}

			return insertions.Count > 0 
				? string.Format(uriTemplateString, insertions.ToArray()) 
				: uriTemplateString;
		}

		private void ThrowResourceUriFormattingException(string placeholder)
		{
			throw new ResourceUriFormattingException(
				string.Format(
					"Uri template '{0}' requires that resource " +
					"has property '{1}', but it was not found.",
					originalTemplate, placeholder)
				);
		}
	}

	public class ResourceUriFormattingException : Exception
	{
		public ResourceUriFormattingException(string message) 
			: base(message)
		{
		}
	}
}