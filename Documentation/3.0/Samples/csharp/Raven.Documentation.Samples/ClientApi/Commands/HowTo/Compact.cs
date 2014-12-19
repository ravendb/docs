using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class Compact
	{
		private interface IFoo
		{
			#region compact_1
			Operation CompactDatabase(string databaseName);
			#endregion
		}

		public Compact()
		{
			using (var store = new DocumentStore())
			{
				#region compact_2
				Operation operation = store
					.DatabaseCommands
					.GlobalAdmin
					.CompactDatabase("Northwind");

				operation.WaitForCompletion();
				#endregion
			}
		}
	}
}