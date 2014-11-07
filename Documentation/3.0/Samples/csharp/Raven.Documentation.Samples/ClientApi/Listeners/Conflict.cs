using System.Linq;

using Raven.Abstractions.Data;

namespace Raven.Documentation.Samples.ClientApi.Listeners
{
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