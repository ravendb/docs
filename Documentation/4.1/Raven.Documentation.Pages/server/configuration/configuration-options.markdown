# Configuration Options

---
{NOTE: }
RavenDB is **Safe by Default**, which means its default settings are configured for best safety.  
You can change these settings, however, to divert from the default behavior.  

* In this Page:  
   * [Environment Variables](../../server/configuration/configuration-options#environment-variables)  
   * [settings.json](../../server/configuration/configuration-options#settings.json)  
   * [Command Line Arguments](../../server/configuration/configuration-options#command-line-arguments)  

{NOTE/}

{PANEL:Environment Variables}

RavenDB's configuration can be adjusted using environment variables.  
The server will retrieve all the environment variables that start with `RAVEN_` 
and apply their values to the specified configuration keys.  

{NOTE: }
All the period (`.`) characters in configuration keys must be replaced with an 
underscore character (`_`) when used in environment variables.  
{NOTE/}

### Example

{CODE-BLOCK:plain}
RAVEN_Setup_Mode=None
RAVEN_DataDir=RavenData
RAVEN_Certificate_Path=/config/raven-server.certificate.pfx
{CODE-BLOCK/}

{PANEL/}

{PANEL:`settings.json`}

Use the `settings.json` file to change the server configuration.  

The file is created when running the server for the first time, duplicating the `settings.default.json` file.  
Find it at the same directory as the server executable.  

The file is read and applied only on server startup.  

### Example

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None"
}
{CODE-BLOCK/}

{NOTE: }
`settings.json` configuration options **override** [environment variables](../../server/configuration/configuration-options#environment-variables) settings. 
{NOTE/}

{INFO:JSON Arrays}

Configuration options that include multiple values (like strings separated by `;`) 
can be configured using regular JSON arrays.  
To set [`Security.WellKnownCertificates.Admin`](../../server/configuration/security-configuration#security.wellknowncertificates.admin), 
for example, use -  

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

{NOTE: }
Command line arguments **override** [environment variables](../../server/configuration/configuration-options#environment-variables) 
and [settings.json](../../server/configuration/configuration-options#settings.json) settings.  
Find additional details about Command Line Arguments [here](../../server/configuration/command-line-arguments). 
{NOTE/}

{PANEL/}

## Related articles

### Configuration

- [Command Line Arguments](../../server/configuration/command-line-arguments)

### Administration

- [Command Line Interface (CLI)](../../server/administration/cli)
