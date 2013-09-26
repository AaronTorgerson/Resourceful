using NUnit.Framework;

namespace Resourceful.Test
{
	[TestFixture]
	public class PropertyInspectorTests
	{
		[Test]
		public void PublicPropertiesForSimpleType()
		{
			var instance = new SampleClass{Name = "Foo"};
			var properties = PropertyInspector.GetProperties(instance.GetType());

			Assert.That(properties, Has.Length.EqualTo(1));
			Assert.That(properties[0].Name, Is.EqualTo("Name"));
			Assert.That(properties[0].GetValue(instance), Is.EqualTo("Foo"));
		}

		public class SampleClass
		{
			public string Name { get; set; }
		}
	}
}
