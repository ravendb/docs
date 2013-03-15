namespace RavenCodeSamples.Faq
{
	using Raven.Abstractions.Data;
	using Raven.Json.Linq;

	public class DenormalizedUpdates : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region denormalized_updates_1
				documentStore.DatabaseCommands.UpdateByIndex(
					"Tracks/ByArtistId",
					new IndexQuery
						{
							Query = "Artist:artists/294"
						},
					new[]
						 {
							 new PatchRequest
								 {
									 Type = PatchCommandType.Modify, 
									 Name = "Artist", 
									 Nested = new[]
										          {
											          new PatchRequest
												          {
													          Type = PatchCommandType.Set, 
															  Name = "Name", 
															  Value = RavenJToken.FromObject("Fame Soundtrack")
												          }
										          }
								 }
						 },
					allowStale: false);

				#endregion
			}
		}
	}
}