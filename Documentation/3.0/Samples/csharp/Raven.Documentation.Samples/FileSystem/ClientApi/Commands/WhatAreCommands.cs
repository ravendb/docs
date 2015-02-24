namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.Net;
	using Client.FileSystem;

	public class WhatAreCommands
	{
		public WhatAreCommands()
		{
			IFilesStore store = null;

			#region commands_access
			IAsyncFilesCommands commands = store.AsyncFilesCommands;
			#endregion

			#region commands_different_fs
			IAsyncFilesCommands southwindCommands = store
				.AsyncFilesCommands
				.ForFileSystem("SouthwindFS")
				.With(new NetworkCredential("user", "pass"));
			#endregion
		}
	}
}