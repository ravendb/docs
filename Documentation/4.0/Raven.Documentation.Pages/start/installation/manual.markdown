# Installation: Manual Setup

By default, when Server is started using `run.ps1` (or `run.sh`) script. It will open a browser with a [Setup Wizard](../../start/installation/setup-wizard) which will guide you through the Server configuration process.

The setup is started because the default configuration file [settings.json](../../server/configuration/configuration-options#json) used in the RavenDB Server package looks like this:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the Server will run:

- In `Setup Wizard` mode
- On `localhost` with a `random port`
- Store the data in `RavenData` directory.

## Disabling 'Setup Wizard'

To disable the 'Setup Wizard' please change the `Setup.Mode` configuration to `None` or remove it completely.

## Server Url and Port

Setting the `ServerUrl` to `http://127.0.0.1:0` will bind the server to a `localhost` with a `random port`. For manual setup we suggest changing the port to a non-random value e.g. **8080**.

{WARNING:Port in Use}

In some cases the port might be in use, this will prevent the Server from starting with "address in use" error (`EADDRINUSE`).

{WARNING/}

## Security

Read the [Manual Certificate Configuration](../../server/security/authentication/certificate-configuration) section to learn how to setup security manually.

## Configuration

Read the [Configuration Section](../../server/configuration/configuration-options) to learn more about using [settings.json](../../server/configuration/configuration-options#json) and see a list of configuration options.

## Related articles

### Installation

- [Common Setup Wizard Errors and FAQ](../../server/security/common-errors-and-faq#setup-wizard-issues)
- [Setup Wizard](../../start/installation/setup-wizard)

### Security

- [Security in RavenDB](../../server/security/overview)
