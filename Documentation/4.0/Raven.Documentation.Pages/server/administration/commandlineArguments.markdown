Command Line Arguments
======================
Running RavenDB using command line options provides the ability to setup both its behavior (i.e. running as daemon or service) and its configuration options.

<br />

### Arguments
-----------

| Argument Option        |      Details                         |  Example |
|------------------------|:-------------------------------------|:--------------------------|
| -h \| -? \| --help     | Print command line arguments list    | _Raven.Server -h_         |
| -v \| --version        | Displays version and exits           | _Raven.Server -v_         |
| --print-id             | Prints server ID upon server start   | _Raven.Server --print-id_ |
| -n \| --non-interactive | Run in non-interactive mode. After RavenDB finishes initialization and starts up, no CLI prompt will be displayed. This is useful when running as service. CLI management is still fully available through the use of `rvn admin-channel`. Do note it is possible to enter non-interactive mode automatically if prompt is not available due to OS limitations, but still `rvn` use - is available! See ```Running RavenDB as daemon/service``` | _Raven.Server -n_ |
| --service-name=<service name> | Set service name. Only applies to RavenDB running on Windows OS as Service | _Raven.Server --service-name=RavenDbService_ |
| -c=<path> \| --config-path=<path> | Sets custom configuration file path. Sets the `setting.json` file to be used by RavenDB | _Raven.Server -c=/home/myuser/settings.local.json_ |
| --browser              | Attempts to open RavenDB Studio in the browser | _Raven.Server --browser |
| -l \| --log-to-console | Print logs to console (when run in non-interactive mode) | _Raven.Server -l_ |

### Config Options
-----------
Configuration options can be set either by using environment variable or command line argument - see : [configuration-options](`https://ravendb.net/docs/article-page/4.0/csharp/server/configuration/configuration-options`)

Setting configration optiopn using command line:
* --<config option>=<value>
i.e. : _--ServerUrl=http://10.0.0.10:8080_

### Docker
------------
Running a docker instance using `-e` docker's argument can help pass few configuration options to RavenDB, i.e. :
`docker run --name docker_nightly -e PublicServerUrl=http://10.0.75.2:8080 -e UNSECURED_ACCESS_ALLOWED=PublicNe
twork -p 8081:8080 -p 38889:38888 ravendb/ravendb-nightly`

The possible environment variable available through command line are:
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


