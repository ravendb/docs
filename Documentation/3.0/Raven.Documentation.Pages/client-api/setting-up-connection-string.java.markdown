# How to setup connection string?

In Java we don't support named connection string, however you can pass actual connection string to `DocumentStore` by calling `parseConnectionString` on uninitialized store. The RavenDB client will load automatically configuration based on connection string's parameters.

{CODE:java connection_string_1@ClientApi\SetupConnectionString.java /}

## Format

RavenDB connection string format is:

| Parameters | | |
| ------------- | ------------- | ----- |
| **ApiKey** | string | API key to use when accessing the server. |
| **Url** | string | Specify where to locate the server. |
| **Failover** | string in predefined format | Failover server definition. Read more [here](../client-api/bundles/how-client-integrates-with-replication-bundle#failover-servers). |
| **Database** or **DefaultDatabase** | string | Use a specific database, not the default one. Using this will also ensure that the database exists. |

## Examples

The following are samples of a few RavenDB connection strings:

{CODE:java connection_string_2@ClientApi\SetupConnectionString.java /}

## Related articles

- [How to setup a default database?](./setting-up-default-database)