#  Indexes: Sorting & Collation

Starting from version 4.0, RavenDB automatically determines sorting based on an indexed value. All values will have a capability to be sorted `lexicographically`. Numerical values will also be sortable by their `double` and `long` value.

## Date types

Dates are written to the index in a form which preserves lexicography order, and is readable by both human and machine (like so: `2011-04-04T11:28:46.0404749+03:00`). This requires no user intervention.

## Example

Please read our dedicated article describing `sorting` capabilities when queries are executed. It can be found [here](../indexes/querying/sorting).

## Collation

RavenDB supports using collations for documents sorting and indexing. You can setup a specific collation for an index field, so you can sort based of culture specific rules.

The following is an example of an index definition which allows sorting based on the Swedish lexical sorting rules:

{CODE-TABS}
{CODE-TAB:java:LINQ static_sorting2@Indexes\Sorting.java /}
{CODE-TAB:java:JavaScript static_sorting2@Indexes\JavaScript.java /}
{CODE-TABS/}

In general, you can sort using `Raven.Database.Indexing.Collation.Cultures.<two-letters-culture-name>CollationAnalyzer`, and _all_ the cultures supported by the .NET framework are supported.

## Related Articles

### Indexes

- [Map Indexes](../indexes/map-indexes)
- [What are Indexes](../indexes/what-are-indexes)

### Querying

- [Basics](../indexes/querying/basics)
- [Sorting](../indexes/querying/sorting)
