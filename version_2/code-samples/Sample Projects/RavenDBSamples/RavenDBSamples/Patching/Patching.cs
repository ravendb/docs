using Raven.Abstractions.Data;
using Raven.Database.Json;
using Raven.Json.Linq;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.Patching
{
	public class Patching : SampleBase
	{
		public void PatchNativeValue()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void PatchPropertyValue()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void RenameProperty()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void IncrementNumericValue()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void AddItemToArray()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void RemoveItemFromArray()
		{
			DocumentStore.DatabaseCommands.Patch(
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
		}

		public void SetValueInNested()
		{
			var doc = new RavenJObject();
			var addToPatchedDoc = new JsonPatcher(doc).Apply(
				new[]
					{
						new PatchRequest
							{
								Type = PatchCommandType.Modify,
								Name = "user",
								Nested = new[]
									{
										new PatchRequest {Type = PatchCommandType.Set, Name = "name", Value = new RavenJValue("rahien")}
									}
							}
					});
		}

		public void RemoveValueFromNested()
		{
			var doc = new RavenJObject();
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
										new PatchRequest {Type = PatchCommandType.Unset, Name = "name"}
									}
							}
					});
		}
	}
}