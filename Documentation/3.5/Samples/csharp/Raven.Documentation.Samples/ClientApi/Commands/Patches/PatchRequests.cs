using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Patches
{
	public class PatchRequests
    {
		private interface IFoo
		{
			#region patch_1
			/// <summary>
			/// Sends a patch request for a specific document, ignoring document's Etag and if it is missing
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patches">Array of patch requests</param>
			RavenJObject Patch(string key, PatchRequest[] patches);

			/// <summary>
			/// Sends a patch request for a specific document, ignoring the document's Etag
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patches">Array of patch requests</param>
			/// <param name="ignoreMissing">true if the patch request should ignore a missing document, false to throw DocumentDoesNotExistException</param>
			RavenJObject Patch(string key, PatchRequest[] patches, bool ignoreMissing);

			/// <summary>
			/// Sends a patch request for a specific document
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patches">Array of patch requests</param>
			/// <param name="etag">Require specific Etag [null to ignore]</param>
			RavenJObject Patch(string key, PatchRequest[] patches, Etag etag);

			/// <summary>
			/// Sends a patch request for a specific document which may or may not currently exist
			/// </summary>
			/// <param name="key">Id of the document to patch</param>
			/// <param name="patchesToExisting">Array of patch requests to apply to an existing document</param>
			/// <param name="patchesToDefault">Array of patch requests to apply to a default document when the document is missing</param>
			/// <param name="defaultMetadata">The metadata for the default document when the document is missing</param>
			RavenJObject Patch(string key, PatchRequest[] patchesToExisting, PatchRequest[] patchesToDefault, RavenJObject defaultMetadata);

			#endregion
		}

		public PatchRequests()
        {
			using (var store = new DocumentStore())
			{
				#region patch_2
				// change FirstName to Robert
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Set, 
									Name = "FirstName", 
									Value = "Robert"
								}
						});
				#endregion

				#region patch_1_0
				// change FirstName to Robert and LastName to Carter in single request
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Set, 
									Name = "FirstName", 
									Value = "Robert"
								},
							new PatchRequest
								{
									Type = PatchCommandType.Set, 
									Name = "LastName", 
									Value = "Carter"
								}
						});
				#endregion

				#region patch_3
				// add new property Age with value of 30
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Set, 
									Name = "Age", 
									Value = 30
								}
						});
				#endregion

				#region patch_4
				// increment Age property value by 10
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Inc, 
									Name = "Age", 
									Value = 10
								}
						});
				#endregion

				#region patch_5
				// remove property Age
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Unset, 
									Name = "Age"
								}
						});
				#endregion

				#region patch_6
				// rename FirstName to First
				store.DatabaseCommands.Patch(
					"employees/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Rename, 
									Name = "FirstName",
									Value = "First"
								}
						});
				#endregion

				#region patch_7
				// add a new comment to Comments
				store.DatabaseCommands.Patch(
					"blogposts/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Add, 
									Name = "Comments",
									Value = RavenJObject.FromObject(new BlogComment
										                                {
																			Content = "Lore ipsum",
																			Title = "Some title"
										                                })
								}
						});
				#endregion

				#region patch_8
				// insert a new comment at position 0 to Comments
				store.DatabaseCommands.Patch(
					"blogposts/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Insert, 
									Position = 0,
									Name = "Comments",
									Value = RavenJObject.FromObject(new BlogComment
										                                {
											                                Content = "Lore ipsum",
																			Title = "Some title"
										                                })
								}
						});
				#endregion

				#region patch_9
				// modify a comment at position 3 in Comments
				store.DatabaseCommands.Patch(
					"blogposts/1",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Modify, 
									Position = 3,
									Name = "Comments",
									Nested = new []
										         {
											         new PatchRequest
												         {
													         Type = PatchCommandType.Set,
															 Name = "Title",
															 Value = "New title"
												         }
										         }
								}
						});
				#endregion
			}
        }
    }
}
