# Installation: Manual Setup

{NOTE: Automatic Certificate Renewals Via Setup Wizard}

RavenDB has developed a quick and simple process to set up a fully secure cluster with our Setup Wizard.  
We've developed automatic renewals of certificates when setting up with the Setup Wizard together with Let's Encrypt.  

If you choose manual setup and/or to provide your own certificate, **you are responsible for their periodic renewal**.

{NOTE/}

In this page: 

* [Downloading Server and Setting Up Node Folders](../../start/installation/manual#downloading-server-and-setting-up-node-folders)
* [Disabling 'Setup Wizard'](../../start/installation/manual#disabling-setup-wizard)  
* [Server Url and Port](../../start/installation/manual#server-url-and-port)
* [Security](../../start/installation/manual#security)
* [Configuration](../../start/installation/manual#configuration)

## Downloading Server and Setting Up Node Folders
Download the [RavenDB server package](https://ravendb.net/download) and extract it into permanent server folders on each machine.  
We recommend using multiple machines to improve [cluster availability via failover](../../client-api/cluster/how-client-integrates-with-replication-and-cluster) in case one goes down.  
Each folder that contains an extracted server package will become a functional node in your cluster.  

By default, when Server is started using `run.ps1` (or `run.sh`) script. It will open a browser with a [Setup Wizard](../../start/installation/setup-wizard) which will guide you through the Server configuration process.

The setup is started because the default configuration file [settings.json](../../server/configuration/configuration-options#json) (found in the downloaded RavenDB Server package) comes configured like this:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the Server will run:

- On `localhost` with a `random port`
- In `Setup Wizard` mode
- Store the data in `RavenData` directory

## Disabling 'Setup Wizard'

To disable the 'Setup Wizard' please change the `Setup.Mode` in the settings.json configuration to `None` or remove it completely.

## Server Url and Port

Setting the `ServerUrl` to `http://127.0.0.1:0` will bind the server to a `localhost` with a `random port`. For manual setup we suggest changing the port to a non-random value - e.g. **8080**.

{NOTE:Port in Use}

In some cases the port might be in use, this will prevent the Server from starting with "address in use" error (`EADDRINUSE`).  
For a list of IPs and ports already in use on your machine, run `netstat -a` in the command line.  

{NOTE/}

## Security

{WARNING: Protect Your Cluster From The Start}

We highly recommend securing your server from the start to [prevent potential vulnerabilities](https://ravendb.net/articles/ravendb-secure-by-default-document-database) later.  
 RavenDB makes securing your cluster from the start as easy as possible to prevent the possiblity of forgetting to secure before going into production.

{WARNING/}

Read the [Manual Certificate Configuration](../../server/security/authentication/certificate-configuration) section to learn how to setup security manually.

## Configuration

Read the [Configuration Section](../../server/configuration/configuration-options) to learn more about using [settings.json](../../server/configuration/configuration-options#json) and see a list of configuration options.

## Related articles

### Installation

- [Common Setup Wizard Errors and FAQ](../../server/security/common-errors-and-faq#setup-wizard-issues)
- [Setup Wizard](../../start/installation/setup-wizard)

### Security

- [Security in RavenDB](../../server/security/overview)
