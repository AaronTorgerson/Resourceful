using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingAClassWithAChildThatIsAlsoMappedAsAResource
	{
		[Test]
		public void MappingWithoutNestedResource()
		{
			ResourceMapper.CreateMapping<Complex>("/complex/{Id}");

			var result = ResourceMapper.Map(new Complex {Id = 1, Child = new Simple {Name = "Foo"}});

			Assert.That(result._Href, Is.EqualTo("/complex/1"));
			Assert.That(result._Relationships, Is.Empty);
			Assert.That(result.Child.Name, Is.EqualTo("Foo"));
		}

		[Test, Ignore("TODO")]
		public void MappingWithNestedMappedResource()
		{
			ResourceMapper.CreateMapping<Complex>("/complex/{Id}");
			ResourceMapper.CreateMapping<Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(new Complex { Id = 1, Child = new Simple { Name = "Foo" } });

			Assert.That(result._Href, Is.EqualTo("/complex/1"));
			Assert.That(result._Relationships, Is.Empty);
			Assert.That(result.Child.Href, Is.EqualTo("/simple/Foo"));
		}

		private class Complex
		{
			public int Id { get; set; }
			public Simple Child { get; set; }
		}

		private class Simple
		{
			public string Name { get; set; }
		}
	}
}