# JavaScript Indexes
---

{NOTE: }

* JavaScript indexes are intended for users who prefer to define static indexes using JavaScript instead of C#.

* The RavenDB JavaScript engine supports ECMAScript 5.1 syntax.  
  In addition, RavenDB provides a set of predefined JavaScript functions that can be used in JavaScript indexes and in other features such as subscriptions, ETL scripts, set-based patching, and more.  
  See the full list in [Predefined JavaScript functions](../server/kb/javascript-engine#predefined-javascript-functions).

---

* In this article:
  * [Creating and deploying a JavaScript index](../indexes/javascript-indexes#creating-and-deploying-a-javascript-index)
  * [Map index](../indexes/javascript-indexes#map-index)
     * [Map index - basic](../indexes/javascript-indexes#example-i---map-index---basic)
     * [Map index - with additional sources](../indexes/javascript-indexes#example-ii---map-index---with-additional-sources)
     * [Map index - with inline string compilation](../indexes/javascript-indexes#example-iii---map-index---with-inline-string-compilation)
  * [Multi-Map index](../indexes/javascript-indexes#multi-map-index)
  * [Map-Reduce index](../indexes/javascript-indexes#map-reduce-index)

{NOTE/}

{PANEL: Creating and deploying a JavaScript index}

**Creating a JavaScript index**:

* To create a JavaScript index, define a class that inherits from `AbstractJavaScriptIndexCreationTask`.  
* This base class itself inherits from [AbstractIndexCreationTask](../indexes/creating-and-deploying#define-a-static-index-using-a-custom-class),
  which is the base class for all C# indexes.

{CODE js_index@Indexes\JavaScriptIndexes.cs /}

---

**Deploying a JavaScript index**:  

* Deploy a JavaScript index using the standard creation flow as described in [Deploy a static-index](../indexes/creating-and-deploying#deploy-a-static-index).

* Note that **JavaScript indexes** can be deployed using a [User/Read-Write](../server/security/authorization/security-clearance-and-permissions#section-1) certificate,  
  while **C# static indexes** require a [User/Admin](../server/security/authorization/security-clearance-and-permissions#section) certificate or higher.

* To restrict the creation of JavaScript indexes to database admins (and above),  
  set the [Indexing.Static.RequireAdminToDeployJavaScriptIndexes](../server/configuration/indexing-configuration#indexing.static.requireadmintodeployjavascriptindexes) configuration to `true`.

* All other capabilities and features of JavaScript indexes are identical to those of [C# indexes](../indexes/indexing-basics).

{PANEL/}

{PANEL: Map index}

* A map index contains a single `map` function.  
  To define an index that uses multiple map functions, see the section on [Multi-Map indexes](../indexes/javascript-indexes#multi-map-index) below.

* The `map` function is written as a string and specifies what content from the documents will be indexed.

{CONTENT-FRAME: }

#### Example I - Map index - basic
---

The following index indexes the `FirstName` and `LastName` of employees from the _Employees_ collection.

{CODE index_1@Indexes\JavaScriptIndexes.cs /}

**Query the index**:  
Once the index is deployed, you can query for _Employee_ documents based on the indexed name fields.

{CODE-TABS}
{CODE-TAB:csharp:Query query_1@Indexes\JavaScriptIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstAndLastName/JS"
where LastName == "King"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Example II - Map index - with additional sources
---

* The following index indexes the names of all comment authors (including nested replies) for each _BlogPost_ document.  

* It uses `getNames`, a recursive helper defined in `AdditionalSources`, to traverse every comment level and accumulate author names.

{CODE-TABS}
{CODE-TAB:csharp:The_JS_index index_2@Indexes\JavaScriptIndexes.cs /}
{CODE-TAB:csharp:The_BlogPost_class blog_post_class@Indexes\JavaScriptIndexes.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Example III - Map index - with inline string compilation
---

* To define a JavaScript index using inline string compilation,  
  you must set the [Indexing.AllowStringCompilation](../server/configuration/indexing-configuration#indexing.allowstringcompilation) configuration key to _true_.
  
* The following indexes use inline string compilation to evaluate whether each product’s `UnitsInStock` is low.

{CODE-TABS}
{CODE-TAB:csharp:JS_index_usingFunction index_3@Indexes\JavaScriptIndexes.cs /}
{CODE-TAB:csharp:JS_index_usingEval index_4@Indexes\JavaScriptIndexes.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}

Learn more about [Map indexes here](../indexes/map-indexes).

{PANEL/}

{PANEL: Multi-Map index}

* A Multi-Map index allows indexing data from multiple collections.

* For example, the following index processes documents from both the _Cats_ and _Dogs_ collections.

{CONTENT-FRAME: }

{CODE index_5@Indexes\JavaScriptIndexes.cs /}

**Query the index**:  
Once the index is deployed, querying it will return matching documents from both the _Cats_ and _Dogs_ collections.

{CODE-TABS}
{CODE-TAB:csharp:Query query_2@Indexes\JavaScriptIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Animals/ByName/JS"
where Name == "Milo"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}

Learn more about [Multi-Map indexes here](../indexes/multi-map-indexes).

{PANEL/}

{PANEL: Map-Reduce index}

* A Map-Reduce index allows you to perform complex data aggregations.

* In the **Map** stage, the index processes documents and extracts relevant data using the defined mapping function(s).

* In the **Reduce** stage, the map results are aggregated to produce the final output.

{CONTENT-FRAME: }

#### Example I
---

The following index counts the number of products per category by grouping on the category name.

{CODE index_6@Indexes\JavaScriptIndexes.cs /}

**Query the index**:  
Once the index is deployed, you can query for the total number of products per category,  
and optionally, order the results by product count in descending order.

{CODE-TABS}
{CODE-TAB:csharp:Query query_3@Indexes\JavaScriptIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByCategory/JS"
order by Count as long desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Example II
---

The following index calculates how many items were sold and the total sales amount for each product and month.

{CODE index_7@Indexes\JavaScriptIndexes.cs /}

{CONTENT-FRAME/}

Learn more about [Map-Reduce indexes here](../indexes/map-reduce-indexes).

{PANEL/}

## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying

- [Query Overview](../client-api/session/querying/how-to-query)
- [Querying an Index](../indexes/querying/query-index)
