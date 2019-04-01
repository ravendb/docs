# Conventions: Querying

##FindPropertyNameForIndex and FindPropertyNameForDynamicIndex

These two conventions specify functions that are used to find the indexed property name for a static index or a dynamic one. This can be useful when you are indexing nested properties 
of complex types. Their default implementations are:

{CODE find_prop_name@ClientApi\Configuration\Querying.cs /}

The arguments in the order of appearance: an indexed document type, an index name, a current path and a property path.

##ThrowIfQueryPageSizeIsNotSet

Since RavenDB 4.0 there is no limitation for the number of results returned for a single query by the server. The `ThrowIfQueryPageSizeIsNotSet` convention decides whether RavenDB Client
should prevent from executing queries if their page size is not explicitly set. 

Enabling this configuration at development stage can be useful to pinpoint all the possible performance bottlenecks.

{CODE throw_if_query_page_is_not_set@ClientApi\Configuration\Querying.cs /}

## Related articles

### Conventions

- [Conventions](../../client-api/configuration/conventions)
- [Serialization](../../client-api/configuration/serialization)
- [Load Balance & Failover](../../client-api/configuration/load-balance-and-failover)

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
