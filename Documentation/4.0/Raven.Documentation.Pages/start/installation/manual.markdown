# Installation : Manual Setup

By default, when Server is started using `run.ps1` (or `run.sh`) script. It will open a browser with [Setup Wizard](../../start/installation/setup-wizard) to guide you through the Server configuration process more fluently. 

The setup is started because default configuration file `settings.json` used in RavenDB Server package looks like follows:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the Server will run:

- In `Setup Wizard` mode
- On `localhost` with `random port`
- Store the data in `RavenData` directory.

## Disabling 'Setup Wizard'

To disable the 'Setup Wizard' please change the `Setup.Mode` configuration to `None` or remove it completely.

## Server Url and Port

Setting the `ServerUrl` to `http://127.0.0.1:0` will bind the server to a `localhost` with a `random port`. For manual setup we suggest changing the port to non-random value e.g. **8080**.

{WARNING:Port in Use}

In some cases the port might be in use, this will prevent the Server from starting with "address in use" error (`EADDRINUSE`).

{WARNING/}

## Security

Read the [Certificate Configuration Section](../server/security/authentication/certificate-configuration) to learn how setup security manually.

## Configuration

Read the [Configuration Section](../server/configuration/configuration-options) to learn more about using `settings.json` and see a list of configuration options.

## Related articles

- [Security in RavenDB](../server/security/overview)
