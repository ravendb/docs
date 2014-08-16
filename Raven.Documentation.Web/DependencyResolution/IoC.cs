using System.Configuration;

namespace Raven.Documentation.Web.DependencyResolution
{
	using Raven.Client;
	using Raven.Client.Embedded;

	using StructureMap;

	public static class IoC
	{
		public static IContainer Initialize()
		{
			var url = ConfigurationManager.AppSettings["Raven/Server/Url"];
			var store = new EmbeddableDocumentStore
							{
								Url = string.IsNullOrEmpty(url) == false ? url : null,
								DefaultDatabase = "Documentation",
								UseEmbeddedHttpServer = string.IsNullOrEmpty(url),
								RunInMemory = true
							};

			store.Initialize();

			ObjectFactory.Initialize(x => x.For<IDocumentStore>().Use(store));

			return ObjectFactory.Container;
		}
	}
}