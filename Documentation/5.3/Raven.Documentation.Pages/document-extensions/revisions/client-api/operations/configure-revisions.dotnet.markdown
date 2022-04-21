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
 * [Examples](../../../../document-extensions/revisions/client-api/operations/configure-revisions#examples)  
     * [Example I - Replace Existing Configuration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#example-i---replace-existing-configuration)  
     * [Example II - Modify Existing Configuration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#example-ii---modify-existing-configuration)  

{NOTE/}

---

{PANEL: Syntax}

### `ConfigureRevisionsOperation`

The `ConfigureRevisionsOperation` Store operation is used to apply your Revisions configurations.  
{CODE:csharp ConfigureRevisionsOperation_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

| Parameter | Type | Description |
| - | - | - |
| **configuration** | `RevisionsConfiguration` | The Revisions configuration to apply |

---

### `RevisionsConfiguration`

This object contains a Default configuration that applies to all collections, 
and a Dictionary of collection-specific configurations that override the default 
configuration for the collections they are defined for.  
{CODE:csharp RevisionsConfiguration_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

* **Properties**  

    | Property | Type | Description |
    | - | - | - |
    | **Default** | `RevisionsCollectionConfiguration` | An optional default configuration that applies to any collection not specified in `Collections` |
    | **Collections** | `Dictionary<string, RevisionsCollectionConfiguration>` | A Dictionary of collection-specific configurations, where - <br> The `keys` are collection names. <br> The `values` are the corresponding configurations. |

---

### `RevisionsCollectionConfiguration`

This object contains a collection-specific Revisions configuration.  
{CODE:csharp RevisionsCollectionConfiguration_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

* **Properties**  

    | Configuration Option | Type | Description | Default |
    | - | - | - | - |
    | **MinimumRevisionsToKeep** | `long` | Limit the Number of revisions to keep per document. <br> `null` = no limit | `null` |
    | **MinimumRevisionAgeToKeep** | [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan) | Limit the Age of revisions kept per document. <br> `null` = no age limit | `null` |
    | **Disabled** | `bool` | If `true`, disable revisions Creation and Purging | `false` |
    | **PurgeOnDelete** | `bool` | If `true`, deleting a document will also delete its revisions | `false` |
    | **MaximumRevisionsToDeleteUponDocumentUpdate** | `long` | The maximum number of revisions to delete upon document update. <br> `null` = no limit | `null` |
 
    {INFO: }
    
    * Revisions will be purged if they exceeds **either** `MinimumRevisionsToKeep` **or** `MinimumRevisionAgeToKeep`.  
    * After applying the configuration, revisions are Created and Purged for a document when the document 
      is created, modified, or deleted.  
    * Use `MaximumRevisionsToDeleteUponDocumentUpdate` to limit the number of revisions that RavenDB 
      is allowed to purge each time a document is updated, e.g. when many documents have many revisions 
      pending purging and you prefer that the server would purge them gradually.  
     {INFO/}

{PANEL/}


{PANEL: Usage Flow}

To apply a Revisions configuration to all and/or specific collections, follow these steps:  

1. Create a [RevisionsCollectionConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-2) 
   object for every collection you want to set Revisions for.  
2. If you want to define a default configuration, create a 
   `RevisionsCollectionConfiguration` object for it.  
3. Add the `RevisionsCollectionConfiguration` objects to a 
   [RevisionsConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-1) 
   object.  
4. Pass the `RevisionsConfiguration` object to the 
   [ConfigureRevisionsOperation](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section) 
   Store operation.  
   Running this method will apply the Revisions configuration.  

{WARNING: }
**Note** that sending your Revisions configuration to the server 
will **replace** the existing default and collection-specific configurations.  
If you want to **modify** the existing configuration (e.g. by 
adding it a collection-specific configuration, or modifying 
the default configuration), **retrieve and edit** the existing 
configuration [as shown here](../../../../document-extensions/revisions/client-api/operations/configure-revisions#example-ii---modify-existing-configuration).  
{WARNING/}

{PANEL/}

{PANEL: Examples}

### Example I - Replace Existing Configuration

In this example we **replace** the entire Revisions configuration that the server 
currently applies with our own settings (that include a default configuration 
and two collection-specific configurations).  

{INFO: }
Note that the configuration is applied to the Document Store's [default database](../../../../client-api/setting-up-default-database).  
To configure a different database, use the 
[`ForDatabase()` method](../../../../client-api/operations/how-to/switch-operations-to-a-different-database).  
{INFO/}

{CODE-TABS}
{CODE-TAB:csharp:Sync configure-revisions_sync@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TAB:csharp:Async configure-revisions_async@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TABS/}

---

### Example II - Modify Existing Configuration

In this example we **modify** the Revisions configuration that the server 
currently applies.  
We retrieve the existing configuration from the database record, 
and check its contents.  
If the existing configuration is empty, we simply define a new configuration.  
If the existing configuration is populated, we modify it (replacing its default 
configuration with our own and replacing or adding two collection-specific configurations.  

{CODE-TABS}
{CODE-TAB:csharp:Sync update-existing-configuration_sync@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TAB:csharp:Async update-existing-configuration_async@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
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
