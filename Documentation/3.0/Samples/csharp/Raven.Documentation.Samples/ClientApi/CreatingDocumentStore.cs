namespace Raven.Documentation.Samples.ClientApi
{
	using System;
	using Client;
	using Client.Document;

	public class CreatingDocumentStore
	{
		public CreatingDocumentStore()
		{
			#region document_store_creation
			using (IDocumentStore store = new DocumentStore()
			{
				Url = "http://localhost:8080"
			}.Initialize())
			{

			}
			#endregion
		}

		#region document_store_holder
		public class DocumentStoreHolder
		{
			private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>();

			public static IDocumentStore Store
			{
				get { return store.Value; }
			}

			private static IDocumentStore CreateStore()
			{
				IDocumentStore store = new DocumentStore()
				{
					Url = "http://localhost:8080",
					DefaultDatabase = "Northwind"
				}.Initialize();

				return store;
			}
		}
		#endregion
	}
}