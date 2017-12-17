using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class SwitchOperationsToDifferentDatabase
    {
        private interface ForDatabase1
        {
            #region for_database_1
            OperationExecutor ForDatabase(string databaseName);
            #endregion
        }
        private interface ForDatabase2
        {
            #region for_database_2
            MaintenanceOperationExecutor ForDatabase(string databaseName);
            #endregion
        }

        public SwitchOperationsToDifferentDatabase()
        {
            using (var documentStore = new DocumentStore())
            {
                #region for_database_3
                OperationExecutor operations = documentStore.Operations.ForDatabase("otherDatabase");
                #endregion

                #region for_database_4
                MaintenanceOperationExecutor maintenanceOperations = documentStore.Maintenance.ForDatabase("otherDatabase");
                #endregion
            }

        }     
    }
}
