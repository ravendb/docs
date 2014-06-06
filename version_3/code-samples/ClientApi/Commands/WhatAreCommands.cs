namespace Raven.Documentation.CodeSamples.ClientApi.Commands
{
	using Raven.Client.Document;

	public class WhatAreCommands
    {
		public WhatAreCommands()
        {
			using (var store = new DocumentStore())
			{
				#region what_are_commands_1
				var commands = store.DatabaseCommands;
				var asyncCommands = store.AsyncDatabaseCommands;
				#endregion
			}
        }
    }
}