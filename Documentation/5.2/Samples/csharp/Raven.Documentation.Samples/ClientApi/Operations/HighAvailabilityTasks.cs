using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.SQL;
using Raven.Client.Documents.Operations.ETL.OLAP;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class HighAvailabilityTasks
    {
        public HighAvailabilityTasks()
        {
            using (var store = new DocumentStore())
            {
                #region add_raven_etl_task

                AddEtlOperation<RavenConnectionString> operation = new AddEtlOperation<RavenConnectionString>(
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
                        },

                        // Pin the task to prevent failover to another node
                        PinToMentorNode = true

                    });
                
                AddEtlOperationResult result = store.Maintenance.Send(operation);
                #endregion
            }
        }
    }
}

