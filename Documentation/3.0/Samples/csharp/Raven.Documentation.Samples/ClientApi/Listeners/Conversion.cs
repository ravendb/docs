namespace Raven.Documentation.CodeSamples.ClientApi.Listeners
{
	using Json.Linq;

	public class Conversion
	{
		#region document_conversion_interface
		public interface IDocumentConversionListener
		{
			/// <summary>
			/// Called before converting an entity to a document and metadata
			/// </summary>
			void BeforeConversionToDocument(string key, object entity, RavenJObject metadata);

			/// <summary>
			/// Called after having converted an entity to a document and metadata
			/// </summary>
			void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata);

			/// <summary>
			/// Called before converting a document and metadata to an entity
			/// </summary>
			void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata);

			/// <summary>
			/// Called after having converted a document and metadata to an entity
			/// </summary>
			void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity);
		}
		#endregion

		#region document_conversion_example
		public class Item
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string Revision { get; set; }
		}

		public class MetadataToPropertyConversionListener : IDocumentConversionListener
		{
			public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
			{
				if (entity is Item == false)
					return;

				document.Remove("Revision");
			}

			public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
			{
				if (entity is Item == false)
					return;

				((Item)entity).Revision = metadata.Value<string>("Raven-Document-Revision");
			}

			public void BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
			{
			}

			public void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
			{
			}
		}
		#endregion
	}
}