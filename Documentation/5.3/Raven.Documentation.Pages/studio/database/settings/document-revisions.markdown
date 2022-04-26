
# Settings: Document Revisions

---

{NOTE: }

* Use the **Document Revisions** Settings view to create and manage a 
  **Revisions configuration**.  
* Learn about Revisions and the Revisions configuration here:  
   * [Revisions Overview](../../../document-extensions/revisions/overview)  
   * [Revisions Configuration](../../../document-extensions/revisions/overview#revisions-configuration)  
   * [Revisions Configuration API](../../../document-extensions/revisions/client-api/operations/configure-revisions)  

* In this page:
  * [](../../../)  

{NOTE/}

---

{PANEL: Document Reisions View}

![Document Reisions View](images/revisions/document-revisions-view.png "Document Reisions View")

1. **Document Revisions View**  
   Click to open the **Document Revisions** Settings view.  
2. **Set Status**  
   Check the selection box to select all configurations.  
   Click the _Set Status_ dropdown list to Enable or Disable selected configurations.  
   ![Enable or Disable Configurations](images/revisions/set-status-dropdown.png "Enable or Disable Configurations")
3. **Save**  
   Click after modifying the configuration to apply your changes.  
4. **Revert Revisions**  
   Click to [revert the database](../../../document-extensions/revisions/revert-revisions) 
   to a specified point in time.  
5. **Enforce Configuration**  
   Click to [Enforce the Revisions configuration](../../../studio/database/settings/document-revisions#enforce-configuration).  
   {WARNING: }
   This operation may delete many revisions irrevocably and require substantial 
   server resources. Please read carefully the section dedicated to it.  
   {WARNING/}
6. **Create document defaults**  
   Click to create a default configuration that will apply to documents of all 
   the collections that a collection-specific configuration isn't defined for.  
7. **Add a collection-specific configuration**  
   Click to create a configuration for a specific collection.  
   If a default configuration exists, collection-specific configurations will 
   override it for the collections they are defined for.  
8. **The Defined Revisions configuration**  
   The Revisions configuration can be comprised of:  
   ![Configurations Types](images/revisions/configuration-types.png "Configurations Types")
    * **A**. **Document Defaults**: The Default Configuration  
      An optional Configuration that applies to all documents 
      that a collection-specific configuration is not defined for.  
      {INFO: }
      As long as no default or collection-specific configurations are 
      defined, Revisions will be **disabled** for all collections.  
      {INFO/}
    * **B**. **Conflicting Document Defaults**  
      A mandatory configuration, provided so revisions would be created for 
      _conflicting documents_ and for _conflict resolution documents_.  
      Collection-specific configurations **override** this configuration for 
      the collections they are defined for.  
    * **C**. **Collections**: Collection-Specific Configurations  
      Optional configurations whose settings override those of the default 
      configuration (if one exists) for the collections they are defined for.  

{PANEL/}

{PANEL: Enforce Configuration}

![Enforce Configuration](images/revisions/enforce-configuration.png "Enforce Configuration")

When the **Enforce Configuration** button is clicked:  

* All the revisions that pend purging will be **purged**.  
  Revisions that pend purging are revisions that should be purged 
  according to the default settings or to a collection-specific 
  configuration that applies to them.  
* Revisions that no collection-specific configuration applies 
  to will be **deleted**, if the Revisions configuratin has no 
  default settings (or its default settings are disabled).  

  {WARNING: }
  
  * Large databases and collections may contain numerous revisions pending 
    purging, that Enforcing Configuration will purge all at once.  
    Be aware that this operation may require substantial server resources, 
    and time it accordingly.  
  * Revisions that were created over time that no configuration currently 
    applies to will be deleted. Make sure your configuration includes the 
    default settings and collection-specific configurations that will 
    keep the revisions you want to keep intact.  
  {WARNING/}
  
{PANEL/}

## Related Articles

### Document Extensions

- [Session: What are Revisions](../../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
