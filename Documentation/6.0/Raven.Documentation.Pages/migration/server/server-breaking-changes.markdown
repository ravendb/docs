# Migration: Server Breaking Changes
---

{NOTE: }

* The following features and behaviors that were available in previous versions of RavenDB   
  are either unavailable in RavenDB `6.x` or incompatible with those earlier versions.

* In this page:
  * [License keys](../../migration/server/server-breaking-changes#license-keys)
  * [RavenDB for Docker](../../migration/server/server-breaking-changes#ravendb-for-docker)
  * [Unsupported sharding features](../../migration/server/server-breaking-changes#unsupported-sharding-features)
  * [Graph queries](../../migration/server/server-breaking-changes#graph-queries)
  * [SQL ETL](../../migration/server/server-breaking-changes#sql-etl)
  * [DateOnly & TimeOnly](../../migration/server/server-breaking-changes#dateonly-&-timeonly)
  * [Full-text search with wildcards](../../migration/server/server-breaking-changes#full-text-search-with-wildcards)

{NOTE/}

---

{PANEL: License keys}

License keys for versions lower than `6.0` are **not supported** by RavenDB `6.0`.  
If you own a valid license key for RavenDB `5.x` or lower, please upgrade it using 
the quick online interface described [here](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x).

{PANEL/}

{PANEL: RavenDB for Docker}

RavenDB now applies an improved security model, and uses a **dedicated user** rather than `root`.  
Read more about this change [here](../../migration/server/docker).  

{PANEL/}

{PANEL: Unsupported sharding features}

RavenDB `6.0` introduces [sharding](../../sharding/overview). 
Server and client features that are currently unavailable in a sharded database (but remain available in regular databases) are listed [here](../../sharding/unsupported).  

{PANEL/}

{PANEL: Graph queries}

[Graph Queries](https://ravendb.net/docs/article-page/5.4/csharp/indexes/querying/graph/graph-queries-overview) support, 
available in RavenDB versions `4.2` to `5.x`, has been removed from the RavenDB `6.x` server and client API.  

{PANEL/}

{PANEL: SQL ETL}

SQL ETL no longer tolerates errors on `Load`, load errors are thrown immediately.  
This is done to distinguish partial load errors that are used in SQL ETL from, for example, commit errors that may happen during load.
(Prior to this change, the ETL would just advance instead of retrying.)

{PANEL/}

{PANEL: DateOnly & TimeOnly}

[DateOnly and TimeOnly](../../client-api/how-to/using-timeonly-and-dateonly) types are now supported for every new auto index.  

{PANEL/}

{PANEL: Full-text search with wildcards}

Starting with `6.0` we have changed how the [Search method](../../indexes/querying/searching) handles wildcards when they are included in search terms:

{NOTE: }

##### Behavior for versions lower than `6.0`:  

After the analyzer stripped wildcards from the search term, 
RavenDB would attempt to restore the `*` to their original positions before sending the term to the search engine (Lucene or Corax). 

{NOTE/}

{NOTE: }

##### Behavior for `6.0` and up:

Once wildcards are stripped by the analyzer, we no longer add them back before sending the term to the search engine.
The search terms sent to the search engine are solely based on the transformations applied by the analyzer used in the index.

Note the different behavior in the following cases:  

* **When using `StandardAnalyzer` or `NGramAnalyzer`**:  
  The queried terms in the _Search_ method are processed with the `LowerCaseKeywordAnalyzer` before being sent to the search engine.
* **When using a custom analyzer**:  
  The queried terms in the _Search_ method are processed according to the custom analyzer's logic.
* **When using the Exact analyzer**:  
  The queried terms in the _Search_ method remain untouched as produced by the exact analyzer. 

{INFO: }
When using **Corax** as the search engine,  
this behavior will only apply to indexes that are newly created or have been reset.
{INFO/}

See detailed examples in: [Searching with wildcards](../../indexes/querying/searching#searching-with-wildcards).

{NOTE/}
{PANEL/}
