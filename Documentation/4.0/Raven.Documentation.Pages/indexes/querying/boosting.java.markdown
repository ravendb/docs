# Querying: Boosting

Indexing in RavenDB is built upon the Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 

Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 

You can improve your searching mechanism and provide users with much more accurate results.

## Examples

{CODE-TABS}
{CODE-TAB:java:Java boosting_1_0@Indexes\Querying\Boosting.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This search will promote users who do sports before book readers and they will be placed at the top of the results list.

<hr />

{CODE-TABS}
{CODE-TAB:java:Java boosting_2_1@Indexes\Querying\Boosting.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where boost(startsWith(Name, 'G'), 10) or boost(startsWith(Name, 'A'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This shows users which name starts with letter 'G' or 'A'. Results which starts with 'G' go first. 

## Remarks

{INFO Boosting is also available at the index definition level. You can read more about it [here](../../indexes/boosting). /}

## Related Articles

### Indexes

- [Boosting](../../indexes/boosting)

### Querying

- [Searching](../../indexes/querying/searching)
