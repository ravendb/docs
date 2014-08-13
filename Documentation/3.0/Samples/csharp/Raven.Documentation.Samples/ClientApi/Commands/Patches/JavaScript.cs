namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Patches
{
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
	using Raven.Client.Document;
	using Raven.Json.Linq;

	public class JavaScript
	{
		private interface IFoo
		{
			#region patch_1
			/// <summary>
			/// Sends a patch request for a specific document, ignoring document's Etag and if it is missing
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patch">The patch request to use (using JavaScript)</param>
			RavenJObject Patch(string key, ScriptedPatchRequest patch);

			/// <summary>
			/// Sends a patch request for a specific document, ignoring the document's Etag
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patch">The patch request to use (using JavaScript)</param>
			/// <param name="ignoreMissing">true if the patch request should ignore a missing document, false to throw DocumentDoesNotExistException</param>
			RavenJObject Patch(string key, ScriptedPatchRequest patch, bool ignoreMissing);

			/// <summary>
			/// Sends a patch request for a specific document
			/// </summary>
			/// <param name="key">Key of the document to patch</param>
			/// <param name="patch">The patch request to use (using JavaScript)</param>
			/// <param name="etag">Require specific Etag [null to ignore]</param>
			RavenJObject Patch(string key, ScriptedPatchRequest patch, Etag etag);

			/// <summary>
			/// Sends a patch request for a specific document which may or may not currently exist
			/// </summary>
			/// <param name="key">Id of the document to patch</param>
			/// <param name="patchExisting">The patch request to use (using JavaScript) to an existing document</param>
			/// <param name="patchDefault">The patch request to use (using JavaScript)  to a default document when the document is missing</param>
			/// <param name="defaultMetadata">The metadata for the default document when the document is missing</param>
			RavenJObject Patch(string key, ScriptedPatchRequest patchExisting, ScriptedPatchRequest patchDefault, RavenJObject defaultMetadata);

			#endregion
		}

		public JavaScript()
		{
			using (var store = new DocumentStore())
			{
				#region patch_2
				// change FirstName to Robert
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
							{
								Script = "this.FirstName = 'Robert';"
							});
				#endregion

				#region patch_2
				// trim FirstName
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = "this.FirstName = this.FirstName.trim();"
						});
				#endregion

				#region patch_3
				// add new property Age with value of 30
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = "this.Age = 30;"
						});
				#endregion

				#region patch_4
				// add new property Age with value of 30 using LoDash
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = "_.extend(this, { 'Age': '30'});"
						});
				#endregion

				#region patch_5
				// passing data and loading different document
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = @"
										var person = LoadDocument(differentPersonId);
										this.FirstName = person.FirstName;",
							Values = new Dictionary<string, object>
								         {
									         { "differentPersonId", "people/2" }
								         }
						});
				#endregion

				#region patch_6
				// accessing metadata (added ClrType property with value from @metadata)
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = @"this.ClrType = this['@metadata']['Raven-Clr-Type'];"
						});
				#endregion

				#region patch_7
				// creating new document with auto-assigned key e.g. 'Comments/100'
				store
					.DatabaseCommands
					.Patch(
						"people/1",
						new ScriptedPatchRequest
						{
							Script = @"PutDocument('Comments/', { 'Author': this.LastName }, { });"
						});
				#endregion

				#region patch_8
				// add a new comment to Comments
				store
					.DatabaseCommands
					.Patch(
						"blogposts/1",
						new ScriptedPatchRequest
						{
							Script = @"this.Comments.push({ 'Author': this.FirstName + ' ' + this.LastName, Message: 'Hi' });"
						});
				#endregion

				#region patch_9
				// removing comments with 'Joe Doe' as na author
				store
					.DatabaseCommands
					.Patch(
						"blogposts/1",
						new ScriptedPatchRequest
						{
							Script = @"
										this.Comments.RemoveWhere(function(comment) {
											return comment.Author == 'Joe Doe';
										});"
						});
				#endregion

				#region patch_1_0
				// modifying each comment
				store
					.DatabaseCommands
					.Patch(
						"blogposts/1",
						new ScriptedPatchRequest
						{
							Script = @"
										this.Comments.Map(function(comment) {
											comment.Author = 'Joe Doe';
											return comment;
										});"
						});
				#endregion
			}
		}
	}
}