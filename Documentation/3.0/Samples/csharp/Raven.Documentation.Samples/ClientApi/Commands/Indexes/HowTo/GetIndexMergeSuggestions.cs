namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes.HowTo
{
	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;

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
				var suggestions = store.DatabaseCommands.GetIndexMergeSuggestions();
				#endregion
			}
		}
	}
}