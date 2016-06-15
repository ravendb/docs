namespace Raven.Documentation.Samples.FileSystem.ClientApi.Listeners
{
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Client.FileSystem.Listeners;

	public class General
	{
		public class SampleListener : IFilesDeleteListener
		{
			public bool BeforeDelete(FileHeader instance)
			{
				throw new System.NotImplementedException();
			}

			public void AfterDelete(string filename)
			{
				throw new System.NotImplementedException();
			}
		}

		public void Foo()
		{
			IFilesStore store = null;

			#region set_listeners
			store.SetListeners(new FilesSessionListeners
			{
				DeleteListeners = new IFilesDeleteListener[] { /* ... */ },
				ConflictListeners = new IFilesConflictListener[] { /* ... */ },
				MetadataChangeListeners = new IMetadataChangeListener[] { /* ... */ }
			});
			#endregion

			#region register_listener
			store.Listeners.RegisterListener(new SampleListener());
			#endregion
		}
	}
}