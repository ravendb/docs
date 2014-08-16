# Troubleshooting : Enabling logging

RavenDB has extensive support for logging, enabling you to figure out exactly what is going on in the server.   
By default, **logging is turned off**, but it can be easily enabled by creating a file called `NLog.config` in base directory of a server (remember to restart your server).

## Sample log file

{CODE enabling_logging_1@Server\Troubleshooting\EnablingLogging.cs /}

## Remarks

{INFO To make it even easier for you, we have included `NLog.Ignored.config` file in Server directory inside distribution package that just need to be renamed. /}

#### Related articles

TODO