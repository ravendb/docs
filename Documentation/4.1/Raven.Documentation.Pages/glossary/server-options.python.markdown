# Glossary : ServerOptions

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **framework_version** | str | The framework version to run the server |
| **data_directory** | str | Where to store our database files |
| **server_directory** | str | The path to the server binary files (.dll) |
| **dotnet_path** | str | The path to exec dotnet (If dotnet is in PATH leave it)|
| **accept_eula** |  bool | If set to false, will ask to accept our terms |
| **server_url** | str | What address we want to start our server (default 127.0.0.1:0) |
| **max_server_startup_time_duration** | timedelta | The timeout for the server to start |
| **command_line_args** | list | The command lines arguments to start the server with |
	
