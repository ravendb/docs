using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.Queue;
using Raven.Client.Documents.Operations.ETL.SQL;
namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.Queue
{
    public class ConnectionStrings
    {
        private interface IFoo
        {
            #region queue-broker-type
            public enum QueueBrokerType
            {
                None,
                Kafka,
                RabbitMq
            }
            #endregion

        }

        public ConnectionStrings()
        {
            using (var store = new DocumentStore())
            {
                #region add_rabbitMQ_connection-string
                var res = store.Maintenance.Send(new PutConnectionStringOperation<QueueConnectionString>(new QueueConnectionString
                {
                    Name = "RabbitMqConStr",
                    BrokerType = QueueBrokerType.RabbitMq,
                    RabbitMqConnectionSettings = new RabbitMqConnectionSettings() { ConnectionString = "amqp://guest:guest@localhost:5672/" }
                }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_kafka_connection-string
                var res = store.Maintenance.Send(new PutConnectionStringOperation<QueueConnectionString>(new QueueConnectionString
                {
                    Name = "KafkaConStr",
                    BrokerType = QueueBrokerType.Kafka,
                    KafkaConnectionSettings = new KafkaConnectionSettings() { BootstrapServers = "localhost:29092" }
                }));
                #endregion
            }
        }
    }
}
