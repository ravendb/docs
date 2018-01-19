using System.Configuration;
using Raven.Documentation.Web.Indexes;

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

            store.ExecuteIndex(new DocumentationPages_ByKey());
            store.ExecuteIndex(new DocumentationPages_LanguagesAndVersions());
            store.ExecuteTransformer(new DocumentationPage_WithVersionsAndLanguages());

			var container = new Container(x => x.For<IDocumentStore>().Use(store));
			return container;
		}
	}
}
