# OpenTelemetry Support
---

{NOTE: }

* OpenTelemetry is a popular monitoring standard designated to help in the inspection and 
  administration of networks, infrastructures, databases, etc.  

* RavenDB sends data metrics via an OpenTelemetry Protocol protocol, 
  allowing a OpenTelemetry retriever to scrape the data from RavenDB.  

* A OpenTelemetry support is provided by RavenDB instances both on-premise and on the cloud.  

* You can also retrieve data for OpenTelemetry collector from Prometheus endpoint.

{NOTE/}

---

{PANEL: OpenTelemetry}

OpenTelemetry is a collection of APIs, SDKs, and tools. Use it to instrument, generate, collect, and export telemetry data (metrics, logs, and traces) to help you analyze your software's performance and behavior. (description via [https://opentelemetry.io](https://opentelemetry.io))

RavenDB utilize official SDK and allows user to retrieve the metrics via OpenTelemetry protocol and much more!

{PANEL/}

{PANEL: RavenDB OpenTelemetry Metrics}

{INFO: How to turn on metrics in RavenDB}
To enable metrics in RavenDB, you need to set the configuration option `Monitoring.OpenTelemetry.Enabled` to `true`. 
Please remember that to apply the changes, it is necessary to restart the RavenDB process.
{INFO/}

{INFO: Identifaction of nodes in metrices}
RavenDB exposes the node tag to identify metrics specific to machines in the instruments' instance tag.
{INFO/}

RavenDB exposes the following metrics:

| Name | Description |
| :--- | :--- |
| ravendb.serverwide.general | Exposes general info about server |
| ravendb.serverwide.requests | Exposes informations about requests processed by server |
| ravendb.serverwide.storage | Exposes storage informations |
| ravendb.serverwide.gc | Exposes detailed informations about Garbage Collector |
| ravendb.serverwide.resources | Exposes detailed information about resources usage (e.g. CPU etc) |
| ravendb.serverwide.totaldatabases | Exposes aggregated informations about databases on the server |
| ravendb.serverwide.cpucredits | Exposes status of CPU credits (cloud) |

We also support exposing metrices developed by Microsoft for AspNetCore and also .NET Runtime.
More info about it can be found on official Microsoft documentations:
- [Runtime documentation](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.Runtime#metrics)
- [AspNetCore documentation](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Instrumentation.AspNetCore/README.md#metrics)

### Ommiting meters
Omitting meters is configurable through our standard configuration settings.

| Configuration name | Meter name | Default value |
| :--- | :--- | :--- |
| Monitoring.OpenTelemetry.Metrics.AspNetCoreInstrumentation.Enabled | Official AspNetCore instrumentation | false |
| Monitoring.OpenTelemetry.Metrics.RuntimeInstrumentation.Enabled | Official Runtime instrumentation | false |
| Monitoring.OpenTelemetry.ServerWide.Storage.Enabled | ravendb.serverwide.storage | true |
| Monitoring.OpenTelemetry.ServerWide.CPUCredits.Enabled | ravendb.serverwide.cpucredits | false|
| Monitoring.OpenTelemetry.ServerWide.Resources.Enabled | ravendb.serverwide.resources | true |
| Monitoring.OpenTelemetry.ServerWide.TotalDatabases.Enabled | ravendb.serverwide.totaldatabases | true |
| Monitoring.OpenTelemetry.ServerWide.Requests.Enabled | ravendb.serverwide.requests | true |
| Monitoring.OpenTelemetry.ServerWide.GC.Enabled | ravendb.serverwide.gc | false |

### Meters instruments
| Name | Description | Instrument type |
| :--- | :--- | :--- |
| ravendb.serverwide.cpucredits.alert_raised | CPU Credits Any Alert Raised | Gauge |
| ravendb.serverwide.cpucredits.background.tasks.alert_raised | CPU Credits Background Tasks Alert Raised | Gauge |
| ravendb.serverwide.cpucredits.base | CPU Credits Base | UpDownCounter |
| ravendb.serverwide.cpucredits.consumption_current | CPU Credits Gained Per Second | UpDownCounter |
| ravendb.serverwide.cpucredits.failover.alert_raised | CPU Credits Failover Alert Raised | Gauge |
| ravendb.serverwide.cpucredits.max | CPU Credits Max | UpDownCounter |
| ravendb.serverwide.cpucredits.remaining | CPU Credits Remaining | Gauge |
| ravendb.serverwide.gc.compacted | Specifies if this is a compacting GC or not. | Gauge |
| ravendb.serverwide.gc.concurrent | Specifies if this is a concurrent GC or not. | Gauge |
| ravendb.serverwide.gc.finalizationpendingcount | Gets the number of objects ready for finalization this GC observed. | Gauge |
| ravendb.serverwide.gc.fragmented | Gets the total fragmentation (in MB) when the last garbage collection occurred. | Gauge |
| ravendb.serverwide.gc.gclohsize | Gets the large object heap size (in MB) after the last garbage collection of given kind occurred. | Gauge |
| ravendb.serverwide.gc.generation | Gets the generation this GC collected. | Gauge |
| ravendb.serverwide.gc.heapsize | Gets the total heap size (in MB) when the last garbage collection occurred. | Gauge |
| ravendb.serverwide.gc.highmemoryloadthreshold | Gets the high memory load threshold (in MB) when the last garbage collection occurred. | Gauge |
| ravendb.serverwide.gc.index | The index of this GC. | Gauge |
| ravendb.serverwide.gc.memoryload | Gets the memory load (in MB) when the last garbage collection occurred. | Gauge |
| ravendb.serverwide.gc.pausedurations1 | Gets the pause durations. First item in the array. | Gauge |
| ravendb.serverwide.gc.pausedurations2 | Gets the pause durations. Second item in the array. | Gauge |
| ravendb.serverwide.gc.pinnedobjectscount | Gets the number of pinned objects this GC observed. | Gauge |
| ravendb.serverwide.gc.promoted | Gets the promoted MB for this GC. | Gauge |
| ravendb.serverwide.gc.timepercentage | Gets the pause time percentage in the GC so far. | Gauge |
| ravendb.serverwide.gc.totalavailablememory | Gets the total available memory (in MB) for the garbage collector to use when the last garbage collection occurred. | Gauge |
| ravendb.serverwide.gc.totalcommited | Gets the total committed MB of the managed heap. | Gauge |
| ravendb.serverwide.general.certificate_server_certificate_expiration_left_seconds | Server certificate expiration left | Gauge |
| ravendb.serverwide.general.cluster.index | Cluster index | UpDownCounter |
| ravendb.serverwide.general.cluster.node.state | Current node state | UpDownCounter |
| ravendb.serverwide.general.cluster.term | Cluster term | UpDownCounter |
| ravendb.serverwide.general.license.cores.max | Server license max CPU cores | Gauge |
| ravendb.serverwide.general.license.cpu.utilized | Server license utilized CPU cores | Gauge |
| ravendb.serverwide.general.license.expiration_left_seconds | Server license expiration left | Gauge |
| ravendb.serverwide.general.license.type | Server license type | Gauge |
| ravendb.serverwide.resources.available_memory_for_processing | Available memory for processing \(in MB\) | Gauge |
| ravendb.serverwide.resources.cpu.machine | Machine CPU usage in % | Gauge |
| ravendb.serverwide.resources.cpu.process | Process CPU usage in % | Gauge |
| ravendb.serverwide.resources.dirty_memory | Dirty Memory that is used by the scratch buffers in MB | Gauge |
| ravendb.serverwide.resources.encryption_buffers.memory_in_pool | Server encryption buffers memory being in pool in MB | Gauge |
| ravendb.serverwide.resources.encryption_buffers.memory_in_use | Server encryption buffers memory being in use in MB | Gauge |
| ravendb.serverwide.resources.io_wait | IO wait in % | Gauge |
| ravendb.serverwide.resources.low_memory_flag | Server low memory flag value | Gauge |
| ravendb.serverwide.resources.machine.assigned_processor_count | Number of assigned processors on the machine | UpDownCounter |
| ravendb.serverwide.resources.machine.processor_count | Number of processor on the machine | UpDownCounter |
| ravendb.serverwide.resources.managed_memory | Server managed memory size in MB | Gauge |
| ravendb.serverwide.resources.thread_pool.available_completion_port_threads | Number of available completion port threads in the thread pool | Gauge |
| ravendb.serverwide.resources.thread_pool.available_worker_threads | Number of available worker threads in the thread pool | Gauge |
| ravendb.serverwide.resources.total_memory | Server allocated memory in MB | Gauge |
| ravendb.serverwide.resources.total.swap_usage | Server total swap usage in MB | Gauge |
| ravendb.serverwide.resources.total.swap.size | Server total swap size in MB | Gauge |
| ravendb.serverwide.resources.unmanaged_memory | Server unmanaged memory size in MB | Gauge |
| ravendb.serverwide.resources.working_set_swap_usage | Server working set swap usage in MB | Gauge |
| ravendb.serverwide.requests.requests.average_duration | Average request time in milliseconds | Gauge |
| ravendb.serverwide.requests.requests.concurrent_requests | Number of concurrent requests | UpDownCounter |
| ravendb.serverwide.requests.requests.per_second | Number of requests per second. | Gauge |
| ravendb.serverwide.requests.tcp.active.connections | Number of active TCP connections | Gauge |
| ravendb.serverwide.requests.total.requests | Total number of requests since server startup | UpDownCounter |
| ravendb.serverwide.storage.storage.disk.ios.read_operations | IO read operations per second | Gauge |
| ravendb.serverwide.storage.storage.disk.ios.write_operations | IO write operations per second | Gauge |
| ravendb.serverwide.storage.storage.disk.queue_length | Queue length | Gauge |
| ravendb.serverwide.storage.storage.disk.read_throughput | Read throughput in kilobytes per second | Gauge |
| ravendb.serverwide.storage.storage.disk.remaining.space | Remaining server storage disk space in MB | Gauge |
| ravendb.serverwide.storage.storage.disk.remaining.space_percentage | Remaining server storage disk space in % | Gauge |
| ravendb.serverwide.storage.storage.disk.write_throughput | Write throughput in kilobytes per second | Gauge |
| ravendb.serverwide.storage.storage.total_size | Server storage total size in MB | Gauge |
| ravendb.serverwide.storage.storage.used_size | Server storage used size in MB | Gauge |
| ravendb.serverwide.totaldatabases.count_stale_indexes | Number of stale indexes in all loaded databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.data.written.per_second | Number of bytes written \(documents, attachments, counters\) in all loaded databases | Gauge |
| ravendb.serverwide.totaldatabases.database.disabled_count | Number of disabled databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.database.encrypted_count | Number of encrypted databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.database.faulted_count | Number of faulted databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.database.loaded_count | Number of loaded databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.database.node_count | Number of databases for current node | UpDownCounter |
| ravendb.serverwide.totaldatabases.database.total_count | Number of all databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.map_reduce.index.mapped_per_second | Number of maps per second for map-reduce indexes \(one minute rate\) in all loaded databases | Gauge |
| ravendb.serverwide.totaldatabases.map_reduce.index.reduced_per_second | Number of reduces per second for map-reduce indexes \(one minute rate\) in all loaded databases | Gauge |
| ravendb.serverwide.totaldatabases.map.index.indexed_per_second | Number of indexed documents per second for map indexes \(one minute rate\) in all loaded databases | Gauge |
| ravendb.serverwide.totaldatabases.number_error_indexes | Number of error indexes in all loaded databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.number_of_indexes | Number of indexes in all loaded databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.number.faulty_indexes | Number of faulty indexes in all loaded databases | UpDownCounter |
| ravendb.serverwide.totaldatabases.writes_per_second | Number of writes \(documents, attachments, counters\) in all loaded databases | Gauge |

{PANEL/}

{PANEL: OpenTelemetry exporters}
{INFO: Exporters}
RavenDB currently supports two options for metrics export:

- OpenTelemetry Protocol
- Console

{INFO/}
### Console
All metrices will be printed in RavenDB console. This is useful for local development and debugging purposes

### OpenTelemetryProtocol
Official protocol for OpenTelemetry is supported by default. You can export your metrices to the software that support this protocol. The suggested software, provided by OpenTelemetry authors  is called OpenTelemetry Collector. It allows to gather all data from RavenDB and configure your favorite tools as retrievers of metrics. 

Best source knowledge about its possibilities is the official documentation site: [https://opentelemetry.io/docs/collector/](https://opentelemetry.io/docs/collector/)

RavenDB by default is not overriding default values for OpenTelemetryProtocol exporter, however customization is available.
| Configuration key | Description | Accepted values |
| :--- | :--- | :--- |
| Monitoring.OpenTelemetry.OpenTelemetryProtocol.Endpoint | Endpoint where OpenTelemetryProtocol should sends data. | string |
| Monitoring.OpenTelemetry.OpenTelemetryProtocol.Protocol | Defines the protocol that OpenTelemetryProtocol should use to send data. | Grpc / HttpProtobuf |
| Monitoring.OpenTelemetry.OpenTelemetryProtocol.Headers | Custom headers | string |
| Monitoring.OpenTelemetry.OpenTelemetryProtocol.ExportProcessorType | Export processor type | Simple / Batch |
| Monitoring.OpenTelemetry.OpenTelemetryProtocol.Timeout | Timeout | int |

{INFO: Setting protocol to HttpProtobuf}
Currently, official .NET implementation requires to provide complete path to the collector endpoint. By default for OpenTelemetry collector it is `/v1/metrics`.
For example, default OpenTelemetryCollector setting endpoint for `HttpProtobuf` is `http://localhost:4318/v1/metrics`.
{INFO/}

{PANEL/}

{PANEL: OpenTelemetry Collector}
### Configuring OpenTelemetry protocol in collector

{CODE-BLOCK: json}
receivers:
  otlp:
    protocols:
      grpc:
        endpoint: localhost:4317
      http:
        endpoint: localhost:4318

{CODE-BLOCK/}


### Prometheus endpoint as data source in collector
OpenTelemetryCollector contributors added support to retrieve metrices from prometheus. Our Prometheus endpoint provides metrices in a well-known format and it works as plug-in without requiring any custom configuration. An example configuration may look like this:

{CODE-BLOCK:json}
receivers:
  prometheus_simple:
    endpoint: "your_ravendb_server.run"
    metrics_path: "/admin/monitoring/v1/prometheus"
    collection_interval: 10s
    tls:
      cert_file: "D:\\cert.crt"
      key_file: "D:\\key.key"
      insecure: false
      insecure_skip_verify: false
{CODE-BLOCK/}
{PANEL/}

