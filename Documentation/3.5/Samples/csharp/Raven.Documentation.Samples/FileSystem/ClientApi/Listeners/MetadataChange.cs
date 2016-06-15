namespace Raven.Documentation.Samples.FileSystem.ClientApi.Listeners
{
	using System.Collections.Generic;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Json.Linq;

	public class MetadataChange
	{
		#region interface
		public interface IMetadataChangeListener
		{
			/// <summary>
			/// Invoked before the written data is sent to the server.
			/// </summary>
			/// <param name="instance">The file to affect</param>
			/// <param name="metadata">The new metadata</param>
			/// <param name="original">The original remote object metadata</param>
			/// <returns>
			/// Whatever the metadata was modified and requires us to re-send it.
			/// Returning false would mean that any request of write to the file would be 
			/// ignored in the current SaveChanges call.
			/// </returns>
			bool BeforeChange(FileHeader instance, RavenJObject metadata, RavenJObject original);

			/// <summary>
			/// Invoked after the metadata is sent to the server.
			/// </summary>
			/// <param name="instance">The updated file information.</param>
			/// <param name="metadata">The current metadata as stored in the server.</param>
			void AfterChange(FileHeader instance, RavenJObject metadata);
		}
		#endregion

		#region example
		public class RemoveBannedTagsListener : IMetadataChangeListener
		{
			private readonly string[] bannedTags =
			{
				/* ... */
			};

			public bool BeforeChange(FileHeader instance, RavenJObject metadata, RavenJObject original)
			{
				var tags = metadata.Value<RavenJArray>("Tags");

				if (tags == null)
					return true;

				foreach (var banned in bannedTags)
				{
					tags.Remove(banned);
				}

				metadata["Tags"] = tags; 

				return true;
			}

			public void AfterChange(FileHeader instance, RavenJObject metadata)
			{
				
			}
		}
		#endregion
	}
}