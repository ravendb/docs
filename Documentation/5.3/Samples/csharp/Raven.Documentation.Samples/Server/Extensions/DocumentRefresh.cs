using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Refresh;

namespace Raven.Documentation.Samples.Server.Extensions
{
    class DocumentRefresh
    {
        public async Task Sample()
		{
            using (var documentStore = new DocumentStore())
            {
                #region refresh_0
                var refreshConfig = new RefreshConfiguration
                {
                    Disabled = false,
                    RefreshFrequencyInSec = 300
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

    /*
    #region refresh_config
    public class RefreshConfiguration
    {
        public bool Disabled { get; set; }
        public long? RefreshFrequencyInSec { get; set; }
    }
    #endregion
    */
}
