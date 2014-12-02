using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes.HowTo
{
	public class GetIndexMergeSuggestions
	{
		private interface IFoo
		{
			#region merge_suggestions_1
			IndexMergeResults GetIndexMergeSuggestions();
			#endregion
		}

		public GetIndexMergeSuggestions()
		{
			using (var store = new DocumentStore())
			{
				#region merge_suggestions_2
				IndexMergeResults suggestions = store.DatabaseCommands.GetIndexMergeSuggestions();
				#endregion
			}
		}
	}
}