namespace Raven.Documentation.Samples.FileSystem.ClientApi.ChangesApi
{
	using System;
	using System.Threading.Tasks;
	using Client.FileSystem;

	public class Changes
	{
		private interface IFoo
		{
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			store.Changes().ForFolder("/documents/books").Subscribe(x =>
			{
				
			});
		}
	}
}