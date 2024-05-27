using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class SetPriority
    {
        public SetPriority()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region set_priority_single
                    // Define the set priority operation
                    // Pass index name & priority
                    var setPriorityOp = new SetIndexesPriorityOperation("Orders/Totals", IndexPriority.High);

                    // Execute the operation by passing it to Maintenance.Send
                    // An exception will be thrown if index does not exist
                    store.Maintenance.Send(setPriorityOp);
                    #endregion
                }
                {
                    #region set_priority_multiple
                    // Define the index list and the new priority:
                    var parameters = new SetIndexesPriorityOperation.Parameters
                    {
                        IndexNames = new[] {"Orders/Totals", "Orders/ByCompany"},
                        Priority = IndexPriority.Low
                    };

                    // Define the set priority operation, pass the parameters
                    var setPriorityOp = new SetIndexesPriorityOperation(parameters);

                    // Execute the operation by passing it to Maintenance.Send
                    // An exception will be thrown if any of the specified indexes do not exist
                    store.Maintenance.Send(setPriorityOp);
                    #endregion
                }
            }
        }
        
        public async Task SetPriorityAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region set_priority_single_async
                    // Define the set priority operation
                    // Pass index name & priority
                    var setPriorityOp = new SetIndexesPriorityOperation("Orders/Totals", IndexPriority.High);
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if index does not exist
                    await store.Maintenance.SendAsync(setPriorityOp);
                    #endregion
                }
                {
                    #region set_priority_multiple_async
                    // Define the index list and the new priority:
                    var parameters = new SetIndexesPriorityOperation.Parameters 
                    {
                        IndexNames = new[] {"Orders/Totals", "Orders/ByCompany"}, 
                        Priority = IndexPriority.Low
                    };
                
                    // Define the set priority operation, pass the parameters
                    var setPriorityOp = new SetIndexesPriorityOperation(parameters);

                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if any of the specified indexes do not exist
                    await store.Maintenance.SendAsync(setPriorityOp);
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            // Available overloads:
            public SetIndexesPriorityOperation(string indexName, IndexPriority priority);
            public SetIndexesPriorityOperation(Parameters parameters);
            #endregion
            */
        }

        private class Priority
        {
            #region syntax_2
            public enum IndexPriority
            {
                Low,
                Normal,
                High
            }
            #endregion
        }
        
        #region syntax_3
        public class Parameters
        {
            public string[] IndexNames { get; set; }
            public IndexPriority Priority { get; set; }
        }
        #endregion
    }
}
