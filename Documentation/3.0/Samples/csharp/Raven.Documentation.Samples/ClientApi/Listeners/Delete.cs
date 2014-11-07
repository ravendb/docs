using System;

using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Listeners
{
	public class Delete
	{
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
		public class PreventDeleteListener : IDocumentDeleteListener
		{
			public void BeforeDelete(string key, object entityInstance, RavenJObject metadata)
			{
				throw new NotSupportedException();
			}
		}

		#endregion
	}
}