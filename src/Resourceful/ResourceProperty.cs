using System;

namespace Resourceful
{
	public class ResourceProperty
	{
		public ResourceProperty(string name, Type type, object value)
		{
			Name = name;
			Type = type;
			Value = value;
		}

		public string Name { get; set; }
		public Type Type { get; set; }
		public object Value { get; set; }
	}
}