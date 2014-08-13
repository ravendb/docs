namespace Raven.Documentation.CodeSamples.ClientApi.Listeners
{
	using Client.Document;
	using Client.Listeners;
	using Json.Linq;

	public class General
	{
		public class SampleDocumentStoreListener : IDocumentStoreListener
		{
			public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
			{
				throw new System.NotImplementedException();
			}

			public void AfterStore(string key, object entityInstance, RavenJObject metadata)
			{
				throw new System.NotImplementedException();
			}
		}

		public class SampleDocumentDeleteListener : IDocumentDeleteListener
		{
			public void BeforeDelete(string key, object entityInstance, RavenJObject metadata)
			{
				throw new System.NotImplementedException();
			}
		}

		public General()
		{
			using (var store = new DocumentStore())
			{
				#region register_listener
				store.RegisterListener(new SampleDocumentStoreListener());
				#endregion

				#region set_listeners
				store.SetListeners(new DocumentSessionListeners()
				{
					StoreListeners = new IDocumentStoreListener[]
					{
						new SampleDocumentStoreListener()
					},
					DeleteListeners = new IDocumentDeleteListener[]
					{
						new SampleDocumentDeleteListener()
					}
				});
				#endregion
			}
		} 
	}
}