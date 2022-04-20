# Operations: Configuring Revisions

---

{NOTE: }

* [Revisions configurations](../../../../document-extensions/revisions/overview#revisions-configurations) 
  determine whether [revisions](../../../../document-extensions/revisions/overview) 
  would be automatically created or not, and whether to limit the number of revisions kept per document.  

* You can apply a **default configuration** to all database collections.  
  You can also apply **collection-specific configurations** that would override 
  the default configuration only for the collections you apply them to.  
  This way you can, for example, easily enable revisions for all collections but one 
  (by applying a default configuration that enables Revisions for all collections, 
  and a collection-specific configuration that disables the feature for a single collection).  

* Revisions configurations are **defined** using `RevisionsConfiguration` and `RevisionsCollectionConfiguration` objects.  
  Revisions configurations are **applied** using the `ConfigureRevisionsOperation` Store operation.  


* In this page:  
 * [Syntax](../../../../document-extensions/revisions/client-api/operations/configure-revisions#syntax)  
     * [`ConfigureRevisionsOperation`](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section)  
     * [`RevisionsConfiguration`](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-1)  
     * [`RevisionsCollectionConfiguration`](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-2)  
 * [Usage Flow](../../../../document-extensions/revisions/client-api/operations/configure-revisions#usage-flow)  
 * [Example](../../../../document-extensions/revisions/client-api/operations/configure-revisions#example)  

{NOTE/}

---

{PANEL: Syntax}

### `ConfigureRevisionsOperation`

The `ConfigureRevisionsOperation` Store operation is used to apply a Revisions configuration.  
{CODE:csharp ConfigureRevisionsOperation_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

| Parameter | Type | Description |
| - | - | - |
| **configuration** | `RevisionsConfiguration` | The Revisions configuration to apply |

---

### `RevisionsConfiguration`

This object contains a Dictionary of the revision settings for specific collections, and an optional default configuration.  
{CODE:csharp RevisionsConfiguration_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

* **Properties**  

    | Property | Type | Description |
    | - | - | - |
    | **Default** | `RevisionsCollectionConfiguration` | An optional default configuration that applies to any collection not specified in `Collections` |
    | **Collections** | `Dictionary<string, RevisionsCollectionConfiguration>` | A Dictionary of collection-specific configurations, where - <br> The `keys` are collection names. <br> The `values` are the corresponding configurations. |

---

### `RevisionsCollectionConfiguration`

This object contains the revisions settings for a particular collection.  
{CODE:csharp RevisionsCollectionConfiguration_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

* **Properties**  

    | Configuration Option | Type | Description | Default |
    | - | - | - | - |
    | **MinimumRevisionsToKeep** | `long` | Limit the Number of revisions to keep per document. <br> `null` = no limit | `null` |
    | **MinimumRevisionAgeToKeep** | [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan) | Limit the Age of revisions kept per document. <br> `null` = no age limit | `null` |
    | **Disabled** | `bool` | If `true`, disable Revisions | `false` |
    | **PurgeOnDelete** | `bool` | If `true`, deleting a document will also delete its revisions | `false` |
    | **MaximumRevisionsToDeleteUponDocumentUpdate** | `long` | The maximum number of revisions to delete upon document update. <br> `null` = no limit | `null` |
 
    {INFO: }
    
    * Disabling Revisions prevents the creation of new revisions and the purging of existing ones.
    * Revisions will be purged if they exceeds **either** `MinimumRevisionsToKeep` **or** `MinimumRevisionAgeToKeep`.  
    * Note that applying the configuration will **not** immediately purge revisions.  
      A document's revisions will only be purged 
      [the next time the document is updated](../../../../document-extensions/revisions/overview#configurations-execution).  
    * Use `MaximumRevisionsToDeleteUponDocumentUpdate` to limit the number of revisions that RavenDB 
      is allowed to purge upon document update.  
      E.g. if you're aware that many documents have many revisions pending purging, and prefer 
      that the server will purge up to 15 revisions each time a document is updated.  
     {INFO/}

{PANEL/}


{PANEL: Usage Flow}

To apply a Revisions configuration to all and/or specific collections, follow these steps:  

1. Create a [RevisionsCollectionConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-2) 
   object for every collection you want to set Revisions for.  
2. Add the defined `RevisionsCollectionConfiguration` objects to a 
   [RevisionsConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-1) 
   object.  
3. Send the `RevisionsConfiguration` object to the server using the 
   [ConfigureRevisionsOperation](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section) 
   Store operation.  

{PANEL/}

{PANEL: Example}

In this code sample we apply a default Revisions configuration to all collections, 
and two collection-specific configurations that override it.  
{INFO: }
This code will update the Document Store's [default database](../../../../client-api/setting-up-default-database).  
To update the configuration of a different database, use the 
[`ForDatabase()`](../../../../client-api/operations/how-to/switch-operations-to-a-different-database) method.  
{INFO/}

{CODE-TABS}
{CODE-TAB:csharp:Sync operation_sync@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TAB:csharp:Async operation_async@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What Are Revisions](../../../../client-api/session/revisions/what-are-revisions)
- [What Is a Collection](../../../../client-api/faq/what-is-a-collection)
- [What Are Operations](../../../../client-api/operations/what-are-operations)
- [Switch Operation Database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database)
- [Setting Up a Default Database](../../../../client-api/setting-up-default-database)

### Server

- [Revisions](../../../../server/extensions/revisions)

### Studio

- [Manage Database Group](../../../../studio/database/settings/manage-database-group)
