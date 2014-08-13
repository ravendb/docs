namespace Raven.Documentation.CodeSamples.ClientApi.Listeners
{
	using System.Linq;
	using Abstractions.Data;

	public class Conflict
	{
		#region document_conflict_listener
		public interface IDocumentConflictListener
		{
			bool TryResolveConflict(string key, JsonDocument[] conflictedDocs, 
									out JsonDocument resolvedDocument);
		}
		#endregion

		#region document_conflict_example
		public class ResolveInFavourOfNewest : IDocumentConflictListener
		{
			public bool TryResolveConflict(string key, JsonDocument[] conflictedDocs, 
											out JsonDocument resolvedDocument)
			{
				var maxDate = conflictedDocs.Max(x => x.LastModified);
				resolvedDocument = conflictedDocs
									.FirstOrDefault(x => x.LastModified == maxDate);

				return resolvedDocument != null;
			}
		}

		#endregion
	}
}