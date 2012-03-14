#Building Raven from Source

* Raven requires .NET 4.0 SDK install to build correctly.
* * You should be able to just open Raven in Visual Studio 2010 and start working with it immediately.
* Raven uses power shell to execute its build process. From the power shell prompt, execute: .\psake.ps1
* You may need to allow script execution in your power shell configuration:
* Set-ExecutionPolicy unrestricted
* The build process will, by default, execute all the tests, which may take a while. You may skip the tests by executing:  
        .\psake.ps1 default -task ReleaseNoTests
* Raven uses the xUnit unit testing framework.