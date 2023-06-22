using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class ConfigureRevisionsForConflicts
    {
        public ConfigureRevisionsForConflicts()
        {
            using (var documentStore = new DocumentStore())
            {
                #region conflict_revisions_configuration
                // Define the settings that will apply for conflict revisions (for all collections)
                var conflictRevConfig = new RevisionsCollectionConfiguration
                {
                    PurgeOnDelete = true,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 45, 0, 0, 0)
                    
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created for conflict documents
                    // * When the parent document is deleted all its revisions will be removed.
                    // * Revisions that exceed 45 days will be removed on next revision creation.
                };

                // Define the configure conflict revisions operation, pass the configuration
                var configureConflictRevisionsOp = 
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database, conflictRevConfig);

                // Execute the operation by passing it to Maintenance.Server.Send
                // The existing conflict revisions configuration will be replaced by the configuration passed
                documentStore.Maintenance.Server.Send(configureConflictRevisionsOp);
                #endregion
            }
        }

        public async Task ConfigureRevisionsForConflictsAsync()
        {
            using (var documentStore = new DocumentStore())
            {
                #region conflict_revisions_configuration_async
                // Define the settings that will apply for conflict revisions (for all collections)
                var conflictRevConfig = new RevisionsCollectionConfiguration
                {
                    PurgeOnDelete = true,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 45, 0, 0, 0)
                    
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created for conflict documents
                    // * When the parent document is deleted all its revisions will be removed.
                    // * Revisions that exceed 45 days will be removed on next revision creation.
                };

                // Define the configure conflict revisions operation, pass the configuration
                var configureConflictRevisionsOp = 
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database, conflictRevConfig);

                // Execute the operation by passing it to Maintenance.Server.Send
                // The existing conflict revisions configuration will be replaced by the configuration passed
                await documentStore.Maintenance.Server.SendAsync(configureConflictRevisionsOp);
                #endregion
            }
        }
        
        public class Syntax
        {
            public interface IFoo
            {
                /*
                #region syntax_1
                public ConfigureRevisionsForConflictsOperation(string database, RevisionsCollectionConfiguration configuration)
                #endregion
                */
            }
        
            #region syntax_2
            public class RevisionsCollectionConfiguration
            {
                public long? MinimumRevisionsToKeep { get; set; }
                public TimeSpan? MinimumRevisionAgeToKeep { get; set; }
                public long? MaximumRevisionsToDeleteUponDocumentUpdate { get; set; }
                public bool PurgeOnDelete { get; set; }
                public bool Disabled { get; set; }
            }
            #endregion
        }
    }
}
