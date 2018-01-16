using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class ChangeIndexPriority
    {
        private interface IFoo
        {
            /*
            #region set_priority_1
            public SetIndexesPriorityOperation(string indexName, IndexPriority priority);
            public SetIndexesPriorityOperation(Parameters parameters);
            #endregion
            */
        }

        private class Foo
        {
            #region set_priority_2
            public enum IndexPriority
            {
                Low = 0,
                Normal = 1,
                High = 2
            }
            #endregion
        }

        #region set_priority_3
        public class Parameters
        {
            public string[] IndexNames { get; set; }
            public IndexPriority Priority { get; set; }
        }
        #endregion

        public ChangeIndexPriority()
        {

            using (var store = new DocumentStore())
            {
                #region set_priority_4
                store.Maintenance.Send(new SetIndexesPriorityOperation("Orders/Totals", IndexPriority.High));
                #endregion

                #region set_priority_5
                store.Maintenance.Send(new SetIndexesPriorityOperation(new SetIndexesPriorityOperation.Parameters
                {
                    IndexNames = new []{ "Orders/Totals", "Orders/ByCompany" },
                    Priority = IndexPriority.Low
                }));
                #endregion
            }
        }
    }
}
