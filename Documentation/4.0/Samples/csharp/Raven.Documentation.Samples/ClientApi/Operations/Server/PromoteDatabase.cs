using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class PromoteDatabase
    {
        private interface IFoo
        {
            /*
            #region promote_1
            public PromoteDatabaseNodeOperation(string databaseName, string node)
            #endregion
            */
        }

        public PromoteDatabase()
        {
            using (var store = new DocumentStore())
            {
                #region promote_2
                PromoteDatabaseNodeOperation promoteOperation = new PromoteDatabaseNodeOperation("Northwind", "C");
                store.Maintenance.Server.Send(promoteOperation);
                #endregion
            }
        }
    }
}
