using Raven.Abstractions.Indexing;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes.HowTo
{
    using Raven.Client.Document;

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
                var indexDefinition = new IndexDefinition();
                #region index_has_changed_2
                store.DatabaseCommands.IndexHasChanged("Orders/Totals", indexDefinition);
				#endregion
			}
		}
	}
}