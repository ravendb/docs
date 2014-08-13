namespace Raven.Documentation.Web.DependencyResolution
{
	using Raven.Client;
	using Raven.Client.Document;
	using Raven.Client.Embedded;

	using StructureMap;

	public static class IoC
	{
		public static IContainer Initialize()
		{
			var store = new EmbeddableDocumentStore
							{
								//Url = "http://localhost:8787",
								DefaultDatabase = "Documentation",
								UseEmbeddedHttpServer = true
							};

			store.Initialize();

			ObjectFactory.Initialize(x => x.For<IDocumentStore>().Use(store));

			return ObjectFactory.Container;
		}
	}
}