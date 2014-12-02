using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes.HowTo
{
	public class IndexHasChanged
	{
		private interface IFoo
		{
			#region index_has_changed_1
			bool IndexHasChanged(string name, IndexDefinition indexDef);
			#endregion
		}

        public IndexHasChanged()
		{
			using (var store = new DocumentStore())
			{
                IndexDefinition indexDefinition = new IndexDefinition();
                #region index_has_changed_2
                store.DatabaseCommands.IndexHasChanged("Orders/Totals", indexDefinition);
				#endregion
			}
		}
	}
}