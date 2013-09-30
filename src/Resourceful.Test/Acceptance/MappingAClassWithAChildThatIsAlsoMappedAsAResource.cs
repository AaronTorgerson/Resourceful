using System.Collections.Generic;
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

		[Test]
		public void MappingWithNestedMappedResource()
		{
			ResourceMapper.CreateMapping<Complex>("/complex/{Id}");
			ResourceMapper.CreateMapping<Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(new Complex { Id = 1, Child = new Simple { Name = "Foo" } });

			Assert.That(result._Href, Is.EqualTo("/complex/1"));
			Assert.That(result._Relationships, Is.Empty);
			Assert.That(result.Child._Href, Is.EqualTo("/simple/Foo"));
		}

		[Test]
		public void MappingObjectWithEnumerableOfMappedResource()
		{
			ResourceMapper.CreateMapping<Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(
				new
				{
					Things = new []
					{
						new Simple {Name = "foo"}
					}
				});

			Assert.That(result.Things[0]._Href, Is.EqualTo("/simple/foo"));
		}

		[Test]
		public void MappingAnEnumerableOfMappedResources()
		{
			ResourceMapper.CreateMapping<Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(
				new List<Simple>
				{
					new Simple {Name = "foo"}
				});

			Assert.That(result[0]._Href, Is.EqualTo("/simple/foo"));
		}

		[Test]
		public void MappingAThingWithANullReference()
		{
			var thing = new Simple {Name = null};

			var result = ResourceMapper.Map(thing);

			Assert.That(((string)result.Name), Is.Null);
		}

		//Dictionary
		//Domain List
		//Map List<T> as a Resource
		//Let Map take arbitrary name/values for URIs
		//Add QueryBuilder for complex query string values.
		//Add Name resolver to ResourceMapping so that uri names can be resolved from child objects, etc.


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