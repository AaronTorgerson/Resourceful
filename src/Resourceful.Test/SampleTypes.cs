using System.Collections.Generic;
using NUnit.Framework;

namespace Resourceful.Test
{
	public static class SampleTypes
	{
		public class Complex
		{
			public int Id { get; set; }
			public List<Simple> Items { get; set; }
		}

		public class LessSimple
		{
			public int Id { get; set; }
			public Simple Child { get; set; }
		}

		public class Simple
		{
			public string Name { get; set; }
		}
	}
}