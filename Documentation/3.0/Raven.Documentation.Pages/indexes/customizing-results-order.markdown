# Customizing results order using SortOptions

Indexes in RavenDB are lexicographically sorted by default, so all queries return results which are ordered lexicographically. When putting a static index in RavenDB, you can specify custom sorting requirements, to ensure results are sorted the way you want them to.

## Native types

Dates are written to the index in a form which preserves lexicography order, and is readable by both human and machine (like so: `2011-04-04T11:28:46.0404749+03:00`), so this requires no user intervention, too.

Numerical values, on the other hand, are stored as text and therefore require the user to specify explicitly what is the number type used so a correct sorting mechanism is enforced. This is quite easily done, by declaring the required sorting setup in `SortOptions` in the index definition:

{CODE static_sorting1@Indexes\CustomizingResultsOrder.cs /}

The index outlined above will allow sorting by value on the number of units in stock (1, 2, 3, 11, etc) for each `Product`. If we wouldn't specify this option, it would have been sorted lexically (1, 11, 2, 3, etc).

The default `SortOptions` value is `String`. Appropriate values available for all numeric types (`Byte`, `Double`, `Float`, `Int`, `Long` and `Short`).

{NOTE Specifing the `Sort` in the index definition won't make results from this index be ordered unless you call `OrderBy` on the query itself. /}

### Example

In the following query we are using the `OrderBy` method in order to indicate that we want to sort on `UnitsInStock`:

{CODE static_sorting3@Indexes\CustomizingResultsOrder.cs /}

So, by default it will sort on `UnitsInStock` as it was a string. By specifing `Sort(x => x.UnitsInStock, SortOptions.Int)` in the index definition, we inidicate that the sort order should be in a numerical order. 

## Collation support

RavenDB supports using collations for documents sorting and indexing. You can setup a specific collation for an index field, so you can sort based of culture specific rules.

The following is an example of an index definition which allows sorting based on the Swedish lexical sorting rules:

{CODE static_sorting2@Indexes\CustomizingResultsOrder.cs /}

In general, you can sort using `Raven.Database.Indexing.Collation.Cultures.<two-letters-culture-name>CollationAnalyzer`, and _all_ the cultures supported by the .NET framework are supported.

## Related articles

- [Map indexes](../indexes/map-indexes)
- [What are indexes?](../indexes/what-are-indexes)
- [Querying : Basics](../indexes/querying/basics)
- [Querying : Sorting](../indexes/querying/sorting)
