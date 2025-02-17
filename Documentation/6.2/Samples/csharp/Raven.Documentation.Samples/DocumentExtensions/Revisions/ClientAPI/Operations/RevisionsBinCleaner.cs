using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class RevisionsBinCleaner
    {
        public RevisionsBinCleaner()
        {
            using (var store = new DocumentStore())
            {
                #region configure_cleaner
                //Define the revisions bin cleaner configuration
                var config = new RevisionsBinConfiguration()
                {
                    // Enable the cleaner
                    Disabled = false,

                    // Set the cleaner execution frequency
                    CleanerFrequencyInSec = 24 * 60 * 60, // one day (in seconds)
                    
                    // Revisions bin entries older than the following value will be deleted
                    MinimumEntriesAgeToKeepInMin = 24 * 60 // one day (in minutes)
                };
                
                // Define the operation
                var configRevisionsBinCleanerOp = new ConfigureRevisionsBinCleanerOperation(config);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(configRevisionsBinCleanerOp);
                #endregion
            }
        }

        public async Task ConfigRevisionsAsync()
        {
            using (var store = new DocumentStore())
            {
                #region configure_cleaner_async
                var config = new RevisionsBinConfiguration()
                {
                    Disabled = false,
                    CleanerFrequencyInSec = 24 * 60 * 60,
                    MinimumEntriesAgeToKeepInMin = 24 * 60 
                };

                var configRevisionsBinCleanerOp = new ConfigureRevisionsBinCleanerOperation(config);
                await store.Maintenance.SendAsync(configRevisionsBinCleanerOp);
                #endregion
            }
        }

        public class Syntax
        {
            public interface IFoo
            {
                /*
                #region syntax_1
                public ConfigureRevisionsBinCleanerOperation(RevisionsBinConfiguration configuration);
                #endregion
                */
            }

            #region syntax_2
            public class RevisionsBinConfiguration
            {
                // Set to true to enable the revisions bin cleaner.
                // Default: false (cleaner is disabled).
                public bool Disabled { get; set; }
                
                // The minimum age (in minutes) for revisions-bin entries to be kept in the database.
                // The cleaner deletes entries older than this value.
                // When set to 0: ALL revisions-bin entries will be removed from the Revisions Bin
                // Default: 30 days.
                public int MinimumEntriesAgeToKeepInMin { get; set; }
                
                // The frequency (in seconds) at which the revisions bin cleaner executes.
                // Default: 300 seconds (5 minutes).
                public long CleanerFrequencyInSec { get; set; } = 5 * 60;
            }
            #endregion
        }
    }
}
