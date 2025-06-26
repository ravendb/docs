using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.DataArchival;

namespace Raven.Documentation.Samples.DataArchival
{
    public class EnableArchiving
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                #region enable_archiving
                // Define the archival configuration object
                var configuration = new DataArchivalConfiguration
                {
                    // Enable archiving
                    Disabled = false,

                    // Optional: override the default archiving frequency
                    // Scan for documents scheduled for archiving every 180 seconds 
                    ArchiveFrequencyInSec = 180,
                    
                    // Optional: limit the number of documents processed in each archival run
                    MaxItemsToProcess = 100 
                };
                
                // Define the archival operation, pass the configuration 
                var configureArchivalOp = new ConfigureDataArchivalOperation(configuration);
    
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(configureArchivalOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region enable_archiving_async
                // Define the archival configuration object
                var configuration = new DataArchivalConfiguration
                {
                    // Enable archiving
                    Disabled = false,
                    
                    // Optional: override the default archiving frequency
                    // Scan for documents scheduled for archiving every 180 seconds 
                    ArchiveFrequencyInSec = 180,
                    
                    // Optional: limit the number of documents processed in each archival run
                    MaxItemsToProcess = 100 
                };
                
                // Define the archival operation, pass the configuration 
                var configureArchivalOp = new ConfigureDataArchivalOperation(configuration);
    
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(configureArchivalOp);
                #endregion
            }
        }
    
        private interface IFoo
        {
            /*
            #region syntax_1
            public ConfigureDataArchivalOperation(DataArchivalConfiguration configuration)
            #endregion
            */
                
            /*
            #region syntax_2
            public class DataArchivalConfiguration
            {
                public bool Disabled { get; set; }
                public long? ArchiveFrequencyInSec { get; set; }
                public long? MaxItemsToProcess { get; set; }
            }
            #endregion
            */
        }
    }
}

