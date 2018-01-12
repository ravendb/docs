# Boosting

A that RavenDB leverages from Lucene is called Boosting. This feature gives you the ability to manually tune the relevance level of matching documents when performing a query.


## Boosting in query

Indexing in RavenDB is built upon the Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. 

For example:

{CODE-TABS}
{CODE-TAB:csharp:Query boosting_1_0@Indexes\Querying\Boosting.cs /}
{CODE-TAB:csharp:DocumentQuery boosting_2_1@Indexes\Querying\Boosting.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This search will promote users who do sports before book readers and they will be placed at the top of the results list.

<hr />


{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery boosting_2_1@Indexes\Querying\Boosting.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where boost(startsWith(Name, 'G'), 10) or boost(startsWith(Name, 'A'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This shows users which name starts with letter 'G' or 'A'. Results which starts with 'G' goes first. 



## Remarks

{INFO Boosting is also available at the index definiton level. You can read more about it [here](../../indexes/boosting). /}


## Related Articles

- [Indexing : Boosting](../../indexes/boosting)
- [Indexing : Querying : Searching](../../indexes/querying/searching)
