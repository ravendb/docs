using System;
using System.Configuration;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Documentation.Web.Indexes;
using StructureMap;

namespace Raven.Documentation.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var url = ConfigurationManager.AppSettings["Raven/Server/Url"];
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Please set 'Raven/Server/Url' in Web.config");

            var store = new DocumentStore
            {
                Urls = new[] { url },
                Database = "Documentation"
            };

            store.Initialize();

            try
            {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
            }
            catch (Exception e)
            {
            }

            store.ExecuteIndex(new DocumentationPages_ByKey());
            store.ExecuteIndex(new DocumentationPages_LanguagesAndVersions());

            var container = new Container(x => x.For<IDocumentStore>().Use(store));
            return container;
        }
    }
}
