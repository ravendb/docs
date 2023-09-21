# Configuration: Queue Sink
---

{PANEL: `QueueSink.MaxBatchSize`}

The maximum number of pulled messages consumed in a single batch.  

- **Default**: `8192`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

{PANEL/}

{PANEL: `QueueSink.MaxFallbackTimeInSec`}

The maximum number of seconds the queue sink process will be in a fallback 
mode (i.e. suspending the process) after a connection failure.  

- **Default**: `15*60`
- **TimeUnit**: `TimeUnit.Seconds`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

{PANEL/}

## Related Articles

### Server

- [Queue Sink: Overview](../../server/ongoing-tasks/queue-sink/overview)  
- [Queue Sink: Kafka](../../server/ongoing-tasks/queue-sink/kafka-queue-sink)  
- [Queue Sink: RabbitMQ](../../server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink)  
- [Queue ETL: Overview](../../server/ongoing-tasks/etl/queue-etl/overview)  
- [Queue ETL: Kafka](../../server/ongoing-tasks/etl/queue-etl/kafka)  
- [Queue ETL: RabbitMQ](../../server/ongoing-tasks/etl/queue-etl/rabbit-mq)  

### Studio

- [Queue Sink: Kafka](../../studio/database/tasks/ongoing-tasks/kafka-queue-sink)  
- [Queue Sink: RabbitMQ](../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink)  
- [Queue ETL: Kafka](../../studio/database/tasks/ongoing-tasks/kafka-etl-task)  
- [Queue ETL: RabbitMQ](../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)  

### Configuration
- [Configuration Options](../../server/configuration/configuration-options)
