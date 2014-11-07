using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes
{
	public class Get
	{
		private interface IFoo
		{
			#region get_1_0
			IndexDefinition GetIndex(string name);
			#endregion

			#region get_2_0
			IndexDefinition[] GetIndexes(int start, int pageSize);
			#endregion

			#region get_3_0
			string[] GetIndexNames(int start, int pageSize);
			#endregion
		}

		public Get()
		{
			using (var store = new DocumentStore())
			{
				#region get_1_1
				var index = store.DatabaseCommands.GetIndex("Orders/Totals");
				#endregion

				#region get_2_1
				var indexes = store.DatabaseCommands.GetIndexes(0, 10);
				#endregion

				#region get_3_1
				var indexNames = store.DatabaseCommands.GetIndexNames(0, 10);
				#endregion
			}
		}
	}
}