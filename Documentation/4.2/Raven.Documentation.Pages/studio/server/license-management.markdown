# License Management
## Register License 
In order to register an instance of RavenDB with a license, you can: 

![Figure 1. Register License](images/manage-license-1.png "Register License")

- Navigate to _About_ page in studio, click _Replace License_ button, copy your license and click submit.
- Use the following [configuration](../../server/configuration/license-configuration) options:
    * **License**  
      The full license string for RavenDB. If License is specified, it overrides the `License.Path` configuration.

    * **License.Path**  
      Path (either **full** or **relative to the server folder**) to the license file.  
      Default: `license.json` in the **server** folder.  
      E.g. - "License.Path": "D:\\RavenDB\\Server\\license.json"  
      E.g. - "License.Path": "License\\license.json"  

{NOTE: }

* Each instance of RavenDB has to be registered with a license.  
* A development license isn't applicable for commercial use.  

{NOTE/}

## Replace License 

To replace license, same as before ,click _REPLACE LICENSE_ and submit a new one.

![Figure 2. Replace License](images/manage-license-2.png "Replace License")

## Force Update 

In order to fetch the latest license information, click _FORCE UPDATE_.

![Figure 3. Force Update](images/manage-license-3.png "Force Update")

{NOTE License information being update automatically.  _FORCE UPDATE_ does not stop this operation. /}

## Related Articles

- [Configuration : License Options](../../server/configuration/license-configuration)

## Related Links

- [Get License](https://ravendb.net/buy)
- [Support Options](https://ravendb.net/support)


