namespace RavenCodeSamples.ClientApi.Advanced
{
	using System;

	using Raven.Json.Linq;

	public class DocumentMetadata : CodeSampleBase
	{
		public void Metadata()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region getting_metadata
					var product = session.Load<Product>(1);
					RavenJObject metadata = session.Advanced.GetMetadataFor(product);

					// Get the last modified time stamp, which is known to be of type DateTime
					DateTime collectionName = metadata.Value<DateTime>("Last-Modified");

					#endregion
				}
			}
		}
	}
}