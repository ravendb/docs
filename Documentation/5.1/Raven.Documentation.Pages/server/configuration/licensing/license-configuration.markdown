# Configuration: License Options

{PANEL:License}

The full license string for RavenDB. If License is specified, it overrides the `License.Path` configuration.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.Path}

Full or relative path to the license file for RavenDB.

- **Type**: `string`
- **Default**: `license.json`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.Eula.Accepted}

Indicates if End-User License Agreement was accepted.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.DisableAutoUpdateFromApi}

EXPERT ONLY. Disables automatic renewal of the license from api.ravendb.net to allow manual activation and renewal.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.CanActivate}

EXPERT ONLY. Indicates if license can be activated.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.CanForceUpdate}

EXPERT ONLY. Indicates if license can be updated from api.ravendb.net.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.CanRenewLicense}

EXPERT ONLY. Indicates if license can be renewed from api.ravendb.net.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.SkipLeasingErrorsLogging}

EXPERT ONLY. Skip logging of lease license errors.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}
