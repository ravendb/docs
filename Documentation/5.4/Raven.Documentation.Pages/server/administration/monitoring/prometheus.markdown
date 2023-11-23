# Monitoring: Prometheus

---

{NOTE: }

* Prometheus is a popular monitoring tool designated to help in the inspection and 
  administration of networks, infrastructures, databases, etc.  

* RavenDB exposes data metrics via an HTTP endpoint in a Prometheus-compatible format, 
  allowing a Prometheus server to scrape the data from the endpoint and handle it.  

* A Prometheus endpoint is provided by RavenDB instances both on-premise and on the cloud.  

* In this page:  
   * [Prometheus](../../../server/administration/monitoring/prometheus#prometheus)  
   * [RavenDB Prometheus Endpoint](../../../server/administration/monitoring/prometheus#ravendb-prometheus-endpoint)  
       * [Omit or Include Selected Metrics](../../../server/administration/monitoring/prometheus#omit-or-include-selected-metrics)  
       * [Metrics Provided by the Prometheus Endpoint](../../../server/administration/monitoring/prometheus#metrics-provided-by-the-prometheus-endpoint)  
   * [Using the RavenDB Endpoint by a Prometheus Server](../../../server/administration/monitoring/prometheus#using-the-ravendb-endpoint-by-a-prometheus-server)  
       * [Fetching Additional RavenDB Information](../../../server/administration/monitoring/prometheus#fetching-additional-ravendb-information)  

{NOTE/}

---

{PANEL: Prometheus}

Prometheus is commonly used in conjunction with services that expose numeric data via HTTP endpoints.  
A Prometheus server can be used (either manually or by an application using a dedicated library) to 
scrape the data off an endpoint, monitor and store it, and put it to use in additional ways like raising 
alerts related to it or forwarding it to analysis services.  

{PANEL/}

{PANEL: RavenDB Prometheus Endpoint}

The path to the Prometheus endpoint of a RavenDB instance is: `/admin/monitoring/v1/prometheus`  
To inspect the endpoint's output using a browser, add the endpoint path to the RavenDB server's URL.  
E.g. [http://live-test.ravendb.net/admin/monitoring/v1/prometheus](http://live-test.ravendb.net/admin/monitoring/v1/prometheus)  

![RavenDB: Prometheus Endpoint Output](images/ravendb_prometheus-endpoint-output.png "RavenDB: Prometheus Endpoint Output")

* As prometheus handles only numeric values, the endpoint outputs all values as numbers, 
  providing legends that explain what the numbers mean.  
  Metrics values are also explained in the [table below](../../../server/administration/monitoring/prometheus#metrics-provided-by-the-prometheus-endpoint).  

## Omit or Include Selected Metrics

To omit metrics related to selected topics from the data sent to the endpoint, set one or more of the 
flags listed below, in the URL line. Each flag can be set to `true` to omit the topic from the results, 
or to `false` to include it.  
`skipServerMetrics`  
`skipDatabasesMetrics`  
`skipIndexesMetrics`  
`skipCollectionsMetrics`  

E.g., to skip indexing metrics use -  
http://localhost:8080/admin/monitoring/v1/prometheus?skipIndexesMetrics=true
And to skip both indexing and server metrics use -  
http://localhost:8080/admin/monitoring/v1/prometheus?skipIndexesMetrics=true&skipServerMetrics=true

## Metrics Provided by the Prometheus Endpoint

Here is the list of metrics made available by the `/admin/monitoring/v1/prometheus` endpoint.  

| Metrics | Description |
| - | - |
| archived_data_processing_behavior | Archived data processing behavior + `0`/`1`/`2` <br> 0 => ExcludeArchived <br> 1 => IncludeArchived <br> 2 => ArchivedOnly |
| backup_current_number_of_running_backups | Number of currently running backups |
| backup_max_number_of_concurrent_backups | Maximum number of concurrent backups |
| certificate_server_certificate_expiration_left_seconds | Server certificate expiration left in seconds |
| cluster_current_term | Cluster term |
| cluster_index | Cluster index |
| cluster_node_state | Current node state + `0`/`1`/`2` <br> 0 => Passive <br> 1 => Candidate <br> 2 => Follower <br> 3 => LeaderElect <br> 4 => Leader |
| collection_documents_count | Number of documents in collection |
| collection_documents_size_bytes | Size of documents |
| collection_revisions_size_bytes | Size of revisions |
| collection_tombstones_size_bytes | Size of tombstones |
| collection_total_size_bytes | Total size of collection |
| cpu_assigned_processor_count | Number of assigned processors on the machine |
| cpu_machine_io_wait | IO wait in % |
| cpu_machine_usage | Machine CPU usage in % |
| cpu_process_usage | Process CPU usage in % |
| cpu_processor_count | Number of processors on the machine |
| cpu_thread_pool_available_completion_port_threads | Number of available completion port threads in the thread pool |
| cpu_thread_pool_available_worker_threads | Number of available worker threads in the thread pool |
| database_alerts_count | Number of alerts |
| database_attachments_count | Number of attachments |
| database_documents_count | Number of documents |
| database_indexes_auto_count | Number of auto indexes |
| database_indexes_count | Number of indexes |
| database_indexes_errored_count | Number of error indexes |
| database_indexes_disabled_count | Number of disabled indexes |
| database_indexes_errors_count | Number of indexing errors |
| database_indexes_idle_count | Number of idle indexes |
| database_indexes_stale_count | Number of stale indexes |
| database_indexes_static_count | Number of static indexes |
| database_performance_hints_count | Number of performance hints |
| database_rehabs_count | Number of rehabs |
| database_replication_factor | Database replication factor |
| database_revisions_count | Number of revision documents |
| database_statistics_doc_puts_per_second | Number of document puts per second (one minute rate) |
| database_statistics_map_index_indexes_per_second | Number of indexed documents per second for map indexes (one minute rate) |
| database_statistics_map_reduce_index_mapped_per_second | Number of maps per second for map-reduce indexes (one minute rate) |
| database_statistics_map_reduce_index_reduced_per_second | Number of reduces per second for map-reduce indexes (one minute rate) |
| database_statistics_request_average_duration_seconds | Average request time in seconds |
| database_statistics_requests_count | Number of requests from database start |
| database_statistics_requests_per_second | Number of requests per second (one minute rate) |
| database_storage_documents_allocated_data_file_bytes | Documents storage allocated size |
| database_storage_documents_used_data_file_bytes | Documents storage used size |
| database_storage_indexes_allocated_data_file_bytes | Index storage allocated size |
| database_storage_indexes_used_data_file_bytes | Index storage used size |
| database_storage_io_read_operations | Disk IO Read operations |
| database_storage_io_write_operations | Disk IO Write operations |
| database_storage_queue_length | Disk Queue length |
| database_storage_read_throughput_bytes | Disk Read Throughput |
| database_storage_total_allocated_storage_file_bytes | Total storage size |
| database_storage_total_free_space_bytes | Remaining storage disk space |
| database_storage_write_throughput_bytes | Disk Write Throughput |
| database_time_since_last_backup_seconds | Last backup |
| database_unique_attachments_count | Number of unique attachments |
| database_uptime_seconds | Database up-time |
| databases_loaded_count | Number of loaded databases |
| databases_total_count | Number of all databases |
| index_entries_count | Number of entries in the index |
| index_errors | Number of index errors |
| index_is_invalid | Indicates if index is invalid |
| index_lock_mode | Index lock mode + `0`/`1`/`2` <br> 0 => Unlock <br> 1 => LockedIgnore <br> 2 => LockedError |
| index_mapped_per_second | Number of maps per second (one minute rate) |
| index_priority | Index priority + `0`/`1`/`2` <br> 0 => Low <br> 1 => Normal <br> 2 => High |
| index_reduced_per_second | Number of reduces per second (one minute rate) |
| index_state | Index state + `0`/`1`/`2`/`3` <br> 0 => Normal <br> 1 => Disabled <br> 2 => Idle <br> 3 => Error |
| index_status | Index status + `0`/`1`/`2`/`3` <br> 0 => Running  <br> 1 => Paused <br> 2 => Disabled <br> 3 => Pending (for rolling indexes) |
| index_time_since_last_indexing_seconds | Time since last indexing |
| index_time_since_last_query_seconds | Time since last query |
| index_type | Index type + `0`/`1`/`2`/`3`/`4`/`5`/`6`/`7` <br> 0 => None <br> 1 => AutoMap <br> 2 => AutoMapReduce <br> 3 => Map <br> 4 => MapReduce <br> 5 => Faulty <br> 6 => JavaScriptMap <br> 7 => JavaScriptMapReduce |
| license_expiration_left_seconds | Server license expiration left |
| license_max_cores | Server license max CPU cores |
| license_type | Server license type + `-1`/`0`/`1`/`2`/`3`/`4`/`5`/`6` <br> -1 => Invalid <br> 0 => None <br> 1 => Community <br> 2 => Reserved <br> 3 => Professional <br> 4 => Enterprise <br> 5 => Developer <br> 6 => Essential |
| license_utilized_cpu_cores | Server license utilized CPU cores |
| memory_allocated_bytes | Server allocated memory |
| memory_installed_bytes | Installed memory |
| memory_low_memory_severity | Server low memory flag value + `0`/`1`/`2` <br> 0 => None <br> 1 => Low <br> 2 => ExtremelyLow |
| memory_physical_bytes | Physical memory |
| memory_total_dirty_bytes | Dirty memory that is used by the scratch buffers |
| memory_total_swap_size_bytes | Server total swap size |
| memory_total_swap_usage_bytes | Server total swap usage |
| memory_working_set_swap_usage_bytes | Server working set swap usage |
| network_concurrent_requests_count | Number of concurrent requests |
| network_last_authorized_non_cluster_admin_request_time_in_sec | Server last authorized non cluster admin request time |
| network_last_request_time_in_seconds | Server last request time |
| network_requests_per_second | Number of requests per second (one minute rate) |
| network_tcp_active_connections | Number of active TCP connections |
| network_total_requests | Total number of requests since server startup |
| server_disk_remaining_storage_space_percentage | Remaining server storage disk space in % |
| server_disk_system_store_total_data_file_size_bytes | Server storage total size |
| server_disk_system_store_used_data_file_size_bytes | Server storage used size |
| server_info | Server Info |
| server_process_id | Server process ID |
| server_storage_io_read_operations | Disk IO Read operations |
| server_storage_io_write_operations | Disk IO Write operations |
| server_storage_queue_length | Disk Queue length |
| server_storage_read_throughput_bytes | Disk Read Throughput |
| server_storage_total_free_space_bytes | Remaining storage disk space |
| server_storage_write_throughput_bytes | Disk Write Throughput |
| server_uptime_seconds | Server up-time |

{PANEL/}

{PANEL: Using the RavenDB Endpoint by a Prometheus Server}

To direct a Prometheus server to the Prometheus endpoint of a RavenDB instance 
[add an entry](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#scrape_config) 
to the Prometheus `yml` configuration file.  

* **Prometheus.yml**:  
  {CODE-BLOCK:json}
  - job_name: "local-raven-instance"
      # Monitor a local, non-secure RavenDB server
      metrics_path: "/admin/monitoring/v1/prometheus"
      static_configs:
        - targets:
          - "localhost:8080"
  - job_name: "cloud-raven-instance"
      # Monitor 3 RavenDB nodes on the cloud
      scheme: https
      tls_config:
         cert_file: 'a.client.certificate.crt'
         key_file: 'a.client.certificate.key'
      metrics_path: "/admin/monitoring/v1/prometheus"
      static_configs:
        - targets:
          - "a.cloudtest.ravendb.org"  
          - "b.cloudtest.ravendb.org"  
          - "c.cloudtest.ravendb.org"  
  {CODE-BLOCK/}

* When you run the Prometheus server, direct it to the configuration file you want it to use.  
  `./prometheus --config.file=prometheus.yml`
  
* When the Prometheus server is running, you'll be able to provide it with various RavenDB metrics.  
  
    ![Prometheus: Select Metrics](images/prometheus_select-metrics.png "Prometheus: Select Metrics")
    
    ![Prometheus: Metrics Graph](images/prometheus_metrics-graph.png "Prometheus: Metrics Graph")

    * Use the search bar to search for relevant metrics.   
      Typing **raven** will display a list of metrics provided by the endpoint.  

* Metrics can also be found in RavenDB's enpoint output, using the browser.  
  In the following screenshot, for example, we can see that the priority of one of the indexes was updated to 2 (high).
  
    ![RavenDB Endpoint Output: Index Priority](images/RavenDB_changed-index-priority.png "RavenDB Endpoint Output: Index Priority")

    Providing the Prometheus server with the metrics' name will display the event graphically.  

    ![Prometheus Server: Index Priority](images/Prometheus_changed-index-priority.png "Prometheus Server: Index Priority")

## Fetching Additional RavenDB Information

To retrieve additional information about RavenDB, including its version, 
HTTP and TCP URLs, and other details, pass Prometheus: `ravendb_server_info`  

![Additional Information](images/additional-info.png "Additional Information")

{PANEL/}

## Related Articles

### Monitoring
- [RavenDB Telegraf Plugin](../../../server/administration/monitoring/telegraf)  

### Administration
- [SNMP Administration](../../../server/administration/SNMP/snmp)  
- [Zabbix](../../../server/administration/SNMP/setup-zabbix)  

### Integrations
- [PostgreSQL Overview](../../../integrations/postgresql-protocol/overview)  
- [Power BI](../../../integrations/postgresql-protocol/power-bi)  
