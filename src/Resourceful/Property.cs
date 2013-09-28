using System;
using System.Collections;

namespace Resourceful
{
	public class Property
	{
		public Property(string name, Type type, object value)
		{
			Name = name;
			Type = type;
			Value = value;
		}

		public string Name { get; set; }
		public Type Type { get; set; }
		public object Value { get; set; }

		public bool IsMappableEnumerable
		{
			get
			{
				return typeof (IEnumerable).IsAssignableFrom(Type)
				       && Type != typeof (string);
			}
		}

		public bool IsMappableSingleValue
		{
			get
			{
				return !Type.Namespace.StartsWith("System")
				       && !Type.IsPrimitive;
			}
		}
	}
}