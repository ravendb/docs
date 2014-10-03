# Configuration : License registration

In order to register an instance of RavenDB with a license you can either: 

Rename the license file to License.xml and put it in the bin folder where RavenDB executable exists or use the following configuration options:

* **Raven/License**
	The full license string for RavenDB. If Raven/License is specified, it overrides the Raven/LicensePath configuration.

* **Raven/LicensePath**
	The path to the license file for RavenDB.   
	_Default:_ ~\license.xml

{NOTE Each instance of RavenDB outside of the development machines must be registered with a license. /}

## Related articles

TODO