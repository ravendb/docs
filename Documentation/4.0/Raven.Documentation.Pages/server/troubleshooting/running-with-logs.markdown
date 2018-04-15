# Troubleshooting: Running with Logs
#### Debug Levels
RavenDB has 3 debug log modes:
* `None` - no debugging information
* `Operations` - save server's operations into log
* `Information` - save server's operations and any debug log information avaialable

Add log mode to settings.json in order to set from startup:
`"Log.Mode": "< None | Operations | Information>"`

While runing, you may use RavenDB's CLI:

`log [http-]<on|off|information/operations> [no-console]`

##### Example:

`ravendb> log information`

or filter only http requests:

`log http-on`


#### Path to log files

By default logs are saved under `Logs` directory in Raven.Server executable path.  You may specify a specific path in settings.json:
`"Log.Path": "<path>"`

