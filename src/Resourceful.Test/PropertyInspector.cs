using System;
using System.Reflection;

namespace Resourceful.Test
{
	public class PropertyInspector
	{
		public static PropertyInfo[] GetProperties(Type sourceType)
		{
			return sourceType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
		}

	}
}