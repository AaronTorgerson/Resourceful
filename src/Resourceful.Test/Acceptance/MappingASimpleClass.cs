﻿using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingASimpleClass
	{
		[SetUp]
		public void ClearMappings()
		{
			ResourceMapper.ClearMappings();
		}

		[Test]
		public void MappingAnUnmappedType()
		{
			var result = ResourceMapper.Map(new SimpleType {Name = "Foo"});
				
			Assert.That(result, Is.Not.Null);
			Assert.That(result["Name"], Is.EqualTo("Foo"));
		}

		[Test]
		public void MappedTypeGetsSelfLinkWithInterpolatedId()
		{
			ResourceMapper.CreateMapping<SimpleType>("/simple/{Name}");

			var result = ResourceMapper.Map(new SimpleType {Name = "Me"});

			Assert.That(result["_Relationships"]["Self"], Is.EqualTo("/simple/Me"));
		}

		[Test]
		public void CanAddOtherLinksToMapping()
		{
			ResourceMapper.CreateMapping<SimpleType>("/simple/{Id}")
										.AddRelationship("OtherThing", "/related/?name={Name}");

			var result = ResourceMapper.Map(new SimpleType {Name = "Foo", Id = 1});

			Assert.That(result["_Relationships"]["OtherThing"], Is.EqualTo("/related/?name=Foo"));
		}
	}

	class SimpleType
	{
		public string Name { get; set; }
		public int Id { get; set; }
	}
}
