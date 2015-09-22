# Map indexes

`Map` indexes (sometimes referred to as simple indexes) contain one (or more) mapping functions that indicate which fields from the documents should be indexed (in other words they indicate which documents can be searched by which fields). 

These **mapping functions** are the **LINQ-based functions** and can be considered to be the indexes **core**.

## What can be indexed?

The easiest answer to this question is practically anything. You can:

- [index single fields](../indexes/map-indexes#basics---indexing-single-fields)
- [combined multiple fields](../indexes/map-indexes#combining-multiple-fields-together) together
- [index partial field data](../indexes/map-indexes#indexing-partial-field-data)
- [index nested data](../indexes/map-indexes#indexing-nested-data)
- [index fields from related documents](../indexes/indexing-related-documents)
- [index fields from multiple collections](../indexes/indexing-polymorphic-data#multi-map-indexes)
- ...and so on. 

Various articles in this part of documentation will describe possibilities in detail.

## Basics - indexing single fields

To start, let's create an index that will help us to search for `Employees` by their `FirstName`, `LastName` (or both).

- first, let's start creating an index called `Employees/ByFirstAndLastName`

{CODE indexes_1@Indexes/Map.cs /}

You probably noticed that we're passing `Employee` as a generic parameter to `AbstractIndexCreationTask`. Thanks to that, our indexing function will have a strongly-typed syntax. If you are not familiar with `AbstractIndexCreationTask`, then you should read [this](../indexes/creating-and-deploying) article before proceeding.

- the next step is to create an indexing function itself, and to do it we need to set the `Map` property with our function in **parameterless constructor**.

{CODE-TABS}
{CODE-TAB:csharp:Query-syntax indexes_2@Indexes/Map.cs /}
{CODE-TAB:csharp:Method-syntax indexes_3@Indexes/Map.cs /}
{CODE-TABS/}

- the final step is to [deploy it](../indexes/creating-and-deploying) (omitted) to server and issue a query using session [Query](../client-api/session/querying/how-to-query) method:

{CODE indexes_4@Indexes/Map.cs /}
{CODE indexes_5@Indexes/Map.cs /}

Our final index looks like:

{CODE indexes_6@Indexes/Map.cs /}

{WARNING:Convention}

You will probably notice that in the `Studio` this function is a bit different from the one defined in the `Employees_ByFirstAndLastName` class, and look like this:

{CODE-BLOCK:json}
from doc in docs.Employees
select new
{
	FirstName = doc.FirstName,
	LastName = doc.LastName
}
{CODE-BLOCK/}

The part you should pay attention to is `docs.Employees`. This syntax indicates from which collection a server should take the documents for indexing. In our case documents will be taken from `Employees` collection. To change the collection you need to change `Employees` to desired collection name or remove it and leave only `docs` to index **all documents**.

{WARNING/}

## Combining multiple fields together

Since under each index there is a LINQ-function, you can combine multiple fields in one, if necessary. When the document is being indexed it is not a problem.

{CODE indexes_7@Indexes/Map.cs /}

{CODE-TABS}
{CODE-TAB:csharp:Query indexes_8@Indexes/Map.cs /}
{CODE-TAB:csharp:DocumentQuery indexes_9@Indexes/Map.cs /}
{CODE-TABS/}

## Indexing partial field data

Imagine that you would like to return all employees that were born in a specific year. You could of course do it by indexing `Birthday` from `Employee` in the following way:

{CODE indexes_1_2@Indexes/Map.cs /}

{CODE indexes_1_3@Indexes/Map.cs /}

However, RavenDB gives you an ability to extract field data and to index by it, so a different way to achieve our goal will look as follows:

{CODE indexes_1_0@Indexes/Map.cs /}

{CODE indexes_1_1@Indexes/Map.cs /}

## Indexing nested data

If our document contains nested data, e.g. `Employee` contains `Address`, you can index by its fields by accessing them directly in index. Let's say that we would like to create an index that would return all employees that were born in specific `Country`:

{CODE indexes_1_4@Indexes/Map.cs /}

{CODE indexes_1_5@Indexes/Map.cs /}

If a document relationship is represented by the document's Id, you can use `LoadDocument` method to retrieve such a document. More about it can be found [here](../indexes/indexing-related-documents).

## Indexing multiple collections

Please read an article dedicated to `Multi-Map` indexes that can be found [here](../indexes/indexing-polymorphic-data#multi-map-indexes).

## Related articles

- [Indexing related documents](../indexes/indexing-related-documents)
- [Map-Reduce indexes](../indexes/map-reduce-indexes)
- [Creating and deploying indexes](../indexes/creating-and-deploying)
- [Querying : Basics](../indexes/querying/basics)
