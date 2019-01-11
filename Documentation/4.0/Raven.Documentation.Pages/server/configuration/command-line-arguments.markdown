# Configuration : Command Line Arguments

Running RavenDB using command line options provides the ability to setup both its behavior (e.g. running as daemon or service) and its configuration options.

## Arguments

| Argument        |      Details                         |  Example |
|------------------------|:-------------------------------------|:--------------------------|
| `-h` \| `-?` \| `--help`     | Print command line arguments list    | `Raven.Server -h`        |
| `-v` \| `--version`        | Displays version and exits           | `Raven.Server -v`       |
| `--print-id`             | Prints server ID upon server start   | `Raven.Server --print-id` |
| `-n` \| `--non-interactive` | Run in non-interactive mode. After RavenDB finishes initialization and starts up, no CLI prompt will be displayed. This is useful when running as service. CLI management is still fully available through the use of `rvn admin-channel`. Do note it is possible to enter non-interactive mode automatically if prompt is not available due to OS limitations, but still `rvn` use is available. More information about 'Running as a Service' can be found [here](../../start/installation/running-as-service). | `Raven.Server -n` |
| `--service-name=<service name>` | Set service name. Only applies to RavenDB running on Windows OS as Service | `Raven.Server --service-name=RavenDbService` |
| `-c=<path>` \| `--config-path=<path>` | Sets custom configuration file path. Sets the `setting.json` file to be used by RavenDB | `Raven.Server -c=/home/myuser/settings.local.json` |
| `--browser` | Attempts to open RavenDB Studio in the browser | `Raven.Server --browser` |
| `-l` \| `--log-to-console` | Print logs to console (when run in non-interactive mode) | `Raven.Server -l` |

## Docker

{INFO}

If you are interested in hosting the server in a Docker container, please 
read our dedicated article.

{INFO/}

Running a Docker instance using `-e` Docker's argument can help you pass few configuration options to RavenDB, e.g. :

{CODE-BLOCK:bash}
docker run --name docker_nightly -e PublicServerUrl=http://10.0.75.2:8080 -e UNSECURED_ACCESS_ALLOWED=PublicNetwork -p 8081:8080 -p 38889:38888 ravendb/ravendb-nightly
{CODE-BLOCK/}

The environment variables available when running Docker are:

* BindPort (8080)
* BindTcpPort (38888)
* ConfigPath
* DataDir
* PublicServerUrl
* PublicTcpServerUrl
* LogsMode
* CertificatePath
* CertificatePassword
* Hostname

## Related articles

### Configuration

- [Configuration Options](../../server/configuration/configuration-options)

### Administration

- [Command Line Interface (CLI)](../../server/administration/cli)
