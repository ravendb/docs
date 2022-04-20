using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.Revisions;

namespace Raven.Documentation.Samples.ClientApi.Operations.RevisionsDefinitions
{
    public class ConfigureRevisionsOperation
    {
        #region ConfigureRevisionsOperation_definition
        public ConfigureRevisionsOperation(RevisionsConfiguration configuration)
        #endregion
        {
        }

        #region RevisionsConfiguration_definition
        public class RevisionsConfiguration
        {
            public RevisionsCollectionConfiguration Default;
            public Dictionary<string, RevisionsCollectionConfiguration> Collections;
        }
        #endregion

        #region RevisionsCollectionConfiguration_definition
        public class RevisionsCollectionConfiguration
        {
            public long? MinimumRevisionsToKeep { get; set; }
            public TimeSpan? MinimumRevisionAgeToKeep { get; set; }
            public bool Disabled { get; set; }
            public bool PurgeOnDelete { get; set; }
            public long? MaximumRevisionsToDeleteUponDocumentUpdate { get; set; }
        }
        #endregion

    }
}
