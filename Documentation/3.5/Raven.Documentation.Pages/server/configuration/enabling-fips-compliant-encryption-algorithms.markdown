# Configuration: How to enable FIPS compliant encryption algorithms?

{INFO This feature requires a valid **enterprise** license. /}

By default, RavenDB server and client are not using [FIPS compliant](http://technet.microsoft.com/en-us/library/cc180745.aspx) encryption algorithms. In order to turn it on, please refer to the appropriate sections below.

## Enabling FIPS encryption algorithms on server

{WARNING:Warning}

- `Raven/Encryption/FIPS` is a **global setting** and it **applies to all databases** on a server.
- `Raven/Encryption/FIPS` **must** be set before any database creation (including `<system>`).
- Databases created with `Raven/Encryption/FIPS` turned off (default) will not work on other servers with 'FIPS' turned on (and vice versa). They must be exported and imported.

{WARNING/}

FIPS compliant encryption algorithms can be turned on by setting `Raven/Encryption/FIPS` configuration to `true` in [server configuration file](../../server/configuration/configuration-options).

## Enabling FIPS encryption algorithms on client

FIPS compliant encryption algorithms on client can be turned on by setting `Raven/Encryption/FIPS` appSetting to `true` in application configuration file (`app.config` or `web.config`). 

## Remarks

{NOTE Encryption algorithms used by server and client must match. /}
