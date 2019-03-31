# License Management
## Register License 
In order to register an instance of RavenDB with a license, you can: 

- Navigate to _About_ page in studio, click _Replace License_ button, copy your license and click submit.
- Use the following [configuration](../../server/configuration/license-configuration) options:
	*	**License**
	The full license string for RavenDB. If License is specified, it overrides the `License.Path` configuration.

	*   **License.Path**
	The path to the license file for RavenDB.   
	_Default:_ license.json

{NOTE Each instance of RavenDB has to be registered with a license. A development license isn't applicable for commercial use. /}

## Related Articles

- [Configuration : License Options](../../server/configuration/license-configuration)

## Related Links

- [Get License](https://ravendb.net/buy)
- [Support Options](https://ravendb.net/support)
