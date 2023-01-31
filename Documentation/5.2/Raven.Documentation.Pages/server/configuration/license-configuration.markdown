# Configuration: License Options

{PANEL:License}

The full license key (as a string) for RavenDB.  
If `License` is specified, it overrides the `License.Path` configuration.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.Path}

Path to the license.json file.  
Either the full path to the license file,  
or a relative path to the license file from the Server directory.  

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

{PANEL:License.CanActivate}

EXPERT ONLY.  
Indicates if license can be activated.  

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.CanForceUpdate}

EXPERT ONLY.  
Indicates if license can be updated from the License Server (api.ravendb.net).

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.CanRenewLicense / License.CanRenew}

EXPERT ONLY.  
Indicates if license can be renewed from the License Server (api.ravendb.net).  
Relevant only for Developer and Community licenses.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.SkipLeasingErrorsLogging}

EXPERT ONLY.  
Skip logging of lease license errors.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.DisableAutoUpdate}

EXPERT ONLY.  
Disable all updates of the license, from string, from path and from the License Server (api.ravendb.net). 

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.DisableAutoUpdateFromApi}

EXPERT ONLY.  
Disable automatic updates of the license from the License Server (api.ravendb.net).  
Can still update the license by either:  

* [Setting the settings.json file](../../start/licensing/activate-license#activate-license-from-settings.json)
* [Setting the environment variables](../../start/licensing/activate-license#activate-license-from-environment-variables)
* Replacing the _license.json_ file on disk

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:License.DisableLicenseSupportCheck}

EXPERT ONLY.  
Disable checking the license support options from the License Server (api.ravendb.net).

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

