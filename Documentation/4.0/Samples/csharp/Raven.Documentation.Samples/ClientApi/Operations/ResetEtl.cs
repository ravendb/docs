using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class ResetEtl
    {
        private interface IFoo
        {
            /*
            #region reset_etl_1
            public ResetEtlOperation(string configurationName, string transformationName)
            #endregion
            */
        }

        public ResetEtl()
        {

            using (var store = new DocumentStore())
            {
                #region reset_etl_2
                ResetEtlOperation operation = new ResetEtlOperation("OrdersExport", "script1");
                store.Maintenance.Send(operation);
                #endregion
            }
        }
    }
}
