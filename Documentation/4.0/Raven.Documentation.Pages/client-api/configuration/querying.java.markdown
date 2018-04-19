#Conventions Related to Querying

##ThrowIfQueryPageSizeIsNotSet

Since RavenDB 4.0 there is no limitation for the number of results returned for a single query by the server. The `ThrowIfQueryPageSizeIsNotSet` convention decides whether RavenDB Client
should prevent from executing queries if their page size is not explicitly set. 

Enabling this configuration at development stage can be useful to pinpoint all the possible performance bottlenecks.

{CODE:java throw_if_query_page_is_not_set@ClientApi\Configuration\Querying.java /}
