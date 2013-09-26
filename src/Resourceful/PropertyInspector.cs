using System;
using System.Reflection;

namespace Resourceful
{
	public class PropertyInspector
	{
		public static PropertyInfo[] GetProperties(Type sourceType)
		{
			return sourceType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
		}

	}
}