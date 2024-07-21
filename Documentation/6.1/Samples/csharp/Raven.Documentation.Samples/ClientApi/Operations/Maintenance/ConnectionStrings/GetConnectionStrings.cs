using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.Queue;
using Raven.Client.Documents.Operations.ETL.SQL;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.ConnectionStrings
{
    public class GetConnectionStrings
    {
        public GetConnectionStrings()
        {
            #region get_connection_string_by_name
            using (var store = new DocumentStore())
            {
                // Request to get a specific connection string, pass its name and type:
                // ====================================================================
                var getRavenConStrOp = 
                    new GetConnectionStringsOperation("ravendb-connection-string-name", ConnectionStringType.Raven);
                
                GetConnectionStringsResult connectionStrings = store.Maintenance.Send(getRavenConStrOp);
                
                // Access results:
                // ===============
                Dictionary<string, RavenConnectionString> ravenConnectionStrings = 
                    connectionStrings.RavenConnectionStrings;
                
                var numberOfRavenConnectionStrings = ravenConnectionStrings.Count;
                var ravenConStr = ravenConnectionStrings["ravendb-connection-string-name"];
                
                var targetUrls = ravenConStr.TopologyDiscoveryUrls;
                var targetDatabase = ravenConStr.Database;
            }
            #endregion
            
            #region get_all_connection_strings
            using (var store = new DocumentStore())
            {
                // Get all connection strings:
                // ===========================
                var getAllConStrOp = new GetConnectionStringsOperation();
                GetConnectionStringsResult allConnectionStrings = store.Maintenance.Send(getAllConStrOp);
                
                // Access results: 
                // ===============
                
                // RavenDB
                Dictionary<string, RavenConnectionString> ravenConnectionStrings = 
                    allConnectionStrings.RavenConnectionStrings;
                
                // SQL
                Dictionary<string, SqlConnectionString> sqlConnectionStrings = 
                    allConnectionStrings.SqlConnectionStrings;
                
                // OLAP
                Dictionary<string, OlapConnectionString> olapConnectionStrings = 
                    allConnectionStrings.OlapConnectionStrings;
                
                // Elasticsearch
                Dictionary<string, ElasticSearchConnectionString> elasticsearchConnectionStrings = 
                    allConnectionStrings.ElasticSearchConnectionStrings;
                
                // Access the Queue ETL connection strings in a similar manner:
                // ============================================================
                Dictionary<string, QueueConnectionString> queueConnectionStrings = 
                    allConnectionStrings.QueueConnectionStrings;
                
                var kafkaConStr = queueConnectionStrings["kafka-connection-string-name"];
            }
            #endregion
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            public GetConnectionStringsOperation()
            public GetConnectionStringsOperation(string connectionStringName, ConnectionStringType type)
            #endregion
            */

            /*
            #region syntax_2
            public enum ConnectionStringType
            {
                Raven,
                Sql,
                Olap,
                ElasticSearch,
                Queue
            }
            #endregion
            */
            
            /*
            #region syntax_3
            public class GetConnectionStringsResult
            {
                public Dictionary<string, RavenConnectionString> RavenConnectionStrings { get; set; }
                public Dictionary<string, SqlConnectionString> SqlConnectionStrings { get; set; }
                public Dictionary<string, OlapConnectionString> OlapConnectionStrings { get; set; }
                public Dictionary<string, ElasticSearchConnectionString> ElasticSearchConnectionStrings { get; set; }
                public Dictionary<string, QueueConnectionString> QueueConnectionStrings { get; set; }
            }
            #endregion 
            */
        }
    }
}
