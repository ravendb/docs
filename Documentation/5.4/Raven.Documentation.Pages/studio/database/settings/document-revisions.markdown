# Document Revisions

---

{NOTE: }
 
* Revisions configuration settings can be managed from the **Document Revisions** view.  

* Learn more about revisions [here](../../../document-extensions/revisions/overview).

* In this page:
  * [Document revisions view](../../../studio/database/settings/document-revisions#document-revisions-view)  
  * [Revisions configuration](../../../studio/database/settings/document-revisions#revisions-configuration)  
  * [Define default configuration](../../../studio/database/settings/document-revisions#define-default-configuration)  
  * [Define collection-specific configuration](../../../studio/database/settings/document-revisions#define-collection-specific-configuration)  
  * [Edit conflicting document defaults](../../../studio/database/settings/document-revisions#edit-conflicting-document-defaults)  
     * [Conflicting documents example](../../../studio/database/settings/document-revisions#conflicting-documents-example)  
  * [Enforce configuration](../../../studio/database/settings/document-revisions#enforce-configuration)  


{NOTE/}

---

{PANEL: Document revisions view}

![Document Revisions View](images/revisions/document-revisions-view.png "Document Revisions View")

1. **Document Revisions View**  
   Click to open the **Document Revisions** view.  

2. **Set Status**  
   Check the selection box to select all configurations.  
   Click the _Set Status_ dropdown list to Enable or Disable selected configurations.  
   ![Enable or Disable Configurations](images/revisions/set-status-dropdown.png "Enable or Disable Configurations")  

3. **Save**  
   Click after modifying the configuration to apply your changes.  

4. **Revert Revisions**  
   Click to [revert the database](../../../document-extensions/revisions/revert-revisions) to its state at a specified point in time.  
   You can select whether to revert specific collections or revert all collections.  
   Only documents are reverted. Other entities (such as ongoing tasks) are not modified by this process.  

5. **Enforce Configuration**  
   Click to [enforce the revisions configuration](../../../studio/database/settings/document-revisions#enforce-configuration).  
   {WARNING: }
   This operation may delete many revisions irrevocably and require substantial 
   server resources.  
   Please read carefully the dedicated section.  
   {WARNING/}  

6. **Create document defaults**  
   Click to define [default configuration](../../../studio/database/settings/document-revisions#define-default-configuration) 
   that will apply to documents in all collections that don't have a  
   collection-specific configuration defined.   

7. **Add a collection-specific configuration**  
   Click to create a [configuration for a specific collection](../../../studio/database/settings/document-revisions#define-collection-specific-configuration).  
   If default settings were defined, the collection-specific configuration will override them for this collection.  

8. **The defined Revisions configuration**  
   Read more [below](../../../studio/database/settings/document-revisions#revisions-configuration).  

{PANEL/}

{PANEL: Revisions configuration}

* The Revisions configuration can include:  
  * __Default configuration__ - applies to all document collections.  
  * __Collection-specific configurations__ - override the default settings for these collections.  

* When no default configuration or collection-specific configurations are defined and enabled,  
  no revisions will be created for any document.

* When a revision configuration is defined,  
  its rules are applied to the revisions of a document upon any modification of the document. 

![Defined Configuration](images/revisions/defined-configuration.png "Defined Configuration")

1. **Document Defaults**  
   This is the [default revisions configuration](../../../studio/database/settings/document-revisions#define-default-configuration) that applies to all non-conflicting documents in all collections  
   that don't have a collection-specific configuration defined.   
   These settings are optional and can be removed.  

2. **Conflicting Document Defaults**  
   This pre-defined revisions configuration is for conflicting documents only.  
   When enabled, a revision is created for each conflicting item.  
   A revision is also created for the conflict resolution document.   
   When a collection-specific configuration is defined, it overrides these defaults.  
    * The Conflicting Document Defaults configuration cannot be removed.  
    * You can [modify](../../../studio/database/settings/document-revisions#edit-conflicting-document-defaults) this configuration,  
      or disable it if not interested in tracking document conflicts using revisions.  
   
3. **Collections**  
   These are optional collection-specific configurations whose settings override the Document Defaults  
   and the Conflicting Document Defaults for the collections they are defined for.  

4. **Selection Box**  
   Click to select this configuration.  
   Selected configurations can be enabled or disabled using the **Set status** button.  

5. **Configuration Settings**  
   Read more about the available settings in the sections dedicated to defining them below.  

6. **Controls**  
    * **Disable/Enable** - Click to Enable or Disable the configuration.  
    * **Edit** - Click to modify the configuration.  
    * **Remove** - Click to remove the configuration.  

{PANEL/}

{PANEL: Define default configuration}

![Define Default Settings](images/revisions/define-default-settings.png "Define Default Settings")

1. **Create document defaults**  
   Click to define default configuration that will apply to non-conflicting documents in all collections  
   that don't have a collection-specific configuration defined.  

2. **Purge revisions on document delete**  
   Enable if you want document revisions to be deleted when their parent document is deleted.

3. **Limit # of revisions to keep** <a id="limit-revisions" />  
   ![Limit By Number](images/revisions/define-default-settings_limit-by-number.png "Limit By Number")  
   Enable to limit the number of revisions that will be kept in the revisions storage per document.  
   Upon revision creation (when the parent document is modified), if the number of revisions exceeds this limit  
   then older revisions will be purged (starting from the oldest revision).  

     * Enabling the # of revisions to keep will display the following setting as well:  
       <br>
       ![Maximum Number of Revisions to Purge](images/revisions/maximum-revisions-to-purge.png "Maximum Number of Revisions to Purge")   
       Enable to limit the number of revisions that RavenDB is allowed to purge per document modification.  
       {INFO: }
       This will be the maximum number of revisions that RavenDB will purge per document modification,  
       even if the number of revisions that pend purging is greater.  
       Setting this limit can reserve server resources if many revisions pend purging,  
       by dividing the purging between multiple document modifications.  
       {INFO/}

4. **Limit # of revisions to keep By Age**  
   ![Limit By Age](images/revisions/define-default-settings_limit-by-age.png "Limit By Age")  
   Enable to set a revisions age limit.  
   Revisions older than the defined retention time will be purged when their parent document is modified.  

   * Enabling the age limit setting will also display the __Set # of revisions to delete upon document update__   
     (see above).  
   
5. Click **OK** to keep these default settings, or **Cancel**.  
   Confirming will add the new settings to the revisions configuration **Defaults** section:  
   ![Defined Default Settings](images/revisions/defined-default-settings.png "Defined Default Settings")
   {INFO: }
   Click __Save__ when done.
   {INFO/}

{PANEL/}

{PANEL: Define collection-specific configuration}

![Define collection-specific configurations](images/revisions/define-collection-specific-configuration.png "Define Collection-Specific Configurations")

1. **Add a collection-specific configuration**  
   Click to define a configuration that applies to a specific collection.  
   This configuration will override _Document Defaults_ and _Conflicting Document Defaults_ configurations.  

2. **Collection**  
   Select or enter a collection to define a configuration for.  
   ![Select Collection](images/revisions/select-collection.png "Select Collection")  

3. **Configuration options**  
   These options are similar to those explained above for the [default configuration](../../../studio/database/settings/document-revisions#define-default-configuration),  
   the only difference is the configuration scope.  

4. Click __OK__ to keep the configuration, or __Cancel__.  
   Confirming will add the new configuration to the **Collections** section:  
   ![Defined Configuration](images/revisions/defined-collection-specific-configuration.png "Defined Configuration")  
   {INFO: }
   Click __Save__ when done.
   {INFO/}

{PANEL/}

<a id="editing-the-conflicting-document-defaults" />
{PANEL: Edit conflicting document defaults}

* Click the __Edit__ button to edit the default configuration for conflicting documents.  

![Conflicting Document Defaults](images/revisions/conflicting-document-defaults.png "Conflicting Document Defaults")

![Editing the Conflicting Document Defaults](images/revisions/edit-conflicting-docs-configuration.png "Editing the Conflicting Document Defaults")

* The settings options are similar to those explained above for the [default configuration](../../../studio/database/settings/document-revisions#define-default-configuration).   
  
* Note: the __Limit # of revisions to keep by age__ value is set to `45 Days` by default.  
  This means that revisions created for conflicting documents will start to be purged after 45 days,  
  whenever their parent documents are modified.  

{NOTE: }

#### Conflicting documents example:

* For this example, we created a conflict by replicating into the database a document with an ID similar to that of a local document.  

* Revisions will be created when the documents **enter a conflict** and when the conflict is **resolved**.  
  So in this case, **three** revisions were created:  
    1. when the replicated document arrived and entered a conflict state.   
    2. when the local document entered a conflict state on the arrival of the replicated document.  
    3. when the conflict was resolved by replacing the local document with the replicated one.  
       {INFO: }
       In this example, the conflict was resolved by placing the replicated version as the current document. 
       Learn more about conflict resolution [here](../../../studio/database/settings/conflict-resolution#conflict-resolution).  
       {INFO/}

* To see these revisions, open the document's [Revisions tab](../../../studio/database/document-extensions/revisions#revisions-tab).  
  The revision state is indicated by:  
    * A red __title__ at the top (i.e. "Conflict revision" or "Resolved revision").  
    * An __icon__ next to the revision's creation time in the right Properties pane.  
    * A __flag__ in the revision's `@flags` metadata property (i.e. "Conflicted" or "Resolved").  

{NOTE/}

![1. Incoming Document in Conflict](images/revisions/conflict-revisions-1.png "1. Incoming Document in Conflict")

![2. Local Document in Conflict](images/revisions/conflict-revisions-2.png "2. Local Document in Conflict")

![3. Conflict Resolved](images/revisions/conflict-revisions-3.png "3. Conflict Resolved")

{PANEL/}

{PANEL: Enforce configuration}

![Enforce Configuration](images/revisions/enforce-configuration-1.png "Enforce Configuration")

* The revision configuration rules are usually applied to the revisions of a document when the document is modified.  
* Executing __Enforce__ will apply the current revision configuration rules on ALL existing revisions for ALL documents at once.  

{NOTE: }

__For collections that have a specific revision configuration__:  

  * The collection-specific configuration will be executed per collection.  
  * Revisions that pend purging (according to the configuration) will be __deleted__.
{NOTE/}

{NOTE: } 

__For collections that DON'T have a specific revision configuration__:  

  * Non-conflicting documents:  
      * If Document Defaults are defined & enabled, they will be applied.  
      * If NOT defined, or if disabled, ALL non-conflicting document revisions will be __deleted__.  
  * Conflicting documents:  
      * If Conflicting Document Defaults are enabled, they will be applied to the conflicting document revisions.  
      * If disabled, ALL conflicting document revisions will be __deleted__.  
{NOTE/}

{WARNING: }

* Large databases may contain numerous revisions pending purging that Enforcing will purge all at once.  
  Be aware that this operation may require substantial server resources, and time it accordingly.  
* Revisions that no configuration currently applies to will be deleted.  
  Make sure your configuration includes the default settings and collection-specific configurations  
  that will keep the revisions you want to keep intact.  

{WARNING/}

{PANEL/}

## Related Articles

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: Conflict Revisions Configuration](../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration)  
* [Revisions: API Overview](../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Document Extensions: Revisions](../../../studio/database/document-extensions/revisions)  
