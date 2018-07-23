# Glossary : ServerOptions

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **FrameworkVersion** | string | The framework version to run the server |
| **DataDirectory** | string | Where to store our database files |
| **DotNetPath** | string | The path to exec dotnet (If dotnet is in PATH leave it)|
| **AcceptEula** |  bool | If set to false, will ask to accept our terms |
| **ServerUrl** | string | What address we want to start our server (default 127.0.0.1:0) |
| **MaxServerStartupTimeDuration** | TimeSpan | The timeout for the server to start |
| **CommandLineArgs** | List&lt;string&gt; | The command lines arguments to start the server with |
	
