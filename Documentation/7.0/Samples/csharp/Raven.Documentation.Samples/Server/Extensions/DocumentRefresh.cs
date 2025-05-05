using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Refresh;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.Server.Extensions
{
    class DocumentRefresh
    {
        public async Task Sample()
        {
            using (var documentStore = new DocumentStore())
            {
                #region refresh_0

                var refreshConfig = new RefreshConfiguration {
                    Disabled = false, 
                    RefreshFrequencyInSec = 300,
                    MaxItemsToProcess = 1000 
                };

                var result = documentStore.Maintenance.Send(new ConfigureRefreshOperation(refreshConfig));

                #endregion

                #region refresh_1

                using (var session = documentStore.OpenSession())
                {
                    var document = session.Load<object>("users/1-A");

                    session.Advanced.GetMetadataFor(document)["@refresh"] = DateTime.UtcNow.AddHours(1);

                    session.SaveChanges();
                }

                #endregion
            }
        }
    }

    class fresh
    {
        #region refresh_config
        public class RefreshConfiguration
        {
            // Set 'Disabled' to false to enable the refresh feature
            public bool Disabled { get; set; }

            // How frequently to process documents with a @refresh flag
            public long? RefreshFrequencyInSec { get; set; }

            // How many items to refresh (each time the refresh task is invoked)
            public long? MaxItemsToProcess { get; set; }
        }
        #endregion
    }
}
