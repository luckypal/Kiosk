using System;

namespace wyUpdate.Common
{
	public class PatchApplicationException : Exception
	{
		public PatchApplicationException(string message)
			: base(message)
		{
		}
	}
}
