﻿# New Lucene Query Parser

In RavenDB 3.5 we introduced our own new implementation of the Lucene Query Parser.
In older versions of RavenDB, we encountered several issues with the standard parser:

A major issue was performance and stability of queries. RavenDB uses Lucene 
syntax for queries, but we have [extended it in several ways](./full-query-syntax). Those extensions to the 
syntax were implemented primarily as pre and post processing over the raw query 
string using regular expressions. Under profiling, it turned out that a significant 
amount of time was spent during this processing, and in particular, in those regular expressions.

Another problem we encountered was the fact that the old Lucene Query Parser is relying 
on exceptions for control flow. If you were debugging a test that is using RavenDB, 
you were likely to be stopped by LookAheadSuccessException. The exception was handled 
internally, but the default VS configuration will stop on all exceptions, which 
caused more than a single person to assume that there is actually some error.

The new parser has some significant improvements. We fixed a few performance and syntax issues. 
Most queries, especially long queries, now run a lot faster (by two orders of magnitude). 
We also got rid of exceptions during parsing.

{NOTE: Note:}
In RavenDB 3.5, the new parser is used by default. In order to change this, and use the old
parser, you need to set the `Raven/Indexing/UseLuceneASTParser` 
[configuration option](../../server/configuration/configuration-options) to true.
{NOTE/}
## Related articles

- [Indexing : Querying : Full Query Syntax](./full-query-syntax)
- [Client API: Session : Querying : How to use Lucene in queries](../../client-api/session/querying/lucene/how-to-use-lucene-in-queries)
- [Server: Configuration : Configuration Options](../../server/configuration/configuration-options)
