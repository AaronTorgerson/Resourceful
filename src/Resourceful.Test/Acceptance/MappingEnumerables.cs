using System.Collections.Generic;
using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingEnumerables
	{
		[Test]
		public void MappingObjectWithEnumerableOfMappedResource()
		{
			ResourceMapper.CreateMapping<SampleTypes.Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(
				new
				{
					Things = new[]
					{
						new SampleTypes.Simple {Name = "foo"}
					}
				});

			Assert.That(result["Things"][0]["_Relationships"]["Self"], Is.EqualTo("/simple/foo"));
		}

		[Test]
		public void MappingAnEnumerableOfMappedResources()
		{
			ResourceMapper.CreateMapping<SampleTypes.Simple>("/simple/{Name}");

			var result = ResourceMapper.Map(
				new List<SampleTypes.Simple>
				{
					new SampleTypes.Simple {Name = "foo"}
				});

			Assert.That(result[0]["_Relationships"]["Self"], Is.EqualTo("/simple/foo"));
		}
	}
}