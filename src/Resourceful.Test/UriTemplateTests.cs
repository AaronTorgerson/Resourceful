﻿using System.Collections.Generic;
using NUnit.Framework;
using Resourceful.UriFormatting;

namespace Resourceful.Test
{
	[TestFixture]
	public class UriTemplateTests
	{
		private static string FormatUri(string uriTemplate, 
			IEnumerable<Property> resourceProperties, 
			IEnumerable<QueryParam> queryParams = null)
		{
			var template = new UriTemplate(uriTemplate);
			var uri = template.GenerateUri(resourceProperties, queryParams);
			return uri;
		}

		[Test]
		public void UriWithNoPlaceholdersFormatsAsIs()
		{
			var uri = FormatUri("/path/to/the/moon", new Property[0]);
			Assert.That(uri, Is.EqualTo("/path/to/the/moon"));
		}

		[Test]
		public void FormatsUriGivenListOfResourceProperties()
		{
			var uri = FormatUri("/things/{Id}/name", new[]
				{
					new Property("Id", typeof (int), 1)
				});

			Assert.That(uri, Is.EqualTo("/things/1/name"));
		}

		[Test]
		public void PropertyNotFoundThrowsException()
		{
			var exception = Assert.Throws<ResourceUriFormattingException>(() =>
			{
				FormatUri("/things/{Name}/name", new Property[0]);
			});

			Assert.That(exception.Message, Is.EqualTo("Uri template '/things/{Name}/name' requires that resource has property 'Name', but it was not found."));
		}

		[Test]
		public void QueryParameterNamesAndValuesAreEscaped()
		{
			var uri = FormatUri("/things", new Property[0], new[]
			{
				new QueryParam("this param", "needs to be escaped")
			});

			Assert.That(uri, Is.EqualTo("/things?this+param=needs+to+be+escaped"));
		}

		[Test, Ignore("TODO")]
		public void ThatAintEvenAUri()
		{
			var exception = Assert.Throws<ResourceUriFormattingException>(() =>
			{
				new UriTemplate("what do you mean this isn't a uri?");
			});
		}

		[Test, Ignore("TODO")]
		public void PlaceholderIsNotCSharpName()
		{
			var exception = Assert.Throws<ResourceUriFormattingException>(() =>
			{
				new UriTemplate("/bad/{f-ing}/placeholder");
			});
		}
	}
}