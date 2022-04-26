
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
  * [Document Reisions View](../../../studio/database/settings/document-revisions#document-reisions-view)  
  * [Enforce Configuration](../../../studio/database/settings/document-revisions#enforce-configuration)  
  * [Defining Default Settings](../../../studio/database/settings/document-revisions#defining-default-settings)  
  * [Defining a Collection-Specific Configuration](../../../studio/database/settings/document-revisions#defining-a-collection-specific-configuration)  


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
   Click to define default settings that will apply to documents of all 
   the collections that a collection-specific configuration isn't defined for.  
7. **Add a collection-specific configuration**  
   Click to create a configuration for a specific collection.  
   If default settings were defined, a collection-specific configuration 
   will override them for the collection it is defined for.  
8. **The Defined Revisions configuration**  
   ![Configurations Types](images/revisions/configuration-types.png "Configurations Types")
   The Revisions configuration can be comprised of:  
    * **A**. **Document Defaults**: The Default Settings  
      Optional settings that apply to all documents 
      that a collection-specific configuration is not defined for.  
      {INFO: }
      As long as no default settings or collection-specific configurations 
      are defined, Revisions will be **disabled** for all collections.  
      {INFO/}
    * **B**. **Conflicting Document Defaults**  
      Pre-defined settings, provided to assure that revisions would be 
      created for _conflicting documents_ and for _conflict resolution documents_.  
      Collection-specific configurations **override** these settings for 
      the collections they are defined for.  
    * **C**. **Collections**: Collection-Specific Configurations  
      Optional configurations whose settings override those of the default 
      configuration (if one exists) for the collections they are defined for.  

{PANEL/}

{PANEL: Enforce Configuration}

![Enforce Configuration](images/revisions/enforce-configuration-1.png "Enforce Configuration")

* Enforcing Configuration will:  
   * **Enforce the default settings and all collection-specific configurations.**  
     All the revisions that pend purging will be **purged**.  
     Revisions that pend purging are revisions that should be purged 
     according to the default settings or to the collection-specific 
     configuration that applies to them.  
   * **Delete all the revisions that no configuration applies to.**  
     If default settings were's defined for the Revisions configuration 
     (or the default settings are disabled), Revisions that no collection-specific 
     configuration applies to will be **deleted**.  
* **Note**:  
      {WARNING: }

      * Large databases and collections may contain numerous revisions 
        pending purging, that Enforcing Configuration will purge all at once.  
        Be aware that this operation may require substantial server resources, 
        and time it accordingly.  
      * Revisions that were created over time that no configuration currently 
        applies to will be deleted.  
        Make sure your configuration includes the default settings and 
        collection-specific configurations that will keep the revisions 
        you want to keep intact.  

    {WARNING/}

* Enforcing Configuration will detail the process and allow you to proceed or cancel.  
  ![Proceed or Cancle](images/revisions/enforce-configuration-2.png "Proceed or Cancle")

{PANEL/}

{PANEL: Defining Default Settings}

![Define Default Settings](images/revisions/define-default-settings.png "Define Default Settings")

1. **Create document defaults**  
   Click to define default settings that will apply to all the collections 
   that a collection-specific configuration is not applied to.  
2. **Purge revisions on document delete**  
   Enable if you want document revisions to be deleted when their 
   parent document is deleted.  
3. **Limit # of revisions to keep**  
   ![Limit By Number](images/revisions/define-default-settings_limit-by-number.png "Limit By Number")  
   Enable to set a limit to the number of revisions that can be kept in the revisions 
   storage per document.  
   If this limit is set, old revisions will be purged when new ones are added 
   if the limit is exceeded.  
    * **Set # of revisions to delete upon document update**  
      Enabling **Limit # of revisions to keep** will display this setting as well:
      ![Maximum Number of Revisions to Purge](images/revisions/maximum-revisions-to-purge.png "Maximum Number of Revisions to Purge")  
      Enable to set a limit to the number of revisions that RavenDB is allowed 
      to purge per document modification.  
      {INFO: }
      RavenDB will refrain from purging more revisions than this limit allows 
      it to purge, even if the number of revisions that pend purging exceeds it.  
      Setting this limit can reserve server resources if many revisions pend 
      purging, by dividing the purging between multiple modification events.  
      {INFO/}
4. **Limit # of revisions to keep By Age**  
   ![Limit By Age](images/revisions/define-default-settings_limit-by-age.png "Limit By Age")
   Enable to set a Revisions age limit.  
   If this limit is set, revisions older than the age it defines will be purged 
   when the document is modified.  
    * Enabling this setting will disply the **Set # of revisions to delete upon document update** 
      setting as well (read about it above).  
5. Click **OK** to modify or create the default settings, or **Cancel**.  
   Confirming will add the new settings to the Revisions configuration **Default** section:  
   ![Defined Default Settings](images/revisions/defined-default-settings.png "Defined Default Settings")
   Remember to also [Save the configuration](../../../studio/database/settings/document-revisions#document-reisions-view) 
   to apply the new settings.  

{PANEL/}

{PANEL: Defining a Collection-Specific Configuration}

![Defining Collection-Specific Configurations](images/revisions/define-collection-specific-configuration.png "Defining Collection-Specific Configurations")

1. **Add a collection-specific configuration**  
   Click to define a configuration that applies to a specific collection 
   and overrides the default settings for this collection.  
2. **Collection**  
   Click to select a collection to define a configuration for.  
   ![Select Collection](images/revisions/select-collection.png "Select Collection")
3. **Purge revisions on document delete**  
   Enable if you want document revisions to be deleted when their 
   parent document is deleted.  
4. **Limit # of revisions to keep**  
   ![Limit By Number](images/revisions/define-default-settings_limit-by-number.png "Limit By Number")  
   Enable to set a limit to the number of revisions that can be kept in the revisions 
   storage per document.  
   If this limit is set, old revisions will be purged when new ones are added 
   if the limit is exceeded.  
    * **Set # of revisions to delete upon document update**  
      Enabling **Limit # of revisions to keep** will display this setting as well:
      ![Maximum Number of Revisions to Purge](images/revisions/maximum-revisions-to-purge.png "Maximum Number of Revisions to Purge")  
      Enable to set a limit to the number of revisions that RavenDB is allowed 
      to purge per document modification.  
      {INFO: }
      RavenDB will refrain from purging more revisions than this limit allows 
      it to purge, even if the number of revisions that pend purging exceeds it.  
      Setting this limit can reserve server resources if many revisions pend 
      purging, by dividing the purging between multiple modification events.  
      {INFO/}
5. **Limit # of revisions to keep By Age**  
   ![Limit By Age](images/revisions/define-default-settings_limit-by-age.png "Limit By Age")
   Enable to set a Revisions age limit.  
   If this limit is set, revisions older than the age it defines will be purged 
   when the document is modified.  
    * Enabling this setting will disply the **Set # of revisions to delete upon document update** 
      setting as well (read about it above).  
6. Click **OK** to modify or create the configuration, or **Cancel**.  
   Confirming will add the new configuration to the **Collections** section:  
   ![Defined Configuration](images/revisions/defined-collection-specific-configuration.png "Defined Configuration")
   Remember to also [Save the configuration](../../../studio/database/settings/document-revisions#document-reisions-view) 
   to apply the new configuration.  

{PANEL/}

## Related Articles

### Document Extensions

- [Session: What are Revisions](../../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
