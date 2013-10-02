using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingAClassWithAChildThatIsAlsoMappedAsAResource
	{
		[Test]
		public void MappingWithoutNestedResource()
		{
			ResourceMapper.CreateMapping<SampleTypes.LessSimple>("/complex/{Id}");

			var result = ResourceMapper.Map(new SampleTypes.LessSimple {Id = 1, Child = new SampleTypes.Simple {Name = "Foo"}});

			Assert.That(result._Relationships.Self, Is.EqualTo("/complex/1"));
			Assert.That(result.Child.Name, Is.EqualTo("Foo"));
		}

		[Test]
		public void MappingWithNestedMappedResource()
		{
			ResourceMapper.CreateMapping<SampleTypes.LessSimple>("/complex/{Id}");
			ResourceMapper.CreateMapping<SampleTypes.Simple>("/simple/{Name}");

			var source = new SampleTypes.LessSimple
			{
				Id = 1, 
				Child = new SampleTypes.Simple {Name = "Foo"}
			};

			var result = ResourceMapper.Map(source);

			Assert.That(result._Relationships.Self, Is.EqualTo("/complex/1"));
			Assert.That(result.Child._Relationships.Self, Is.EqualTo("/simple/Foo"));
		}

		//Dictionary
		//Domain List
		//Map List<T> as a Resource
		//Let Map take arbitrary name/values for URIs
		//Add QueryBuilder for complex query string values.
		//Add Name resolver to ResourceMapping so that uri names can be resolved from child objects, etc.


		
	}
}