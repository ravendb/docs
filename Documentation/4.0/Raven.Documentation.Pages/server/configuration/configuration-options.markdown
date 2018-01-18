# Server Configuration
RavenDB is **Safe by Default** which means its set of options are configured for best safety.  However, these options can be manually configured in order to accommodate different server behavior.

{PANEL:Environment Variables}

Configuration can be adjusted by preceding configuration keys with `RAVEN_` or `RAVEN.` prefix. 

### Example

{CODE-BLOCK:plain}
RAVEN_Setup.Mode=None
{CODE-BLOCK/}

{PANEL/}

{PANEL:JSON}

File `settings.json` which can be found in the same directory as the server executable can also be used to change the configuration of the server. The file is read and applied on server startup only.

### Example

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None"
}
{CODE-BLOCK/}

{NOTE Changes in `settings.json` overrides the environment variables settings. /}

{PANEL/}

{PANEL:Command Line Arguments}

The server can be configured using the list of arguments that can be passed to the console application (or while running as a deamon).

### Example:

{CODE-BLOCK:bash}
./Raven.Server --Setup.Mode=None
{CODE-BLOCK/}

{NOTE Executable arguments overrides environment variables settings and `settings.json` ones. /}

{PANEL/}
