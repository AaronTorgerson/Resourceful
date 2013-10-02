using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingWithAUriNameResolver
	{
		[Test]
		public void UriReplacementUponMap()
		{
			ResourceMapper.CreateMapping<SampleTypes.Simple>("/things/{ExternalName}/{Name}");

			var thing = new SampleTypes.Simple {Name = "foo"};
			var result = ResourceMapper.Map(thing, opt => opt.WithUriReplacement("ExternalName", "baz"));

			Assert.That(result._Relationships.Self, Is.EqualTo("/things/baz/foo"));
		}
	}
}