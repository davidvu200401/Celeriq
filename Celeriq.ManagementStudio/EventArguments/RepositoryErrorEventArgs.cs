using System;

namespace Celeriq.ManagementStudio.EventArguments
{
	internal class RepositoryErrorEventArgs : RepositoryEventArgs
	{
		public Exception Exception { get; set; }
	}
}
