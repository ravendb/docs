# Writing your unit test against Raven.TestDriver
In this section we will explain how to use [Raven.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) in order to write unit tests for working with RavenDB.

- [RavenServerLocator](../start/test-driver#ravenserverlocator)
- [RavenTestDriver](../start/test-driver#raventestdriver)
- [Setup the database](../start/test-driver#setupthedatabase)
- [Pre-initializing the store](../start/test-driver#preinitializingthestore)
- [Unit test](../start/test-driver#unittest)
- [Complete code sample](../start/test-driver#completecodesample)

{PANEL:RavenServerLocator}

The first thing we need to implement is a class derived from RavenServerLocator

### Properties
| Signature | Description |
| ----------| ----- |
| **public abstract string ServerPath { get; }** | Allows to fetch the server path. |
| **public virtual string Command => ServerPath;** | Allows to fetch the command used to invoke the server. |
| **public virtual string CommandArguments => string.Empty;** | Allows to fetch the command arguments.|

### Example
{CODE test_driver_5@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:RavenTestDriver}

Now that we learned how to implement a server locator we can define a class that derives from Raven's TestDriver.
Lets start with reviewing the TestDriver's methods and properties and later we will get into implementation (complete code sample of a RavenTestDriver can be found at the [bottom](../start/test-driver#completecodesample) of the page).

### Properties and Methods
| Signature | Description |
| ----------| ----- |
| **protected virtual string DatabaseDumpFilePath => null;** | Allows you to override the path to the database dump file that will be loaded when calling ImportDatabase. |
| **protected virtual Stream DatabaseDumpFileStream => null;** |  Allows you to override the stream containing the database dump that will be loaded when calling ImportDatabase.  |
| **public static bool Debug { get; set; }** | Indicates if the test driver is running in debug mode or not. |
| **public static Process GlobalServerProcess => globalServerProcess;** |Gives you access to the server's process. |
| **public IDocumentStore GetDocumentStore([CallerMemberName] string database = null, TimeSpan? waitForIndexingTimeout = null)** | Gets you an IDocumentStore instance for the requested database. |
| **protected virtual void PreInitialize(IDocumentStore documentStore)** |Allows you to pre-initialize the IDocumentStore. |
| **protected virtual void SetupDatabase(IDocumentStore documentStore)** | Allows you to initialize the database. |
| **protected event EventHandler DriverDisposed;** |An event that is raised when the test driver is been disposed of. |
| **public void WaitForIndexing(IDocumentStore store, string database = null, TimeSpan? timeout = null)** | Allows you to wait for indexes to become non-stale. |
| **public void WaitForUserToContinueTheTest(IDocumentStore store)** | Allows you to break the test and launch the studio to examine the state of the database. |
| **protected virtual void OpenBrowser(string url)** | Allows you to open the browser. |
| **public virtual void Dispose()** | Allows you to dispose of the server. |

{PANEL/}

{PANEL:SetupTheDatabase}
Setup the database will allow you to customise the creation of your test database.
### Example
{CODE test_driver_2@Start\RavenDBTestDriver.cs /}
{PANEL/}

{PANEL:PreInitializingTheStore}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.
### Example
{CODE test_driver_3@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:UnitTest}
Finally we can write down a simple test, note that I'm using [xunit](https://www.nuget.org/packages/xunit/) for my test framework in the below example.
Also note that the test itself is meant to show diffrent capabilities of the test driver and is not meant to be the most efficient.
The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver#completecodesample)

###Example
{CODE test_driver_4@Start\RavenDBTestDriver.cs /}

In the test we get an IDocumentStore to our test database, deploy an index and insert two documents into it. 
We then wait for the indexing to complete and launch the studio so we can verify the documents and index are deployed (we can remove this line once the test is working).
At the end of the test we query for TestDocument where their name contains the world 'hello' and assert that we have only one such document.

{PANEL/}

{PANEL:CompleteCodeSample}

{CODE test_driver_1@Start\RavenDBTestDriverFull.cs /}

{PANEL/}
