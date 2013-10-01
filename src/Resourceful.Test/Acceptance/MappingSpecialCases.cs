using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[TestFixture]
	public class MappingSpecialCases
	{
		[Test]
		public void MappingAThingWithANullReference()
		{
			var thing = new SampleTypes.Simple { Name = null };

			var result = ResourceMapper.Map(thing);

			Assert.That(((string)result.Name), Is.Null);
		}
	}
}