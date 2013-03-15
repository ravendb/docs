namespace RavenCodeSamples.ClientApi
{
	using Raven.Abstractions.Data;

	public class SetBasedOperations : CodeSampleBase
	{
		public void Simple()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region setbased1
				documentStore.DatabaseCommands.DeleteByIndex("IndexName",
				                                             new IndexQuery
				                                             	{
				                                             		Query = "Title:RavenDB" // where entity.Title contains RavenDB
				                                             	}, allowStale: false);

				#endregion

				#region setbased2
				documentStore.DatabaseCommands.UpdateByIndex("IndexName",
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

				#endregion
			}
		}
	}
}
