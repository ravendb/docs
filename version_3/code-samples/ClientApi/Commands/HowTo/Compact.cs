using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
{
	public class Compact
	{
		private interface IFoo
		{
			#region compact_1
			void CompactDatabase(string databaseName);
			#endregion
		}

		public Compact()
		{
			using (var store = new DocumentStore())
			{
				#region compact_2
				store
					.DatabaseCommands
					.GlobalAdmin
					.CompactDatabase("Northwind");
				#endregion
			}
		}
	}
}