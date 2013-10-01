using System.Collections.Generic;
using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingWithAUriNameResolver
	{
		[Test]
		public void Thing()
		{
			ResourceMapper.CreateMapping<SampleTypes.Simple>("/things/{ExternalName}/{Name}");

			var thing = new SampleTypes.Simple {Name = "foo"};
			var result = ResourceMapper.Map(thing, new Dictionary<string, object> {{"ExternalName", "baz"}});

			Assert.That(result._Href, Is.EqualTo("/things/baz/foo"));
		}
	}
}