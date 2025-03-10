# Configure Conflict Revisions Operation

---

{NOTE: }

* By default, RavenDB creates **revisions for conflict documents** for all collections  
  when conflicts occur and when they are resolved.  

* Use `ConfigureRevisionsForConflictsOperation` to disable the feature or modify the configuration. 

* If you define [default configuration](../../../../studio/database/settings/document-revisions#define-default-configuration),  
  then these settings will **override** the conflict revisions configuration.  

* If you define any [collection-specific configuration](../../../../studio/database/settings/document-revisions#define-collection-specific-configuration),  
  then these settings will also **override** the conflict revisions configuration for that collection.  
   * E.g., if the conflict revisions configuration defines that revisions created for conflicting documents will not be purged, 
     but a collection-specific configuration defines an age limit for revisions,  
     revisions for conflicting documents of this collection that exceed this age will be purged.  

* In this page:  
  * [Configure revisions for conflicts - Example](../../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration#configure-revisions-for-conflicts---example)
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration#syntax)
  * [Storage consideration](../../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration#storage-consideration)
{NOTE/}

---

{PANEL: Configure revisions for conflicts - Example}

{CODE:nodejs conflict_revisions_configuration@documentextensions\revisions\client-api\operations\configureRevisionsForConflicts.js /}

{PANEL/}

{PANEL: Syntax} 

{CODE:nodejs syntax_1@documentextensions\revisions\client-api\operations\configureRevisionsForConflicts.js /}

| Parameter | Type | Description |
| - | - | - |
| **database** | `string` | The name of the database whose conflict revisions you want to manage |
| **configuration** | `RevisionsCollectionConfiguration` | The conflict revisions configuration to apply |

{CODE:nodejs syntax_2@documentextensions\revisions\client-api\operations\configureRevisionsForConflicts.js /}

* See properties explanation and default values [here](../../../../document-extensions/revisions/client-api/operations/configure-revisions#revisions-collection-configuration-object).

{PANEL/}

{INFO: }

#### Storage consideration 

Automatic creation of conflict revisions can help track document conflicts and understand their reasons.  
However, it can also lead to a significant increase in the database size if many conflicts occur unexpectedly.  

* Consider limiting the number of conflict revisions kept per document using:  
  `minimumRevisionsToKeep` and/or `minimumRevisionAgeToKeep`.  

* Revisions are purged upon [modification of their parent documents](../../../../document-extensions/revisions/overview#revisions-configuration-execution).  
  If you want to purge a large number of revisions at once, you can **cautiously** [enforce configuration](../../../../studio/database/settings/document-revisions#enforce-configuration).  

{INFO/}

## Related Articles

### Client API

* [Operations: Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  

### Studio

* [Conflicting Document Defaults](../../../../studio/database/settings/document-revisions#editing-the-conflicting-document-defaults)  
* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
