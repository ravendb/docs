# Indexing LINQ extensions

Various indexing LINQ extensions are available to enhance the usability and reduce the complexity of indexing functions. The available extensions are:

- [StripHtml](../indexes/indexing-linq-extensions#striphtml)
- [Boost](../indexes/indexing-linq-extensions#boost)
- [Reverse](../indexes/indexing-linq-extensions#reverse)
- [IfEntityIs](../indexes/indexing-linq-extensions#ifentityis)
- [WhereEntityIs](../indexes/indexing-linq-extensions#whereentityis)

{PANEL:**StripHtml**}

This extension can come in handy when you want to index a HTML without any HTML tags e.g. for full text search.

{CODE-TABS}
{CODE-TAB:csharp:Index indexes_1@Indexes/IndexingLinqExtensions.cs /}
{CODE-TAB:csharp:Query indexes_2@Indexes/IndexingLinqExtensions.cs /}
{CODE-TAB:csharp:Article indexes_3@Indexes/IndexingLinqExtensions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:**Boost**}

You can read more about boosting [here](../indexes/boosting).

{PANEL/}

{PANEL:**Reverse**}

**Strings** and **enumerables** can be reversed by using `Reverse` extension.

{CODE-TABS}
{CODE-TAB:csharp:Index indexes_4@Indexes/IndexingLinqExtensions.cs /}
{CODE-TAB:csharp:Query indexes_5@Indexes/IndexingLinqExtensions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:**WhereEntityIs**}

`WhereEntityIs` can be used to check if given `Raven-Entity-Name` value in metadata for given document matches any of the given values. This can be useful when indexing polymorphic data. Please visit dedicated article to get more information (or click [here](../indexes/indexing-polymorphic-data#other-ways)).

{PANEL/}

{PANEL:**IfEntityIs**}

`IfEntityIs` is similar to `WhereEntityIs`, but it checks only against one value.

{PANEL/}

## Related articles

- [Map indexes](../indexes/map-indexes)
- [Boosting](../indexes/boosting)