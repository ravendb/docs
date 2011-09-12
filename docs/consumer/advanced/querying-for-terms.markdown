# Querying for terms

RavenDB lets you retrieve all terms indexed in a Lucene index for a specific field. Fields are usually your entity properties, but depending on how you built your indexes they may contain different information.

To get all unique values indexed for "MyProperty" in a specific index, with a page size of 128, use:

{CODE getterms1@Consumer\Faceted.cs /}

The third parameter is used for paging. Pass it the last term of the last page you got, and it will generate the next page for you.

This is useful for implementing [Faceted Search](http://en.wikipedia.org/wiki/Faceted_search) for example.