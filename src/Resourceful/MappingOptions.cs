using System.Collections.Generic;
using System.Linq;

namespace Resourceful
{
	public class MappingOptions
	{
		private readonly List<Property> additionalProperties;

		public MappingOptions()
		{
			additionalProperties = new List<Property>();
		}

		public MappingOptions WithUriReplacement(string name, object value)
		{
			additionalProperties.Add(new Property(name, typeof (string), value.ToString()));
			return this;
		}

		public List<Property> GetAditionalProperties()
		{
			return additionalProperties.ToList();
		}
	}
}