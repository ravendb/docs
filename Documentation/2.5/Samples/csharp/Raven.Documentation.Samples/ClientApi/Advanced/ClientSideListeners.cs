namespace RavenCodeSamples.ClientApi.Advanced
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Abstractions.Data;
	using Raven.Client;
	using Raven.Json.Linq;

	public class ClientSideListeners : CodeSampleBase
	{
		#region document_conflict_interface
		public interface IDocumentConflictListener
		{
			bool TryResolveConflict(string key, JsonDocument[] conflictedDocs, out JsonDocument resolvedDocument);
		}

		#endregion

		#region document_conflict_example
		public class TakeNewestConflictResolutionListener : IDocumentConflictListener
		{
			public bool TryResolveConflict(
				string key,
				JsonDocument[] conflictedDocs,
				out JsonDocument resolvedDocument)
			{
				var maxDate = conflictedDocs.Max(x => x.LastModified);
				resolvedDocument = conflictedDocs
									.FirstOrDefault(x => x.LastModified == maxDate);

				if (resolvedDocument == null)
					return false;

				resolvedDocument.Metadata.Remove("@id");
				resolvedDocument.Metadata.Remove("@etag");
				return true;
			}
		}

		#endregion

		#region document_conversion_interface
		public interface IDocumentConversionListener
		{
			/// <summary>
			/// Called when converting an entity to a document and metadata
			/// </summary>
			void EntityToDocument(string key, object entity, RavenJObject document, RavenJObject metadata);

			/// <summary>
			/// Called when converting a document and metadata to an entity
			/// </summary>
			void DocumentToEntity(string key, object entity, RavenJObject document, RavenJObject metadata);

		}

		#endregion

		#region extended_document_conversion_interface
		public interface IExtendedDocumentConversionListener
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
		public class Custom
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string Value { get; set; }
		}

		public class MetadataToPropertyConversionListener : IDocumentConversionListener
		{
			public void EntityToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
			{
				if (entity is Custom == false)
					return;
				document.Remove("Value");
			}

			public void DocumentToEntity(string key, object entity, RavenJObject document, RavenJObject metadata)
			{
				if (entity is Custom == false)
					return;
				((Custom)entity).Value = metadata.Value<string>("Raven-Document-Revision");
			}
		}

		#endregion

		#region document_delete_interface
		public interface IDocumentDeleteListener
		{
			/// <summary>
			/// Invoked before the delete request is sent to the server.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="entityInstance">The entity instance.</param>
			/// <param name="metadata">The metadata.</param>
			void BeforeDelete(string key, object entityInstance, RavenJObject metadata);
		}

		#endregion

		#region document_delete_example
		public class FailDelete : IDocumentDeleteListener
		{
			public void BeforeDelete(string key, object entityInstance, RavenJObject metadata)
			{
				throw new NotSupportedException();
			}
		}

		#endregion

		#region document_query_interface
		public interface IDocumentQueryListener
		{
			/// <summary>
			/// Allow to customize a query globally
			/// </summary>
			void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization);
		}

		#endregion

		#region document_query_example
		public class NonStaleQueryListener : IDocumentQueryListener
		{
			public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
			{
				customization.WaitForNonStaleResults();
			}
		}

		#endregion

		#region document_store_interface
		public interface IDocumentStoreListener
		{
			/// <summary>
			/// Invoked before the store request is sent to the server.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="entityInstance">The entity instance.</param>
			/// <param name="metadata">The metadata.</param>
			/// <param name="original">The original document that was loaded from the server</param>
			/// <returns>
			/// Whatever the entity instance was modified and requires us re-serialize it.
			/// Returning true would force re-serialization of the entity, returning false would 
			/// mean that any changes to the entityInstance would be ignored in the current SaveChanges call.
			/// </returns>
			bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original);

			/// <summary>
			/// Invoked after the store request is sent to the server.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="entityInstance">The entity instance.</param>
			/// <param name="metadata">The metadata.</param>
			void AfterStore(string key, object entityInstance, RavenJObject metadata);
		}

		#endregion

		#region document_store_example
		public class FilterForbiddenKeysDocumentListener : IDocumentStoreListener
		{
			private readonly IList<string> forbiddenKeys = new List<string> { "system" };

			public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
			{
				return this.forbiddenKeys.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)) == false;
			}

			public void AfterStore(string key, object entityInstance, RavenJObject metadata)
			{
			}
		}

		#endregion
	}
}