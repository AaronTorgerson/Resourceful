using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingTransformations
	{
		[Test]
		public void OmitAProperty()
		{
			ResourceMapper.CreateMapping<Thing>("/things/{Id}")
										.OmitProperty("OtherId");

			dynamic result = new Thing
			{
				Id = 1, 
				OtherId = 2, 
				Name = "Foo"
			}.AsResource();

			Assert.Throws<RuntimeBinderException>(() => { var i = result.OtherId; });
			Assert.That(result["Id"], Is.EqualTo(1));
			Assert.That(result["Name"], Is.EqualTo("Foo"));
		}

		[Test]
		public void OmitPropertiesMatchingNamePattern()
		{
			ResourceMapper.CreateMapping<Thing>("/things/{Id}")
										.OmitPropertiesLike("Id");

			dynamic result = new Thing
			{
				Id = 1,
				OtherId = 2,
				Name = "Foo"
			}.AsResource();

			Assert.Throws<RuntimeBinderException>(() => { var i = result.OtherId; });
			Assert.Throws<RuntimeBinderException>(() => { var i = result.Id; });
			Assert.That(result["Name"], Is.EqualTo("Foo"));
		}

		[Test]
		public void AddAPropertyDuringMapping()
		{
			ResourceMapper.CreateMapping<Thing>("/things/{Id}");

			dynamic result = new Thing()
				.AsResource(opt =>
				{
					opt.WithAdditionalProperty("Foo", 42);
				});

			Assert.That(result["Foo"], Is.EqualTo(42));
		}

		private class Thing
		{
			public int Id { get; set; }
			public int OtherId { get; set; }
			public string Name { get; set; }
		}
	}
}
