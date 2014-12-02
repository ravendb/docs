namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using Client.Document;

	public class Caching
	{
		public Caching()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region should_cache
			Conventions.ShouldCacheRequest = url => true;
			#endregion

			#region should_aggressive_cache_track_changes
			Conventions.ShouldAggressiveCacheTrackChanges = true;
			#endregion

			#region should_save_changes_force_aggressive_cache_check
			Conventions.ShouldSaveChangesForceAggressiveCacheCheck = true;
			#endregion
		} 
	}
}