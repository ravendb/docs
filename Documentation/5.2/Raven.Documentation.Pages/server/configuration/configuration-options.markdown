# Configuration Overview

---

{NOTE: }

* RavenDB comes with default settings that are configured for best results.  
  If needed, you can customize the default configuration to suit your specific needs.

* Any __configuration key__ can be modified by either of the following options:
    * [Environment variables](../../server/configuration/configuration-options#environment-variables)
    * [settings.json](../../server/configuration/configuration-options#settings.json)
    * [Command line arguments](../../server/configuration/configuration-options#command-line-arguments)
    * [Database settings view](../../server/configuration/configuration-options#database-settings-view) (database scope only)

{NOTE/}

{PANEL: Environment variables}

{NOTE: }

* To set a configuration key as an environment variable:

    * Add the prefix `RAVEN_` to the configuration key name
    * Replace all period characters (`.`) with the underscore character (`_`)

* The server will retrieve these environment variables and apply their values.

{NOTE/}

{NOTE: }

__Example__:

* To set configuration key [Security.Certificate.Path](../../server/configuration/security-configuration#security.certificate.path) from an environment variable,  
  add the environment variable `RAVEN_Security_Certificate_Path`.

{CODE-BLOCK:plain}
// In Windows PowerShell:
$Env:RAVEN_Security_Certificate_Path=/config/raven-server.certificate.pfx

// This will set the path to your .pfx certificate file
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: settings.json}

{INFO: }
_settings.json_ configuration values **override** their matching [environment variables](../../server/configuration/configuration-options#environment-variables) values.
{INFO/}

{NOTE: }

* The `settings.json` file is created by RavenDB when running the server for the first time,  
  duplicating the `settings.default.json` file located in the same directory as the server executable.

* If you want to apply configuration keys to _settings.json_ prior to running the server for the first time,  
  you can manually copy _settings.default.json_ to _settings.json_ and make your changes there.

* The file is read and applied only on server startup.

* To set a configuration key from _settings.json_ simply add the key and its value to the file.

{NOTE/}

{NOTE: }

__Example__:

{CODE-BLOCK:json}
{
"ServerUrl": "http://127.0.0.1:8080",
"Setup.Mode": "None",
"License.Path": "D:\\RavenDB\\Server\\license.json"
}
{CODE-BLOCK/}

__JSON Arrays__

Configuration options that include multiple values (like strings separated by `;`)  
can be configured using regular JSON arrays.  
For example, to set [Security.WellKnownCertificates.Admin](../../server/configuration/security-configuration#security.wellknowncertificates.admin) use:

{CODE-BLOCK:json}
{
"Security.WellKnownCertificates.Admin" : [ "297430d6d2ce259772e4eccf97863a4dfe6b048c", "e6a3b45b062d509b3382282d196efe97d5956ccb" ]
}
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: Command line arguments}

{INFO: }
Command line arguments configuration values **override** their matching  
[environment variables](../../server/configuration/configuration-options#environment-variables) and [settings.json](../../server/configuration/configuration-options#settings.json) values.  
{INFO/}

{NOTE: }

* The server can be configured using command line arguments that are passed to the console application  
  (or while running as a daemon).

* Find additional details about Command Line Arguments [here](../../server/configuration/command-line-arguments).

{NOTE/}

{NOTE: }

__Example__:

{CODE-BLOCK:bash}
./Raven.Server --Setup.Mode=None
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: Database settings view}

{NOTE: }

* When the server is up and running, you can modify configuration keys that are in the __database scope__ from the [Database settings view](../../studio/database/settings/database-settings) in the Studio.

* After modifying a database configuration key from this view, the database must be [reloaded](../../studio/database/settings/database-settings#how-to-reload-the-database) for the change to take effect.

{NOTE/}

{PANEL/}

## Related articles

### Configuration

- [Command Line Arguments](../../server/configuration/command-line-arguments)

### Administration

- [Command Line Interface (CLI)](../../server/administration/cli)
