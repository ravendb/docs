using Raven.Client.Connection;
using Raven.Client.Connection.Async;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands
{
	public class WhatAreCommands
    {
		public WhatAreCommands()
        {
			using (var store = new DocumentStore())
			{
				#region what_are_commands_1
				IDatabaseCommands commands = store.DatabaseCommands;
				IAsyncDatabaseCommands asyncCommands = store.AsyncDatabaseCommands;
				#endregion
			}
        }
    }
}