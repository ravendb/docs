# Getting Started: Writing your Unit Test using TestDriver

In this section we will explain how to use [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) in order to write unit tests for working with RavenDB.

- [RavenServerLocator](../start/test-driver#ravenserverlocator)
- [RavenTestDriver](../start/test-driver#raventestdriver)
- [Pre-initializing the store](../start/test-driver#preinitialize)
- [Unit test](../start/test-driver#unittest)
- [Complete example](../start/test-driver#complete-example)

{PANEL:RavenServerLocator}

The first thing we need to implement is a class derived from `RavenServerLocator`

{CODE:java RavenServerLocator@Start\RavenDBTestDriver.java /}

### Example

{CODE:java test_driver_5@Start\RavenDBTestDriver.java /}

{PANEL/}

{PANEL:RavenTestDriver}

Now that we learned how to implement a server locator we can define a class that derives from Raven's TestDriver.
Lets start with reviewing the TestDriver's methods and properties and later we will get into implementation (complete code sample of a RavenTestDriver can be found at the [bottom](../start/test-driver##complete-example) of the page).

### Properties and Methods
| Signature | Description |
| ----------| ----- |
| **public IDocumentStore getDocumentStore()** | Gets you an IDocumentStore instance with random database name |
| **public IDocumentStore getDocumentStore(String database)** | Gets you an IDocumentStore instance for the requested database. |
| **protected virtual void customizeStore(DocumentStore documentStore)** |Allows you to pre-initialize the IDocumentStore. |
| **public void waitForIndexing(IDocumentStore store)** | Allows you to wait for indexes to become non-stale. |
| **public void waitForIndexing(IDocumentStore store, String database)** | Allows you to wait for indexes to become non-stale. |
| **public void waitForIndexing(IDocumentStore store, String database, Duration timeout)** | Allows you to wait for indexes to become non-stale. |
| **public void waitForUserToContinueTheTest(IDocumentStore store)** | Allows you to break the test and launch the Studio to examine the state of the database. |
| **public void close()** | Allows you to dispose of the server. |

{PANEL/}

{PANEL:PreInitialize}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.

### Example

{CODE:java test_driver_3@Start\RavenDBTestDriver.java /}

{PANEL/}

{PANEL:UnitTest}
Finally we can write down a simple test, note that I'm using JUnit for my test framework in the below example.
Also note that the test itself is meant to show diffrent capabilities of the test driver and is not meant to be the most efficient.
The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver##complete-example)

### Example

{CODE:java test_driver_4@Start\RavenDBTestDriver.java /}

In the test we get an IDocumentStore to our test database, deploy an index and insert two documents into it. 
We then wait for the indexing to complete and launch the Studio so we can verify the documents and index are deployed (we can remove this line once the test is working).
At the end of the test we query for TestDocument where their name contains the world 'hello' and assert that we have only one such document.

{PANEL/}

{PANEL:Complete Example}

{CODE:java test_driver_1@Start\RavenDBTestDriverFull.java /}

{PANEL/}

## Related articles

### Troubleshooting

- [Sending Support Ticket](../server/troubleshooting/sending-support-ticket)
