# Cloud: Product Features
---

{PANEL: OLAP ETL }

*RavenDB Cloud* provides *OLAP ETL* capabilities, allowing you to integrate your operational data with analytical data
warehouses.

Usage of this feature is described [here](../studio/database/tasks/ongoing-tasks/olap-etl-task).

{PANEL/}

{PANEL: PowerBi }

*RavenDB Cloud* supports direct integration with *Power BI*, a leading business analytics tool. This feature allows you
to seamlessly connect your RavenDB data with *Power BI* for advanced visualization and reporting, enabling real-time
insights and data-driven decision-making.

Usage of this feature is described [here](../integrations/postgresql-protocol/power-bi).

## Configuration

{WARNING: Required }
*Power Bi* IPs must be configured after enabling *PowerBi* feature.
{WARNING/}

{WARNING: Product Restart }
Your product will be restarted automatically node by node to apply settings needed for feature to work properly.
{WARNING/}

To access *Power Bi* Configuration View click **Configure** Button in *Power Bi* feature row. Here you can edit your IP
addresses that will have access to *RavenDB Power Bi* port.

!["Power Bi Configuration View"](images/product-features-powerbi-configuration.png "Power Bi Configuration View")

{INFO: }
*Power Bi* IPs entries are [CIDR ranges](https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing#CIDR_notation)
that define networks from which the connection is allowed.
{INFO/}

{PANEL/}

{PANEL: Queue ETL }

The *Queue ETL* feature enables you to export data from *RavenDB* to various queue systems, such as *RabbitMQ* or
*Kafka*.
This is particularly useful for scenarios where you need to process data asynchronously or distribute it
to other microservices and applications.

Usage of this feature is described [here](../server/ongoing-tasks/etl/queue-etl/overview).

{PANEL/}

{PANEL: Queue Sink }

*Queue Sink* allows *RavenDB Cloud* to directly receive data from queues, making it easier to ingest data into your
database from external systems or services. This feature is ideal for integrating with event-driven architectures
where data is processed and ingested in real time.

Usage of this feature is described [here](../server/ongoing-tasks/queue-sink/overview).

{INFO: Availability }
This feature is available only for *RavenDB* **6.0 and newer**.
{INFO/}

{PANEL/}

{PANEL: Data Archival }

*RavenDB Cloud* offers the ability to archive selected documents.
Archived documents are compressed and can be handled differently by *RavenDB* functions (e.g. Indexing can exclude
archived documents from indexes and Data Subscriptions can avoid sending archived docs to workers), helping to keep the
database smaller and quicker and its contents more relevant.

Usage of this feature is described [here](../server/extensions/archival).

{INFO: Availability }
This feature is available only for *RavenDB* **6.0 and newer**.
{INFO/}

{PANEL/}

{PANEL: Monitoring }

*RavenDB Cloud* allows you to monitor your *RavenDB Cloud* instances via *SNMP* protocol. *SNMP* is a widely used network
management protocol that allows for the collection and organization of information about managed devices on IP networks.

## Configuration

Click **Configure** Button in *Monitoring* feature row in order to configure this feature.
You will see Monitoring Configuration View. You can edit SNMP Monitoring IPs in the first section.
The second section displays SNMPv3 Monitoring credentials.

!["Monitoring Configuration View"](images/product-features-monitoring-configuration.png "Monitoring Configuration View")

### SNMP Monitoring IPs

{WARNING: Required }
*SNMP Monitoring* IPs must be configured **after** enabling *Monitoring* feature.
{WARNING/}

{WARNING: Product Restart }
Your product will be restarted automatically (node by node) to apply settings needed for feature to work properly.
{WARNING/}

This section allows you to specify IPs that will have access to your product using the *SNMP* protocol.

!["Edit SNMP Monitoring IPs"](images/product-features-monitoring-edit-ips.png "Edit SNMP Monitoring IPs")

### SNMPv3 Monitoring credentials

SNMPv3 is the most secure version of SNMP, providing enhanced security features over its predecessors. When setting up
SNMPv3 monitoring for your *RavenDB Cloud* instances, you need to configure specific credentials that ensure secure
communication between the SNMP agent on your end and the RavenDB.

Credentials required to connect to *RavenDB Monitoring* Endpoints are provided in this section. We also provide sample
command you can use to test connection.

{INFO: }
SNMP Monitoring IPs entries
are [CIDR ranges](https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing#CIDR_notation)
that define networks from which the connection is allowed.
{INFO/}

{PANEL/}

