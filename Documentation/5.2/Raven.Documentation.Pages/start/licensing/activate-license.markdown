# Activate License 

---

{NOTE: }

* Each RavenDB cluster must be registered with its own license.  

* The license key is sent to the email that was provided when obtaining the license.  
  There are a few ways to __activate your license__, see below.  

* In this page:
    * [Activate license from settings.json](../../start/licensing/activate-license#activate-license-from-settings.json)
    * [Activate license from environment variables](../..//start/licensing/activate-license#activate-license-from-environment-variables)
    * [Activate license from Studio](../../start/licensing/activate-license#activate-license-from-studio)

{NOTE/}

---

{PANEL: Activate license from settings.json}

* To activate the license from _settings.json_,   
  add either of the below [license configuration](../../server/configuration/license-configuration) options to your [settings.json](../../server/configuration/configuration-options#settings.json) file.  
* Note: If this is the first time you run RavenDB, and the _settings.json_ file does Not exist yet,  
  then manually copy file **_settings.default.json_** to **_settings.json_** and make your changes there.  

{NOTE: }
__"License"__  

  * Embed the full license key as a string, e.g.:  
    `"License": "{ paste your license key including curly brackets here }"`.  

  * When `License` is specified, it overrides the `License.Path` configuration.  
{NOTE/}

{NOTE: }
__"License.Path"__  

  * Save the license key to a `license.json` file.  

  * Provide the path to this file:  
    * Either the __full__ path to the license file, e.g.:  
      `"License.Path": "D:\\RavenDB\\Server\\license.json"`  
    * Or, a __relative__ path to the license file from the Server folder  
      `"License.Path": "License\\license.json"`  
      (where 'License' folder is under the 'Server' folder)  
{NOTE/}

{PANEL/}

{PANEL: Activate license from environment variables}

* You can set either of the following [environment variables](../../server/configuration/configuration-options#environment-variables) with your license info.  
* Note: _settings.json_ configuration options override environment variables settings.  

{NOTE: }
__RAVEN_LICENSE__

  * Set the full license key in the RAVEN_LICENSE environment variable.  
    (Enter the JSON object, not as a string).  

  * If RAVEN_LICENSE is set, it overrides the RAVEN_LICENSE_PATH environment variable.  
{NOTE/}

{NOTE: }
__RAVEN_LICENSE_PATH__  

  * Provide a path to the `license.json` file.
  * Set either the __full__ path or a __relative__ path to the Server folder.  
{NOTE/}

{PANEL/}

{PANEL: Activate license from Studio}

You can activate the license from Studio:  

![Register License](images/register-1.png "Register license")

1. **About**  
   Navigate to the _About_ page in Studio
   
2. **Activate**  
   Click the _REGISTER LICENSE_ button

![Register License](images/register-2.png "Register license")

{PANEL/}

## Related Articles

### Licensing
- [Licensing overview](../../start/licensing/licensing-overview)
- [Replace license](../../start/licensing/replace-license)
- [Renew license](../../start/licensing/renew-license)
- [Force update license](../../start/licensing/force-update)

### Server
- [License configuration options](../../server/configuration/license-configuration)


