namespace Raven.Documentation.Web.DependencyResolution
{
	using Raven.Client;
	using Raven.Client.Document;

	using StructureMap;

	public static class IoC
	{
		public static IContainer Initialize()
		{
			var store = new DocumentStore
							{
								Url = "http://localhost:8787",
								DefaultDatabase = "Documentation"
							};

			store.Initialize();

			ObjectFactory.Initialize(x => x.For<DocumentStore>().Use(store));

			return ObjectFactory.Container;
		}
	}
}