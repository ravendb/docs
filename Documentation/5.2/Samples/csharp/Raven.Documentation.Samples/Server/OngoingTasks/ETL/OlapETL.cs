using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents.Operations.ConnectionStrings;

namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL
{
    public interface Example
    {
        /*
        #region connection_string
        public class OlapConnectionString : ConnectionString
        {
            public LocalSettings LocalSettings;
            public S3Settings S3Settings;
        }

        public class S3Settings
        {
            public string BucketName;
            public string CustomServerUrl;
        }

        public class LocalSettings
        {
            public string FolderPath;
        }
        #endregion
        */

        /*
        AddEtlOperation<OlapConnectionString> operation = new AddEtlOperation<OlapConnectionString>(
                    new OlapEtlConfiguration
                    {
                        ConnectionStringName = "olap-connection-string-name",
                        Name = "Orders ETL",
                        Transforms =
                        {
                            new Transformation
                            {
                                Name = "Script #1",
                                Collections =
                                {
                                    "Orders"
                                },
                                Script = @"
                                    #region script
                                    //Define the object that will be added to the table
                                    var orderData = {
                                        Company : this.Company,
                                        RequireAt : new Date(this.RequireAt),
                                        ItemCount: this.Lines.length
                                    };

                                    //Create the partition names
                                    var orderDate = new Date(this.OrderedAt);
                                    var year = orderDate.getFullYear();
                                    var month = orderDate.getMonth();

                                    //Load to the folder: /OrderData/Year=<year>/Month=<month>/
                                    loadToOrderData(partitionBy(['Year', year], ['Month', month]), orderData);
                                    #endregion
                                    ";
                            }
                        }
                    });
        */
    }
}
