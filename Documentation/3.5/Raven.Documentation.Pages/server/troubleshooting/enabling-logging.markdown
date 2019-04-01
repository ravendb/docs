# Troubleshooting: Enabling logging

RavenDB has extensive support for logging, enabling you to figure out exactly what is going on in the server.   
By default, **logging is turned off**, but it can be easily enabled by creating a file called `NLog.config` in the base directory of a server (remember to restart your server).

## Sample log file

{CODE enabling_logging_1@Server\Troubleshooting\EnablingLogging.cs /}

## Remarks

{INFO To make it even easier for you, we have included `NLog.Ignored.config` file which just needs to be renamed in the Server directory, inside the distribution package. /}
