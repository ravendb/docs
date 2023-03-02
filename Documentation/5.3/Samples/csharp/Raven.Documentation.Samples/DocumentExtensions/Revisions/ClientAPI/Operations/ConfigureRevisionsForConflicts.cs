using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class ConfigureRevisionsForConflicts
    {
        public async Task Foo()
        {
            using (var documentStore = new DocumentStore())
            {
                using (var session = documentStore.OpenSession())
                {
                    #region conflict-revisions-configuration_sync
                    // Modify the document conflicts configuration
                    documentStore.Maintenance.Server.Send(
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database,
                        new RevisionsCollectionConfiguration
                        {
                            // Purge conflict revisions upon their parent document deletion
                            PurgeOnDelete = true,

                            // Limit the number of conflict revisions by age, to 45 days
                            MinimumRevisionAgeToKeep = new TimeSpan(days: 45, 0, 0, 0)
                        }));
                    #endregion
                }

                using (var asyncSession = documentStore.OpenAsyncSession())
                {
                    #region conflict-revisions-configuration_async
                    // Modify the document conflicts configuration
                    await documentStore.Maintenance.Server.SendAsync(
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database,
                        new RevisionsCollectionConfiguration
                        {
                            // Purge conflict revisions upon their parent document deletion
                            PurgeOnDelete = true,

                            // Limit the number of conflict revisions by age, to 45 days
                            MinimumRevisionAgeToKeep = new TimeSpan(days: 45, 0, 0, 0)
                        }));
                    #endregion
                }
            }
        }
    }
}
