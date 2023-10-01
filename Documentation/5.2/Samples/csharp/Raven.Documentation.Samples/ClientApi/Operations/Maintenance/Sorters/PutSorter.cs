using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Sorters;
using Raven.Client.Documents.Queries.Sorting;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Sorters
{
    public class PutSorter
    {
        public PutSorter() 
        {
            using (var store = new DocumentStore())
            {
                #region put_sorter_1
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
                var putSortersOp = new PutSortersOperation(customSorterDefinition);
                 
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(putSortersOp);
                #endregion
            }
        }
        
        public async Task EnableIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region put_sorter_1_async
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
                var putSortersOp = new PutSortersOperation(customSorterDefinition);
                 
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(putSortersOp);
                #endregion
            }
        }
        
        /*
        #region syntax_1
        public PutSortersOperation(params SorterDefinition[] sortersToAdd)
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
