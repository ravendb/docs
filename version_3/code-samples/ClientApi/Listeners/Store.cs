namespace Raven.Documentation.CodeSamples.ClientApi.Listeners
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Json.Linq;

	public class Store
	{
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
			bool BeforeStore(string key, object entityInstance, RavenJObject metadata,
								RavenJObject original);

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

			public bool BeforeStore(string key, object entityInstance, RavenJObject metadata,
										RavenJObject original)
			{
				return forbiddenKeys.Any(x => x.ToLower().Equals(key)) == false;
			}

			public void AfterStore(string key, object entityInstance, RavenJObject metadata)
			{
			}
		}

		#endregion
	}
}