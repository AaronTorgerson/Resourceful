using System.Collections.Generic;
using System.Linq;

namespace Resourceful
{
	public class MappingOptions
	{
		private readonly List<Property> additionalUriPlaceholderProperties;

		public MappingOptions()
		{
			additionalUriPlaceholderProperties = new List<Property>();
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
	}
}