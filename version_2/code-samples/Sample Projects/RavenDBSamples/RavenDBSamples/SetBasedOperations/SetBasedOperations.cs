using Raven.Abstractions.Data;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.SetBasedOperations
{
	public class SetBasedOperations : SampleBase
	{
		public void SetBasedDelete()
		{
			DocumentStore
				.DatabaseCommands
				.DeleteByIndex("IndexName",
				               new IndexQuery
					               {
						               Query = "Title:RavenDB" // where entity.Title contains RavenDB
					               }, allowStale: false);
		}

		public void SetBasedUpdate()
		{
			DocumentStore
				.DatabaseCommands
				.UpdateByIndex("IndexName",
				               new IndexQuery {Query = "Title:RavenDB"},
				               new[]
					               {
						               new PatchRequest
							               {
								               Type = PatchCommandType.Add,
								               Name = "Comments",
								               Value = "New automatic comment we added programmatically"
							               }
					               }, allowStale: false);
		}
	}
}