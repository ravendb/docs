# Building RavenDB from source

RavenDB requires .NET 4.0 SDK installed to build. You should be able to just open RavenDB in Visual Studio 2010 and start working with it immediately.

RavenDB uses PowerShell to execute its build process. Open a PowerShell prompt as a user with Administrator privileges, and execute:

    .\psake.ps1
    
You may need to allow script execution in your PowerShell configuration:

    Set-ExecutionPolicy unrestricted

The build process will, by default, execute all tests, which will take a while. You may skip the tests by executing:

    .\psake.ps1 default -task ReleaseNoTests

For all tests to run correctly, ASP.NET MVC 3 and Silverlight 4 Tools have to be installed as well.

RavenDB uses the [xUnit](https://github.com/xunit/xunit) unit testing framework. You will need to unblock all DLLs under `(source)\Tools\xUnit` and install it from `xunit.installer.exe` before running the tests.