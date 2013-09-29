using System;
using Resourceful.Extensions;

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
			get { return Type.IsContainerCollection(); }
		}

		public bool IsMappableSingleValue
		{
			get { return Type.IsNonFrameworkSingleValueType(); }
		}
	}
}