# Indexes: Map Indexes
---

{NOTE: }

* `Map` indexes, sometimes referred to as simple indexes, contain one or more mapping functions 
  to indicate which document fields should be indexed.  
* After indexing, documents can be searched by the indexed fields.  
* The mapping functions can be considered the **core** of indexes.  

* In This Page:  
   * [Indexing single fields](../indexes/map-indexes#indexing-single-fields)
   * [Combining multiple fields](../indexes/map-indexes#combining-multiple-fields)
   * [Indexing partial field data](../indexes/map-indexes#indexing-partial-field-data)
   * [Filtering data within fields](../indexes/map-indexes#filtering-data-within-fields)
   * [Indexing nested data](../indexes/map-indexes#indexing-nested-data)
   * [Indexing Missing Fields](../indexes/map-indexes#indexing-missing-fields)

{INFO: Also see:}

* Indexing fields from [related documents](../indexes/indexing-related-documents)  
* Aggregating data with [Map-Reduce indexes](../indexes/map-reduce-indexes)  
* Indexing multiple collections with [Multi-Map indexes](../indexes/indexing-polymorphic-data#multi-map-indexes)  
* [Running calculations and storing the results in the index to reduce query time](https://demo.ravendb.net/demos/python/static-indexes/store-fields-in-index)  

{INFO/}

{NOTE/}

---

{PANEL: Indexing single fields}

Let's create an index that will help us search for `Employees` by their `FirstName`, `LastName`, or both.

* First, let's create an index called `Employees/ByFirstAndLastName`  
   {NOTE: }
   Note: The naming separator character "`_`" in your code will become "/" in the index name.  
   In the following sample, "`Employees_ByFirstAndLastName`" will become "Employees/ByFirstAndLastName" in your indexes list.
   {NOTE/}

{CODE-TABS}
{CODE-TAB:php:Index indexes_1@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_1@Indexes/JavaScript.php /}
{CODE-TABS/}

You might notice that we're passing `Employee` as a generic parameter to `AbstractIndexCreationTask`.  
This gives our indexing function a strongly-typed syntax. If you are not familiar with `AbstractIndexCreationTask`, 
you can read [this](../indexes/creating-and-deploying) article before proceeding.

- The next step is to create the indexing function itself. This is done by setting the `map` property with our function in a **parameterless constructor**.

{CODE-TABS}
{CODE-TAB:php:Query indexes_2@Indexes/Map.php /}
{CODE-TAB:php:JavaScript javaScriptindexes_2@Indexes/JavaScript.php /}
{CODE-TABS/}

- The final step is to [deploy it](../indexes/creating-and-deploying) to the server 
  and issue a query using the session [Query](../client-api/session/querying/how-to-query) method.  
  To query an index, the name of the index must be called by the query.  
  If the index isn't called, RavenDB will either use or create an [auto index](../indexes/creating-and-deploying#auto-indexes).

{CODE-TABS}
{CODE-TAB:php:Query indexes_4@Indexes/Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This is how our final index looks like:

{CODE-TABS}
{CODE-TAB:php:Index indexes_6@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_6@Indexes/JavaScript.php /}
{CODE-TABS/}

{INFO:Field Types}

Please note that indexing capabilities are detected automatically from the returned field type from the indexing function. 

For example, if our `Employee` has a property named `Age` that is an `int`, the following indexing function...

{CODE-TABS}
{CODE-TAB-BLOCK:php:Function}
from employee in docs.Employees
select new
{
	Age = employee.Age
}
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:php:JavaScript-syntax}
map('Employees', function(employee)
{
    return {
        Age : employee.Age
    };
})
{CODE-TAB-BLOCK/}
{CODE-TABS/}



...grants us the capability to issue numeric queries (**return all the Employees whose Age is more than 30**). 

Changing the `Age` type to a `string` will take that capability away from you. The easiest example would be to issue `.ToString()` on the `Age` field...

{CODE-TABS}
{CODE-TAB-BLOCK:php:Function}
from employee in docs.Employees
select new
{
	Age = employee.Age.ToString()
}
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:php:JavaScript-syntax}
map('Employees', function(employee)
{
    return {
        Age : employee.Age.toString()
    };
})
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO/}

{WARNING: Convention}

You will probably notice that in the `Studio`, this function is a bit different from the one defined in the `Employees_ByFirstAndLastName` class:

{CODE-BLOCK:php}
from employee in docs.Employees
select new
{
	FirstName = employee.FirstName,
	LastName = employee.LastName
}
{CODE-BLOCK/}

The part you should pay attention to is `docs.Employees`. This syntax indicates from which collection a server should take the documents for indexing. In our case, documents will be taken from the `Employees` collection. To change the collection, you need to change `Employees` to the desired collection name or remove it and leave only `docs` to index **all documents**.

{WARNING/}

{PANEL/}

{PANEL: Combining multiple fields}

Since each index contains a function, you can combine multiple fields into one.

#### Example
Index definition:  
{CODE-TABS}
{CODE-TAB:php:Function indexes_7@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_7@Indexes/JavaScript.php /}
{CODE-TABS/}
  
Query the index:  
{CODE-TABS}
{CODE-TAB:php:Query indexes_8@Indexes/Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName'
where FullName = 'Robert King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing partial field data}

Imagine that you would like to return all employees that were born in a specific year. 
You can do it by indexing `Birthday` from `Employee`, then specify the year in `Birthday` as you query the index:  

Index definition:  
{CODE-TABS}
{CODE-TAB:php:Index_Definition indexes_1_2@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_1_2@Indexes/JavaScript.php /}
{CODE-TABS/}
  
Query the index:  
{CODE-TABS}
{CODE-TAB:php:Query indexes_5_1@Indexes/Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByBirthday '
where Birthday between '1963-01-01' and '1963-12-31T23:59:59.9990000'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

RavenDB gives you the ability **to extract field data and to index by it**. A different way to achieve our goal will look as follows:  

Index definition:  
{CODE-TABS}
{CODE-TAB:php:Index_Definition indexes_1_0@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_1_0@Indexes/JavaScript.php /}
{CODE-TABS/}

Query the index:  
{CODE-TABS}
{CODE-TAB:php:Query indexes_6_1@Indexes/Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByYearOfBirth'
where YearOfBirth = 1963
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filtering data within fields}

In the examples above, `where_equals` is used in the query to filter the results.  
If you consistently want to filter with the same filtering conditions, 
you can use `where_equals` in the index definition to narrow the index terms that the query must scan.  

This can save query-time but narrows the terms available to query.

#### Example I

For logic that has to do with special import rules that only apply to the USA  
`where` can be used to filter the Companies collection `Address.Country` field.  
Thus, we only index documents `where company.Address.Country == "USA"` . 

Index definition:
{CODE:php indexes_1_6@Indexes\Map.php /}

Query the index:
{CODE-TABS}
{CODE-TAB:php:Query indexes_query_1_6@Indexes\Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies_ByAddress_Country'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Example II

Imagine a seed company that needs to categorize its customers by latitude-based growing zones.  

They can create a different index for each zone and filter their customers in the index with  
`where (company.Address.Location.Latitude > 20 && company.Address.Location.Latitude < 50)` .

Index definition:
{CODE:php indexes_1_7@Indexes\Map.php /}

Query the index:
{CODE-TABS}
{CODE-TAB:php:Query indexes_query_1_7@Indexes\Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Companies_ByAddress_Latitude'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}  


{PANEL: Indexing nested data}

If your document contains nested data, e.g. `Employee` contains `Address`, you can index on its fields by accessing them directly in the index. Let's say that we would like to create an index that returns all employees that were born in a specific `Country`:  

Index definition:  
{CODE-TABS}
{CODE-TAB:php:Index_Definition indexes_1_4@Indexes/Map.php /}
{CODE-TAB:php:JavaScript-syntax javaScriptindexes_1_4@Indexes/JavaScript.php /}
{CODE-TABS/}

Query the index:  
{CODE-TABS}
{CODE-TAB:php:Query indexes_7_1@Indexes/Map.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByCountry'
where Country = 'USA'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

If a document relationship is represented by the document's ID, you can use the `LoadDocument` method to retrieve such a document.  
Learn more [here](../indexes/indexing-related-documents).  

{PANEL/}

{PANEL: Indexing Missing Fields}

By default, indexes will not index a document that contains none of the specified fields. This behavior can be changed 
using the [Indexing.IndexEmptyEntries](../server/configuration/indexing-configuration#indexing.indexemptyentries) 
configuration option.  

The option [Indexing.IndexMissingFieldsAsNull](../server/configuration/indexing-configuration#indexing.indexmissingfieldsasnull) 
determines whether missing fields in documents are indexed with the value `null`, or not indexed at all.  

{PANEL/}

## Related Articles

### Indexes
- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying
- [Query Overview](../client-api/session/querying/how-to-query)
- [Querying an Index](../indexes/querying/query-index)

### Studio
- [Create Map Index](../studio/database/indexes/create-map-index)

---

### Code Walkthrough

- [Static Indexes Overview](https://demo.ravendb.net/demos/python/static-indexes/static-indexes-overview)
- [Map Index](https://demo.ravendb.net/demos/python/static-indexes/map-index)
- [Map-Reduce Index](https://demo.ravendb.net/demos/python/static-indexes/map-reduce-index)
- [Project Index Results](https://demo.ravendb.net/demos/python/static-indexes/project-index-results)
- [Store Fields in Index](https://demo.ravendb.net/demos/python/static-indexes/store-fields-in-index)

