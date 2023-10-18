using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Sorting;
using Raven.Client.ServerWide.Operations.Sorters;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server.Sorters
{
    public class PutSorterServerWide
    {
        public PutSorterServerWide() 
        {
            using (var store = new DocumentStore())
            {
                #region put_sorter
                // Assign the code of your custom sorter as a `string`
                string mySorterCode = "<code of custom sorter>";

                // Create the `SorterDefinition` object
                var customSorterDefinition = new SorterDefinition
                {
                    // The sorter Name must be the same as the sorter's class name in your code
                    Name = "MySorter",
                    // The Code must be compilable and include all necessary using statements
                    Code = mySorterCode
                };

                // Define the put sorters operation, pass the sorter definition
                // Note: multiple sorters can be passed, see syntax below
                var putSortersServerWideOp = new PutServerWideSortersOperation(customSorterDefinition);
                 
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(putSortersServerWideOp);
                #endregion
            }
        }
        
        public async Task EnableIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region put_sorter_async
                // Assign the code of your custom sorter as a `string`
                string mySorterCode = "<code of custom sorter>";

                // Create the `SorterDefinition` object
                var customSorterDefinition = new SorterDefinition
                {
                    // The sorter Name must be the same as the sorter's class name in your code
                    Name = "MySorter",
                    // The Code must be compilable and include all necessary using statements
                    Code = mySorterCode
                };

                // Define the put sorters operation, pass the sorter definition
                // Note: multiple sorters can be passed, see syntax below
                var putSortersServerWideOp = new PutServerWideSortersOperation(customSorterDefinition);
                 
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                await store.Maintenance.Server.SendAsync(putSortersServerWideOp);
                #endregion
            }
        }
        
        /*
        #region syntax_1
        public PutServerWideSortersOperation(params SorterDefinition[] sortersToAdd)
        #endregion
        */
        
        /*
        #region syntax_2
        public class SorterDefinition
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }
        #endregion
        */
    }
}
