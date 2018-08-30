using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class UpdateEtl
    {
        private interface IFoo
        {
            /*
            #region update_etl_operation
            public UpdateEtlOperation(long taskId, EtlConfiguration<T> configuration)
            #endregion
            */
        }

        public UpdateEtl()
        {
            using (var store = new DocumentStore())
            {
                AddEtlOperationResult addEtlResult = new AddEtlOperationResult();
                #region update_etl_example

                // AddEtlOperationResult addEtlResult = store.Maintenance.Send(new AddEtlOperation<RavenConnectionString>() { ... });

                UpdateEtlOperation<RavenConnectionString> operation = new UpdateEtlOperation<RavenConnectionString>(
                    addEtlResult.TaskId,
                    new RavenEtlConfiguration
                    {
                        ConnectionStringName = "raven-connection-string-name",
                        Name = "Employees ETL",
                        Transforms =
                        {
                            new Transformation
                            {
                                Name = "Script #1",
                                Collections =
                                {
                                    "Employees"
                                },
                                Script = @"loadToEmployees ({
                                        Name: this.FirstName + ' ' + this.LastName,
                                        Title: this.Title
                                });"
                            }
                        }
                    });

                UpdateEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }
        }
    }
}
