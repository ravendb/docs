# Configuration Options

RavenDB is **Safe by Default** which means its set of options are configured for the best safety.  
However, these options can be manually configured in order to accommodate different server behavior.

{PANEL:Environment Variables}

Configuration can be adjusted by preceding configuration keys with `RAVEN_` or `RAVEN.` prefix. All `.` in configuration keys can be substituted with `_` if needed so from Server perspective `RAVEN_Setup.Mode` and `RAVEN_Setup_Mode` are equivalent.

### Example

{CODE-BLOCK:plain}
RAVEN_Setup.Mode=None
{CODE-BLOCK/}

{PANEL/}

{PANEL:JSON}

The `settings.json` file which can be found in the same directory as the server executable can also be used to change the configuration of the server. 
The file is read and applied on the server startup only. It is created when running the server for the first time from the `settings.default.json` file.

### Example

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None"
}
{CODE-BLOCK/}

{NOTE Changes in `settings.json` override the environment variables settings. /}

{INFO:JSON Arrays}

All configuration options that support multiple values (for example strings separated by `;`) can be configured via regular JSON array e.g. [`Security.WellKnownCertificates.Admin`](../../server/configuration/security-configuration#security.wellknowncertificates.admin)

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None",
    "Security.WellKnownCertificates.Admin" : [ "297430d6d2ce259772e4eccf97863a4dfe6b048c", "e6a3b45b062d509b3382282d196efe97d5956ccb" ]
}
{CODE-BLOCK/}

{INFO/}

{PANEL/}

{PANEL:Command Line Arguments}

The server can be configured using command line arguments that can be passed to the console application (or while running as a deamon).

### Example:

{CODE-BLOCK:bash}
./Raven.Server --Setup.Mode=None
{CODE-BLOCK/}

{NOTE These command line arguments override the settings of environment variables and the `settings.json`. More details about Command Line Arguments can be found [here](../../server/configuration/command-line-arguments). /}

{PANEL/}

## Related articles

### Configuration

- [Command Line Arguments](../../server/configuration/command-line-arguments)

### Administration

- [Command Line Interface (CLI)](../../server/administration/cli)
