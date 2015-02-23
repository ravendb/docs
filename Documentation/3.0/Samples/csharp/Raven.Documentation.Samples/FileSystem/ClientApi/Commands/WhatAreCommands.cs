namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using Client.FileSystem;

	public class WhatAreCommands
	{
		public WhatAreCommands()
		{
			IFilesStore store = null;

			#region commands_access
			IAsyncFilesCommands asyncCommands = store.AsyncFilesCommands;
			#endregion

		}
	}
}