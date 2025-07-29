# Conventions: Querying

## FindPropertyNameForIndex and FindPropertyNameForDynamicIndex

These two conventions specify functions that are used to find the indexed property name for a static index or a dynamic one. This can be useful when you are indexing nested properties 
of complex types. Their default implementations are:

{CODE find_prop_name@ClientApi\Configuration\Querying.cs /}

The arguments in the order of appearance: an indexed document type, an index name, a current path and a property path.

##FindProjectedPropertyNameForIndex

This convention specifies a function that is used to find the projected property name for a static index.  
This can be useful when you want to use a static index to project nested properties which are not [`Stored`](../../indexes/storing-data-in-index).

{CODE find_projected_prop_name@ClientApi\Configuration\Querying.cs /}

Same as in the two conventions above, the arguments in the order of appearance: an indexed document type, an index name, a current path and a property path.

By default `FindProjectedPropertyNameForIndex` is set to `null`.  
When `FindProjectedPropertyNameForIndex` is `null` (or returns `null`), the `FindPropertyNameForIndex` convention is used instead.

###Example
Consider we have the following index, and we want to project `School.Id`:
{CODE users_index@ClientApi\Configuration\Querying.cs /}

Without setting `FindProjectedPropertyNameForIndex`, `FindPropertyNameForIndex` will be used :

{CODE-TABS}
{CODE-TAB:csharp:Query find_projected_prop_query@ClientApi\Configuration\Querying.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'UsersIndex'
where School_Id != null
select School_Id
{CODE-TAB-BLOCK/}
{CODE-TABS/}

`School_Id` is indexed but not `Stored`, meaning that we will try to
fetch `School_Id` from the document - which doesn't have this property.

Setting the `FindProjectedPropertyNameForIndex` convention can solve this issue:
{CODE find_projected_prop_usage@ClientApi\Configuration\Querying.cs /}

{CODE-TABS}
{CODE-TAB:csharp:Query find_projected_prop_query@ClientApi\Configuration\Querying.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'UsersIndex'
where School_Id != null
select School.Id
{CODE-TAB-BLOCK/}
{CODE-TABS/}

##ThrowIfQueryPageSizeIsNotSet

Since RavenDB 4.0 there is no limitation for the number of results returned for a single query by the server. The `ThrowIfQueryPageSizeIsNotSet` convention decides whether RavenDB Client
should prevent from executing queries if their page size is not explicitly set. 

Enabling this configuration at development stage can be useful to pinpoint all the possible performance bottlenecks.

{CODE throw_if_query_page_is_not_set@ClientApi\Configuration\Querying.cs /}

## Related articles

### Conventions

- [Conventions](../../client-api/configuration/conventions)
- [Serialization](../../client-api/configuration/serialization)
- [Load Balance & Failover](../../client-api/configuration/load-balance/overview)

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
