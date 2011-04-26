# Consumer usage

So far we have spoken in abstracts, about NoSQL in general and RavenDB in particular, but in this chapter, we leave the high level concepts aside and concentrate on actually using RavenDB. We will go through all the steps required to perform basic CRUD operations using RavenDB, familizaring ourselves with RavenDB APIs, concepts and workings.

This chapter assumes usage of the RavenDB .NET Client API, and will provide examples of the underlying HTTP calls behind made for each call.

In this part:

* Basic RavenDB concepts explained
* Connecting to a document store
* Creating and modifying documents
* Loading documents
* Querying for documents 
* Using System.Transactions
* Performing more advanced operations like using Includes, Attachments and Patching
* Tracing and debugging client-server communication
* More in-depth look of the RavenDB client API