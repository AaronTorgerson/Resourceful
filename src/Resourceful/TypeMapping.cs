using System;
using System.Collections.Generic;
using FastMember;

namespace Resourceful
{
	public class TypeMapping
	{
		private readonly TypeAccessor accessor;
		private readonly MemberSet members;
		public Type Type { get; set; }

		public TypeMapping(Type type)
		{
			Type = type;
			accessor = TypeAccessor.Create(Type);
			members = accessor.GetMembers();
		}

		public virtual IEnumerable<Property> GetProperties(object source, MappingOptions options)
		{
			foreach (var m in members)
			{
				yield return new Property(m.Name, m.Type, accessor[source, m.Name]);	
			}
		}
	}
}