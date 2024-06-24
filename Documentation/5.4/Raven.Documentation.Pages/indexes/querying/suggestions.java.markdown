# Query for suggestions with index

RavenDB has an indexing mechanism built upon the Lucene engine which has a great suggestions feature. This capability allows a significant improvement of search functionalities enhancing the overall user experience of the application.

Let's consider an example where the users have the option to look for products by their name. The index and query would appear as follows:

{CODE:java suggestions_1@Indexes\Querying\Suggestions.java /}

{CODE:java suggestions_2@Indexes\Querying\Suggestions.java /}

If our database has `Northwind` samples deployed then it will not return any results. However, we can ask RavenDB for help:

{CODE-TABS}
{CODE-TAB:java:Query suggestions_3@Indexes\Querying\Suggestions.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByName' 
select suggest('Name', 'chaig')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

It will produce these suggestions:

    Did you mean?
        chang
        chai

{NOTE:Client API}

You can read more about suggestions in our [Client API](../../client-api/session/querying/how-to-work-with-suggestions) article. 

{NOTE/}

## Suggest Over Multiple Words

RavenDB allows you to perform a suggestion query over multiple words.

{CODE:java query_suggestion_over_multiple_words@Indexes\Querying\Suggestions.java /}

This will produce the following results:

    Did you mean?
        chai
        chang
        chartreuse
        chef
        tofu

## Remarks

{WARNING: Increased indexing time}

Indexes with turned on suggestions tend to use a lot more CPU power than other indexes. This can impact indexing speed (querying is not impacted).

{WARNING/}

## Related Articles

### Client API

- [Query for Suggestions](../../client-api/session/querying/how-to-work-with-suggestions)
