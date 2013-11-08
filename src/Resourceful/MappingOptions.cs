using System.Collections.Generic;
using System.Linq;

namespace Resourceful
{
	public class MappingOptions
	{
		private readonly List<Property> additionalUriPlaceholderProperties;
		private readonly List<Property> additionalProperties;

		public MappingOptions()
		{
			additionalUriPlaceholderProperties = new List<Property>();
			additionalProperties = new List<Property>();
		}

		public MappingOptions WithUriReplacement(string name, object value)
		{
			additionalUriPlaceholderProperties.Add(new Property(name, typeof (string), value.ToString()));
			return this;
		}

		public List<Property> GetAdditionalUriPlaceholderProperties()
		{
			return additionalUriPlaceholderProperties.ToList();
		}

		public List<Property> GetAdditionalProperties()
		{
			return additionalProperties.ToList();
		}

		public MappingOptions WithAdditionalProperty(string propertyName, object propertyValue)
		{
			additionalProperties.Add(new Property(propertyName, propertyValue.GetType(), propertyValue));
			return this;
		}
	}
}