namespace RavenCodeSamples.ClientApi
{
	using Raven.Abstractions.Data;
	using Raven.Database.Json;
	using Raven.Json.Linq;

	public class PartialDocumentUpdates : CodeSampleBase
	{
		public void SimplePatching()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region patching1
				var comment = new BlogComment
								{
									Title = "Foo",
									Content = "Bar"
								};

				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Add,
									Name = "Comments",
									Value = RavenJObject.FromObject(comment)
								}
						});

				#endregion

				#region patching2
				// Setting a native type value
				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Set,
									Name = "Title",
									Value = RavenJObject.FromObject("New title")
								}
						});

				// Setting an object as a property value
				documentStore.DatabaseCommands.Patch(
					"blogposts/4321",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Set,
									Name = "Author",
									Value = RavenJObject.FromObject(
										new BlogAuthor
											{
												Name = "Itamar",
												ImageUrl = "/author_images/itamar.jpg"
											})
								}
						});

				#endregion

				#region patching3
				// This is how you rename a property; copying works
				// exactly the same, but with Type = PatchCommandType.Copy
				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Rename,
									Name = "Comments",
									Value = new RavenJValue("cmts")
								}
						});

				#endregion

				#region patching4
				// Assuming we have a Views counter in our entity
				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Inc,
									Name = "Views",
									Value = new RavenJValue(1)
								}
						});

				#endregion

				#region patching_arrays1
				// Append a new comment; Insert operation is supported
				// as well, by using PatchCommandType.Add and
				// specifying a Position to insert at
				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Add,
									Name = "Comments",
									Value =
										RavenJObject.FromObject(new BlogComment
										                        	{Content = "FooBar"})
								}
						});

				// Remove the first comment
				documentStore.DatabaseCommands.Patch(
					"blogposts/1234",
					new[]
						{
							new PatchRequest
								{
									Type = PatchCommandType.Remove,
									Name = "Comments",
									Position = 0
								}
						});

				#endregion

				var doc = new RavenJObject();

				#region nested1
				var addToPatchedDoc = new JsonPatcher(doc).Apply(
					new[]
				{
					new PatchRequest
					{
						Type = PatchCommandType.Modify,
						Name = "user",
						Nested = new[]
						{
							new PatchRequest {Type = PatchCommandType.Set, Name = "name", Value = new RavenJValue("rahien")},
						}
					},
				});

				#endregion

				#region nested2
				var removeFromPatchedDoc = new JsonPatcher(doc).Apply(
				new[]
				{
					new PatchRequest
					{
						Type = PatchCommandType.Modify,
						Name = "user",
						PrevVal = RavenJObject.Parse(@"{ ""name"": ""ayende"", ""id"": 13}"),
						Nested = new[]
						{
							new PatchRequest {Type = PatchCommandType.Unset, Name = "name" },
						}
					},
				});

				#endregion
			}
		}
	}
}
