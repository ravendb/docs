# Indexes: Map Indexes
---

{NOTE: }

* `Map` indexes, sometimes referred to as simple indexes, contain one (or more) mapping functions 
  that indicate which fields from the documents should be indexed.  
* After indexing, documents can be searched by the indexed fields.  
* The mapping functions are **LINQ-based functions**; they can be considered the **core** of indexes.  

* In This Page:  
   * [Indexing single fields](../indexes/map-indexes#indexing-single-fields)  
   * [Combining multiple fields](../indexes/map-indexes#combining-multiple-fields)  
   * [Indexing partial field data](../indexes/map-indexes#indexing-partial-field-data)  
   * [Indexing nested data](../indexes/map-indexes#indexing-nested-data)  
   * [Indexing Missing Fields](../indexes/map-indexes#indexing-missing-fields)  

{INFO: Also see:}

* Indexing fields from [related documents](../indexes/indexing-related-documents)  
* Aggregating data with [Map-Reduce indexes](../indexes/map-reduce-indexes)  
* Indexing multiple collections with [Multi-Map indexes](../indexes/indexing-polymorphic-data#multi-map-indexes)  
* [Running calculations and storing the results in the index to reduce query time](https://demo.ravendb.net/demos/csharp/static-indexes/store-fields-in-index)  

{INFO/}

{NOTE/}

---

{PANEL: Indexing Single Fields}

Let's create an index that will help us search for `Employees` by their `FirstName`, `LastName`, or both.

- First, let's create an index called `Employees/ByFirstAndLastName`

{CODE:nodejs indexes_1@indexes/map.js /}

- The next step is to create the indexing function itself. This is done by setting the `map` field with mapping function in the **constructor**.

{CODE:nodejs indexes_2@indexes/map.js /}

- The final step is to [deploy it](../indexes/creating-and-deploying) to the server and issue a query using the session [Query](../client-api/session/querying/how-to-query) method:

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_4@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO:Field Types}

Please note that indexing capabilities are detected automatically from the returned field type from the indexing function. 

For example, if our `Employee` will have a property called `Age` that is an `number` then the following indexing function...

{CODE-BLOCK:javascript}
`from employee in docs.Employees
select new
{
	Age = employee.Age
}`
{CODE-BLOCK/}

...grant us the capability to issue numeric queries (**return all the Employees that `Age` is more than 30**). 

Changing the `Age` type to a `string` will take that capability away from you. The easiest example would be to issue `.ToString()` on the `Age` field...

{CODE-BLOCK:javascript}
`from employee in docs.Employees
select new
{
	Age = employee.Age.ToString()
}`
{CODE-BLOCK/}

{INFO/}

{WARNING: Convention}

You will probably notice that in the `Studio`, this function is a bit different from the one defined in the `Employees_ByFirstAndLastName` class:

{CODE-BLOCK:javascript}
`from employee in docs.Employees
select new
{
    FirstName = employee.FirstName,
    LastName = employee.LastName
}`
{CODE-BLOCK/}

The part you should pay attention to is `docs.Employees`. This syntax indicates from which collection a server should take the documents for indexing. In our case, documents will be taken from the `Employees` collection. To change the collection, you need to change `Employees` to the desired collection name or remove it and leave only `docs` to index **all documents**.

{WARNING/}

{PANEL/}

{PANEL: Combining Multiple Fields}

Since each index contains a LINQ function, you can combine multiple fields into one.

#### Example I

{CODE:nodejs indexes_7@indexes/map.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_8@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName'
where FullName = 'Robert King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Example II

{INFO: Information}

In this example, the index field `Query` combines all values from various Employee fields into one. The default Analyzer on field is changed to enable `Full Text Search` operations. The matches no longer need to be exact.

You can read more about analyzers and `Full Text Search` [here](../indexes/using-analyzers).

{INFO/}

{CODE:nodejs indexes_1_6@indexes/map.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_1_7@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/Query'
where search(Query, 'John Doe')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing Partial Field Data}

Imagine that you would like to return all employees that were born in a specific year. You can do it by indexing `Birthday` from `Employee` in the following way:

{CODE:nodejs indexes_1_2@indexes/map.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_5_1@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByBirthday '
where Birthday between '1963-01-01' and '1963-12-31T23:59:59.9990000'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

RavenDB gives you the ability to extract field data and to index by it. A different way to achieve our goal will look as follows:

{CODE:nodejs indexes_1_0@indexes/map.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_6_1@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByYearOfBirth'
where YearOfBirth = 1963
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing Nested Data}

If your document contains nested data, e.g. `Employee` contains `Address`, you can index on its fields by accessing them directly in the index. Let's say that we would like to create an index that returns all employees that were born in a specific `Country`:

{CODE:nodejs indexes_1_4@indexes/map.js /}

{CODE-TABS}
{CODE-TAB:nodejs:Query indexes_7_1@indexes/map.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByCountry'
where Country = 'USA'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

If a document relationship is represented by the document's ID, you can use the `LoadDocument` method to retrieve such a document. More about it can be found [here](../indexes/indexing-related-documents).

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
