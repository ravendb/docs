# Enabling debug logging for Raven

Raven has extensive support for debug logging, enabling you to figure out exactly what is going on in the server. By default, logging is turned off but you can enable it at any time by creating a file called "NLog.config" in Raven's base directory with the following content:

{CODE enabling_logging_1@Server\Deployment\EnablingLogging.cs /}
