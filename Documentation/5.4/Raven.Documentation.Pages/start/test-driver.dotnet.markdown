# Getting Started: Writing your Unit Test using TestDriver
---

{NOTE: }

* In this section, we explain how to use [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) to write RavenDB unit tests.  

* TestDriver uses an [Embedded Server](../server/embedded) package with the same set of [prerequisite](../server/embedded#prerequisite) as embedded servers to run the tests.  

* In this page: 
 - [RavenTestDriver](../start/test-driver#raventestdriver)
 - [Pre-initializing the store](../start/test-driver#preinitialize)
 - [ConfigureServer](../start/test-driver#configureserver)
 - [Unit test](../start/test-driver#unittest)
 - [Complete example](../start/test-driver#complete-example)
 - [CI Servers](../start/test-driver#continuous-integration-servers)

{NOTE/}

{PANEL:RavenTestDriver}

Start by creating a class that derives from `RavenTestDriver`.  
Find below a list of test driver methods, followed by [samples](../start/test-driver#complete-example).  

### TestDriver Methods

#### `DatabaseDumpFilePath`
Override the path to the database dump file that is loaded when calling ImportDatabase.  
{CODE-BLOCK:csharp} 
protected virtual string DatabaseDumpFilePath => null;
{CODE-BLOCK/}  

#### `DatabaseDumpFileStream`
Allow overriding the stream containing the database dump loaded when calling ImportDatabase.  
{CODE-BLOCK:csharp} 
protected virtual Stream DatabaseDumpFileStream => null;
{CODE-BLOCK/}  

#### `GetDocumentStore`
Get an IDocumentStore instance for the requested database.  
{CODE-BLOCK:csharp} 
public IDocumentStore GetDocumentStore([CallerMemberName] string database = null, 
                                                TimeSpan? waitForIndexingTimeout = null)
{CODE-BLOCK/}  
  
#### `PreInitialize`
Pre-initialize IDocumentStore.  
{CODE-BLOCK:csharp} 
protected virtual void PreInitialize(IDocumentStore documentStore)
{CODE-BLOCK/}  

#### `PreConfigureDatabase`
Pre configure the database record before creating it.  
{CODE-BLOCK:csharp} 
protected virtual void PreConfigureDatabase(DatabaseRecord databaseRecord)
{CODE-BLOCK/}  

#### `SetupDatabase`
Initialize the database  
{CODE-BLOCK:csharp} 
protected virtual void SetupDatabase(IDocumentStore documentStore)
{CODE-BLOCK/}  

#### `DriverDisposed`  
An event raised when the test driver is disposed of  
{CODE-BLOCK:csharp} 
protected event EventHandler DriverDisposed;
{CODE-BLOCK/}  

#### `ConfigureServer`  
Configure the server before running it
{CODE-BLOCK:csharp} 
public static void ConfigureServer(TestServerOptions options)
{CODE-BLOCK/}  

#### `WaitForIndexing`
Wait for indexes to become non-stale  
{CODE-BLOCK:csharp} 
public void WaitForIndexing(IDocumentStore store, string database = null, 
                                                    TimeSpan? timeout = null)
{CODE-BLOCK/}  

#### `WaitForUserToContinueTheTest`  
Pause the test and launch Studio to examine database state  
{CODE-BLOCK:csharp} 
public void WaitForUserToContinueTheTest(IDocumentStore store)
{CODE-BLOCK/}  

#### `OpenBrowser`  
Open browser  
{CODE-BLOCK:csharp} 
protected virtual void OpenBrowser(string url)
{CODE-BLOCK/}  

#### `Dispose`  
Dispose of the server  
{CODE-BLOCK:csharp} 
public virtual void Dispose()
{CODE-BLOCK/}  

{PANEL/}

{PANEL:PreInitialize}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.

### Example

{CODE test_driver_PreInitialize@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:UnitTest}

We use [xunit](https://www.nuget.org/packages/xunit/) for the test framework in the below example.  

{NOTE: }
Note that the test itself is meant to show different capabilities of the test driver and is not meant to be the most efficient.  
{NOTE/}

The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver#complete-example)

### Example

In the example, we get an `IDocumentStore` object to our test database, deploy an index, and insert two documents into the document store.  

We then use `WaitForUserToContinueTheTest(store)` which launches the Studio so we can verify that the documents 
and index are deployed (we can remove this line after the test succeeds).  

Finally, we use `session.Query` to query for "TestDocument" where the name contains the word 'hello', 
and we assert that we have only one such document.

{CODE test_driver_MyFirstTest@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL: ConfigureServer}

The `ConfigureServer` method allows you to be more in control of your server.  
You can use it with `TestServerOptions` to change the path to the Raven server binaries, specify data storage path, adjust .NET framework versions, etc.

* `ConfigureServer` can only be set once per test run.  
  It needs to be set before `DocumentStore` is called.  
  See an [example](../start/test-driver#complete-example) below.  

* If it is called twice, or within the `DocumentStore` scope, you will get the following error message:
  `System.InvalidOperationException : Cannot configure server after it was started. Please call 'ConfigureServer' method before any 'GetDocumentStore' is called.`  

{INFO:TestServerOptions}

Defining TestServerOptions allows you to be more in control of 
how the embedded server is going to run with just a minor [definition change](../start/test-driver#example-2).

* To see the complete list of `TestServerOptions`, which inherits from embedded servers, go to embedded [ServerOptions](../server/Embedded#getting-started).  
* It's important to be sure that the correct [.NET FrameworkVersion](../server/Embedded#net-frameworkversion) is set.

{INFO /}

#### Example

{CODE test_driver_ConfigureServer@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:Complete Example}

This is a full unit test using [Xunit](https://www.nuget.org/packages/xunit/).

In the test, we get an `IDocumentStore` object to our test database, deploy an index, and insert two documents into the document store.  

We then use `WaitForUserToContinueTheTest(store)` which launches the Studio so we can verify that the documents 
and index are deployed (we can remove this line after the test succeeds).  

Finally, we use `session.Query` to query for "TestDocument" where the name contains the word 'hello', 
and we assert that we have only one such document.

{CODE test_full_example@Start\RavenDBTestDriverFull.cs /}

{PANEL/}


{PANEL:Continuous Integration Servers}

Best practice is to use a CI/CD server to help automate the testing and deployment of your new code. 
Popular CI/CD products are [AppVeyor](https://www.appveyor.com/) or [Visual Studio Team Services (aka. VSTS)](https://visualstudio.microsoft.com/team-services/).

{PANEL/}

## Related Articles

- [Running an Embedded Instance](../server/Embedded)

### Troubleshooting

- [Collect information for support](../server/troubleshooting/collect-info)
