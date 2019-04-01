# Getting Started: Writing your Unit Test using TestDriver

In this section we will explain how to use [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) in order to write unit tests for working with RavenDB.

- [RavenServerLocator](../start/test-driver#ravenserverlocator)
- [RavenTestDriver](../start/test-driver#raventestdriver)
- [Pre-initializing the store](../start/test-driver#preinitialize)
- [Unit test](../start/test-driver#unittest)
- [Complete example](../start/test-driver#complete-example)
- [CI Servers](../start/test-driver#continuous-integration-servers)

{PANEL:RavenServerLocator}

The first thing we need to implement is a class derived from `RavenServerLocator`

{CODE RavenServerLocator@Start\RavenDBTestDriver.cs /}

### Example

{CODE test_driver_5@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:RavenTestDriver}

Now that we learned how to implement a server locator we can define a class that derives from Raven's TestDriver.
Lets start with reviewing the TestDriver's methods and properties and later we will get into implementation (complete code sample of a RavenTestDriver can be found at the [bottom](../start/test-driver##complete-example) of the page).

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
| **public void WaitForUserToContinueTheTest(IDocumentStore store)** | Allows you to break the test and launch the Studio to examine the state of the database. |
| **protected virtual void OpenBrowser(string url)** | Allows you to open the browser. |
| **public virtual void Dispose()** | Allows you to dispose of the server. |

{PANEL/}

{PANEL:PreInitialize}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.

### Example

{CODE test_driver_3@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:UnitTest}
Finally we can write down a simple test, note that I'm using [xunit](https://www.nuget.org/packages/xunit/) for my test framework in the below example.
Also note that the test itself is meant to show diffrent capabilities of the test driver and is not meant to be the most efficient.
The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver##complete-example)

### Example

{CODE test_driver_4@Start\RavenDBTestDriver.cs /}

In the test we get an IDocumentStore to our test database, deploy an index and insert two documents into it. 
We then wait for the indexing to complete and launch the Studio so we can verify the documents and index are deployed (we can remove this line once the test is working).
At the end of the test we query for TestDocument where their name contains the world 'hello' and assert that we have only one such document.

{PANEL/}

{PANEL:Complete Example}

{CODE test_driver_1@Start\RavenDBTestDriverFull.cs /}

{PANEL/}


{PANEL:Continuous Integration Servers}

Best practice is to use a CI/CD server to help automate the testing and deployment of your new code. 
Popular CI/CD products are [AppVeyor](https://www.appveyor.com/) or [Visual Studio Team Services (aka. VSTS)](https://visualstudio.microsoft.com/team-services/). Some customization is required for any
CI/CD product you use, because you will need to manually download the RavenDb Server _before_ any tests are kicked off. Remember, the Test Driver
requires a `path location` for a `Raven.Server.exe` or `Raven.Server.dll` to be located, where the path on your CI/CD server 
will most likely be different to the path on your localhost-development machine.

### AppVeyor settings

1. Create some environment variables and powershell script to download RavenDB Server and unzip it.
2. Make sure your custom test-driver knows to check/look for those environment variables you've just set.
3. Queue/build away!

#### Step 1 - Create environment variables and powershell script.

Here's some simple, sample appveyor.yml which set the environmental variables, downloads, unzips, restores, builds and then tests.

{CODE-BLOCK:powershell}

version: '{build}.0.0-dev'
configuration: Release
os: Visual Studio 2017
pull_requests:
  do_not_increment_build_number: true
environment:
  RavenServerDirectory: '%temp%\raven-server'
  RavenServerDownloadDestinationFile: '%temp%\raven-server.zip'
  RavenServerTestPath: '%RavenServerDirectory%\server\Raven.Server.dll'

init:
  - ps: |
      iex ((new-object net.webclient).DownloadString('https://gist.githubusercontent.com/PureKrome/0f79e25693d574807939/raw/f5b40256fc2ca77d49f1c7773d28406152544c1e/appveyor-build-info.ps'))
      
      Write-Output "Lets see what all our Environmental variables are now defined as:"
      Get-ChildItem Env:

      Write-Output "Downloading RavenDb 4.0.7 ..."
      (new-object net.webclient).DownloadFile('https://daily-builds.s3.amazonaws.com/RavenDB-4.0.7-windows-x64.zip', $env:RavenServerDownloadDestinationFile)

      Write-Output "Unzipping RavenDb from $env:RavenServerDownloadDestinationFile to $env:RavenServerDirectory"
      expand-archive -Path $env:RavenServerDownloadDestinationFile -DestinationPath $env:RavenServerDirectory

before_build:
  # Display .NET Core version
  - cmd: dotnet --info
  # Display minimal restore text
  - cmd: dotnet restore --verbosity quiet

build_script:
  - dotnet build -c %CONFIGURATION% -v minimal /p:Version=%APPVEYOR_BUILD_VERSION% --no-restore

test_script:
  - dotnet test -c %CONFIGURATION% -v minimal --no-restore --no-build

cache:
  - packages -> **\packages.config
{CODE-BLOCK/}

#### Step 2 - Check/update your custom test-driver code

Here's some sample code which the test-driver checks for environmental variables.

{CODE-BLOCK:powershell}
var path = Environment.GetEnvironmentVariable("RavenServerTestPath");
if (!string.IsNullOrWhiteSpace(path))
{
	if (InitializeFromPath(path))
	{
		return _serverPath;
	}
}
{CODE-BLOCK/}

#### Step 3 - Queue/Build away!
Now queue up a new build to push up a commit and this should kick off where RavenDb-Server downloads, unzips and the 
test-driver references that downloaded server, in your tests.


### VSTS Settings

1. Create some environment variables for the entire build definition.
2. Make sure your custom test-driver knows to check/look for those environment variables you've just set.
3. Add a custom powershell task to manually download and unzip the RavenDB distribution package.
4. Queue/build away!

####Step 1 - Global Environment Variables for the build definition.
![VSTS Global Environment Variables](images/test-driver/td1.png)

####Step 2 - Check/update your custom test-driver code

Here's some sample code which the test-driver checks for environmental variables.

{CODE-BLOCK:powershell}
var path = Environment.GetEnvironmentVariable("RavenServerTestPath");
if (!string.IsNullOrWhiteSpace(path))
{
	if (InitializeFromPath(path))
	{
		return _serverPath;
	}
}
{CODE-BLOCK/}

####Step 3 - Add a custom powershell task
![VSTS Add Custom Powershell Task](images/test-driver/td2.png)

here's the code to quickly copy/paste the script into your VSTS task settings:

{CODE-BLOCK:powershell}
Write-Output "Lets see what all our Environmental variables are now defined as:"
Get-ChildItem Env:

Write-Output "Downloading RavenDb 4.0.7 ..."
(new-object net.webclient).DownloadFile('https://daily-builds.s3.amazonaws.com/RavenDB-4.0.7-windows-x64.zip',  $env:RavenServerDownloadDestinationFile)

Write-Output "Unzipping RavenDb from" + $env:RavenServerDownloadDestinationFile + " to " + $env:RavenServerDirectory
expand-archive -Path $env:RavenServerDownloadDestinationFile -DestinationPath  $env:RavenServerDirectory
{CODE-BLOCK/}

####Step 4 - Queue/Build away!
Now queue up a new build to push up a commit and this should kick off where RavenDb-Server downloads, unzips and the 
test-driver references that downloaded server, in your tests.


{PANEL/}

## Related articles

### Troubleshooting

- [Sending Support Ticket](../server/troubleshooting/sending-support-ticket)
