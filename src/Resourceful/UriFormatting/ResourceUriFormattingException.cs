using System;

namespace Resourceful.UriFormatting
{
	public class ResourceUriFormattingException : Exception
	{
		public ResourceUriFormattingException(string message) 
			: base(message)
		{
		}
	}
}