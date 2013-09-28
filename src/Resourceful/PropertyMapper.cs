using System.Collections;

namespace Resourceful
{
	public static class PropertyMapper
	{
		public static object Map(Property property)
		{
			var value = property.Value;

			if (property.IsMappableEnumerable)
			{
				var items = new ArrayList();
				var enumerable = (IEnumerable) value;
				foreach (var item in enumerable)
				{
					items.Add(ResourceMapper.Map(item));
				}
				value = items;
			}
			else if (property.IsMappableSingleValue)
			{
				value = ResourceMapper.Map(value);
			}

			return value;
		}
	}
}
