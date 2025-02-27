# Configure Revisions Operation

---

{NOTE: }

* Use `ConfigureRevisionsOperation` to apply the following [revisions configuration](../../../../studio/database/settings/document-revisions#revisions-configuration) to the database:  
  * **Default configuration** - applies to all document collections.  
  * **Collection-specific configurations** - override the default settings for these collections.  
  * To apply a configuration for conflict document revisions see [configure conflict revisions](../../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration).  

* The configuration passed to this operation will **REPLACE** the current revisions configuration in the database.  
  To **MODIFY** existing configuration, fetch the current configuration from the database record first.  

* After applying the configuration,  
  revisions are created and purged for a document whenever the document is created, modified, or deleted.  

* To create a revision when there is no configuration defined (or enabled) see: [force revision creation](../../../../document-extensions/revisions/overview#force-revision-creation)

* By default, the operation will be applied to the [default database](../../../../client-api/setting-up-default-database).  
  To operate on a different database see [switch operations to different database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database).  

* In this page:  
  * [Replace configuration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#replace-configuration)  
  * [Modify configuration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#modify-configuration)  
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/configure-revisions#syntax)  

{NOTE/}

---

{PANEL: Replace configuration}

In this example, we create a new `RevisionsConfiguration` for the database.  
If revisions configuration already exists in the database - it will be **replaced**.
{CODE:php replace_configuration@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.php /}

{PANEL/}

{PANEL: Modify configuration}

In this example, we fetch the existing revisions configuration from the database record and **modify** it.   
{CODE:php modify_configuration@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.php /}

| Parameter | Type | Description |
| - | - | - |
| **configuration** | `?RevisionsConfiguration` | The revisions configuration to apply |

{CODE:php syntax_2@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.php /}

| Property | Type | Description |
| - | - | - |
| **defaultConfig** | `RevisionsCollectionConfiguration` | Optional default settings that apply to any collection Not specified in `collections`. |
| **collections** | `null` or `RevisionsCollectionConfigurationArray` or `array` | An array of collection-specific configurations<br>The `keys` are the collection names<br>The `values` are the corresponding configurations.<br>Overrides the default settings for the collections defined. |

{CODE:php syntax_3@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.php /}

<a id="revisions-collection-configuration-object" />

| Property | Type | Description |
| - | - | - |
| **minimumRevisionsToKeep** | `?int` | <ul><li>This number of revisions will be kept per document.</li><li>Older revisions exceeding this number will be purged upon the next document modification.<li> **Default** : `None` = no limit </li></ul> |
| **minimumRevisionAgeToKeep** | `?Duration` | <ul><li>Limit the number of revisions kept per document by their age.</li><li>Revisions that are older than this time will be purged upon the next document modification.</li><li> **Default** : `None` = no age limit</li><ul> |
| **disabled** | `bool` | <ul><li>`fFalse` ( **Default** ) - Revisions will be created and purged according to the configuration.</li><li>`True` - No revisions will be created or purged.</li></ul> |
| **purgeOnDelete** | `bool` | <ul><li>`False` ( **Default** ) - Revisions of a deleted document are moved to the [Revisions Bin](../../../../studio/database/document-extensions/revisions#revisions-bin).</li><li>`True` - When a document is deleted all its revisions are also deleted.</li></ul> |
| **maximumRevisionsToDeleteUponDocumentUpdate** | `?int` | <ul><li>The maximum number of revisions to delete upon each document modification.</li><li> **Default** : `None` = no limit,<br> all revisions that pend purging will be deleted.</li></ul> |

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  
* [What Is a Collection](../../../../client-api/faq/what-is-a-collection)
* [What Are Operations](../../../../client-api/operations/what-are-operations)
* [Switch Operation Database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database)
* [Setting Up a Default Database](../../../../client-api/setting-up-default-database)

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
* [Manage Database Group](../../../../studio/database/settings/manage-database-group)
