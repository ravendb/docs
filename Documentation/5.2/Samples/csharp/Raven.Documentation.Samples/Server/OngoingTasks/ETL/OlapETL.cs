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
                                #region script
                                Script = @"
                                    var orderData = {
                                        Company : this.Company,
                                        RequireAt : new Date(this.RequireAt),
                                        ItemsCount: this.Lines.length,
                                        TotalCost: 0
                                    };
                                    var orderDate = new Date(this.OrderedAt);
                                    var year = orderDate.getFullYear();
                                    var month = orderDate.getMonth();
                                    var key = new Date(year, month);
                                    for (var i = 0; i < this.Lines.length; i++) {
                                        var line = this.Lines[i];
                                        orderData.TotalCost += (line.PricePerUnit * line.Quantity);
                                        // load to 'sales' table
                                        loadToSales(key, {
                                            Qty: line.Quantity,
                                            Product: line.Product,
                                            Cost: line.PricePerUnit
                                        });
                                    }
                                    // load to 'orders' table
                                    loadToOrders(key, orderData);
                                    ";
                                #endregion
                            }
                        }
                    });
        */
    }
}
