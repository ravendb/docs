using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class GetDocumentId
	{
		#region get_document_id_3
		public class Comment
		{
			public string Author { get; set; }

			public string Message { get; set; }
		}
		#endregion

		private interface IFoo
		{
			#region get_document_id_1
			string GetDocumentId(object entity);
			#endregion
		}

		public GetDocumentId()
		{
			using (var store = new DocumentStore())
			{
				Comment comment = null;

				using (var session = store.OpenSession())
				{
					#region get_document_id_2
					var commentId = session
						.Advanced
						.GetDocumentId(comment); // e.g. comments/1
					#endregion
				}
			}
		}
	}
}