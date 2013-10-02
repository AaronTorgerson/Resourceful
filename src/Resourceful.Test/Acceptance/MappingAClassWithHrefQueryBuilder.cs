using System.Collections.Generic;
using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class UsingHrefQueryBuilder
	{
		[SetUp]
		public void SetUp()
		{
			ResourceMapper.ClearMappings();
		}

		[Test]
		public void MultiValuedQueryBuilder()
		{
			ResourceMapper.CreateMapping<SampleTypes.Complex>("/items",
				queryParams =>
				{
					queryParams.MapRepeating("name", c => c.Items, i => i.Name);
				});

			var input = new SampleTypes.Complex
			{
				Items = new List<SampleTypes.Simple>
				{
					new SampleTypes.Simple {Name = "foo"},
					new SampleTypes.Simple {Name = "bar"}
				}
			};

			var result = ResourceMapper.Map(input);

			Assert.That(result._Relationships.Self, Is.EqualTo("/items?name=foo&name=bar"));
		}

	}
}
