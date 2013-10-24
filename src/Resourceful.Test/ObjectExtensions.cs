using System;

namespace Resourceful.Test
{
	public static class ObjectExtensions
	{
		public static dynamic AsResource(this object model)
		{
			return ResourceMapper.Map(model);
		}

		public static dynamic AsResource(this object model, Action<MappingOptions> mappingOptions)
		{
			return ResourceMapper.Map(model, mappingOptions);
		}
	}
}
