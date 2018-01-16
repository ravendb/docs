using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class ChangeIndexLockMode
    {
        private interface IFoo
        {
            /*
            #region set_lock_mode_1
            public SetIndexesLockOperation(string indexName, IndexLockMode lockMode);
            public SetIndexesLockOperation(Parameters parameters);
            #endregion
            */
        }

        private class Foo
        {
            #region set_lock_mode_2
            public enum IndexLockMode
            {
                Unlock = 0,
                LockedIgnore = 1,
                LockedError = 2
            }
            #endregion
        }

        #region set_lock_mode_3
        public class Parameters
        {
            public string[] IndexNames { get; set; }
            public IndexLockMode Mode { get; set; }
        }
        #endregion

        public ChangeIndexLockMode()
        {

            using (var store = new DocumentStore())
            {
                #region set_lock_mode_4
                store.Maintenance.Send(new SetIndexesLockOperation("Orders/Totals", IndexLockMode.LockedIgnore));
                #endregion

                #region set_lock_mode_5
                store.Maintenance.Send(new SetIndexesLockOperation(new SetIndexesLockOperation.Parameters
                {
                    IndexNames = new []{ "Orders/Totals", "Orders/ByCompany" },
                    Mode = IndexLockMode.LockedIgnore
                }));
                #endregion
            }
        }
    }
}
