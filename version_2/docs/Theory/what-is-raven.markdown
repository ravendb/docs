# What is Raven?
Raven is a .NET Linq enabled Document Database, focused on providing high performance, schema-less, flexible and scalable NoSQL data store for the .NET and Windows platforms.

## What does this mean?
Unlike a traditional RDMBS, Raven doesn't store data in rows and columns, instead allowing you to store any JSON document inside the database. Raven is schema-less, which means that you don't have to predefine how the documents that are stored in the database will be formed. Because the data is not limited to simply columns and rows, you can store arbitrarily complex data such as arrays, dictionaries and trees with ease.

On top of the documents, you can define indexes (using C#'s Linq syntax) that the database will maintain for you. This gives you the best of both worlds, you get a schema free nature (no upfront agonizing about the database schema or trying to shoehorn data into a relational model) but you are still able to perform efficient queries.

##Expected Usage and Sweet Spots
The schema-less nature makes it ideal to store dynamic data, such as CMS and CRM entities, which the end user can usually customize as necessary or semi structure data (provided by human). On top of that, Raven offers a powerful indexing mechanism, allowing you to define a Linq query as an index. Raven will take that query and execute it in the background, the result of that Linq query is immediately available as a view that you can query upon.
We see Raven as appropriate for:

* Web Related Data, such as user sessions, shopping cart, etc. - Raven's document based nature means that you can retrieve and store all the data required to process a request in a single remote call.
* Dynamic Entities, such as user-customizable entities, entities with a large number of optional fields, etc. - Raven's schema free nature means that you don't have to fight a relational model to implement it.
* Persisted View Models - Instead of recreating the view model from scratch on every request, you can store it in its final form in Raven. That leads to reduced computation, reduced number of remote calls and improved overall performance.
* Large Data Sets - The underlying storage mechanism for Raven is known to scale in excess of 1 terabyte (on a single machine) and the non relational nature of the database makes it trivial to shard the database across multiple machines, something that Raven can do natively.

##Documents
A document within Raven is just a standard JSON object, such as this one:

{code} //TODO: add code

Each document in Raven is stored using a key (in RDBMS terms, the primary key), which is also called the document id. In the case of the document above, its key is: "posts/2321816"

Raven can store any document, requiring no schema. 

##Metadata
In addition to the data in the document itself, Raven provides a way to store additional metadata about a document. The metadata information is a set of key/value information that is associated with the document.
That information can be as simple as who was the user who last updated the document or a list of endpoints to notify when that document is updated.

##Attachments
In addition to just storing documents, Raven also provides a solution for storing binary data. While documents must be valid JSON documents, attachments are treated as binary data which are just stored and retrieved from the database. Using attachments is a much more efficient form of storing binary data as opposed to encoding the same as a JSON document.

##Indexes
Raven uses indexes as a way to provide order in a schema free world. An index is a Linq query that operates over a set of documents, producing a projection out of each document that can be efficiently queried. An index is essentially a Linq query that Raven executes in the background, and whose results are stored in persistent storage. Those results can be efficiently queried at a later date.
An index looks like the following:

{code} //TODO: add code

This index will index the answered posts by Title, giving us a full text search capability on that. Since indexes are maintained in the background, querying an index is a very cheap operation. Finding out all the posts with an answer about 'NoSQL Scaling' is easy:
http://ravendb/indexes/AnsweredPostsByTitle?query=Title:NoSQL%20Scaling

You can learn more about indexes in [the index documentation]().//TODO: link to indexes

**Index updates**

Raven's approach to indexing is quite different to the one you might be used to with RDBMS. Unlike RDBMS, where defining indexes will slow down insert performance, Raven allows indexes to become stale, in order to increase overall CRUD performance. A stale index will be explicitly marked as such, but the truth is that often enough, you can tolerate showing slightly stale data to the user while the index is being rebuilt on the background. The end result from this decision is that indexes do not hinder the performance of the database.