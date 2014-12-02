using System;
using Raven.Json.Linq;

namespace RavenCodeSamples.Consumer
{
	public class Metadata : CodeSampleBase
	{
		public void BasicMetadataUsage()
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
