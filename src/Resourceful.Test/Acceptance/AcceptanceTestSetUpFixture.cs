using NUnit.Framework;

namespace Resourceful.Test.Acceptance
{
	[SetUpFixture]
	public class AcceptanceTestSetUpFixture
	{
		[SetUp, TearDown]
		public void ClearMappings()
		{
			ResourceMapper.ClearMappings();
		} 
	}
}
