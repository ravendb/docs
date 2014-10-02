# How to setup connection string?

.NET named connection strings are supported by Client API. You can use them by setting the `ConnectionStringName` property value in `DocumentStore`, and the RavenDB client will initialize automatically based on the connection string's parameters.

{CODE connection_string_1@ClientApi\SetupConnectionString.cs /}

You can define the connection string in the application configuration file (`app.config` or `web.config`).

{CODE-BLOCK:plain}
<connectionStrings>
    <add name="Local" connectionString="DataDir = ~\Data"/>
    <add name="Server" connectionString="Url = http://localhost:8080"/>
    <add name="Secure" connectionString="Url = http://localhost:8080;user=beam;password=up;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a"/>
</connectionStrings>
{CODE-BLOCK/}

## Format

RavenDB connection string format is:

| Parameters | | |
| ------------- | ------------- | ----- |
| **ApiKey** | string | API key to use when accessing the server. |
| (Embedded only) **DataDir** | string | Specify which directory to run from. |
| **Enlist** | bool | Indicates if client should enlist in distributed transactions. Default: `True`. |
| **ResourceManagerId** | Guid | Resource Manager Id that will be used by the Distributed Transaction Coordinator (DTC) service to identify Raven. A custom resource manager id will need to be configured for each Raven server instance when Raven is hosted more than once per machine. |
| **Url** | string | Specify where to locate the server. |
| **Failover** | string in predefined format | Failover server definition. Read more [here](../client-api/bundles/how-client-integrates-with-replication-bundle#failover-servers). |
| **Database** or **DefaultDatabase** | string | Use a specific database, not the default one. Using this will also ensure that the database exists. |
| **User**, **Password** and **Domain** | string | Credentials to use when accessing the server. |

## Examples

The following are samples of a few RavenDB connection strings:

{CODE connection_string_2@ClientApi\SetupConnectionString.cs /}

## Related articles

TODO