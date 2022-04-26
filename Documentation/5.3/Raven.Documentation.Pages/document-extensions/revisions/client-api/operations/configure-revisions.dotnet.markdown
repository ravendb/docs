# Operations: Configuring Revisions

---

{NOTE: }

* Use the [ConfigureRevisionsOperation](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section) 
  Store operation to apply a [Revisions configuration](../../../../document-extensions/revisions/overview#revisions-configuration) 
  to the database.  

* A Revisions configuration is comprised of Default settings and/or Collection-specific configurations.  
   * The **default settings** apply to all database collections.  
   * **Collection-specific configurations** override the default settings for the collections 
     they are applied to.  

* Default settings and Collection-specific configurations are defined in 
  [RevisionsCollectionConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-2) 
  objects.  

* All `RevisionsCollectionConfiguration` objects are gathered in a single 
  [RevisionsConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-1) 
  object.  
  There is one `RevisionsConfiguration` object per database, stored in the database record.  

* The `RevisionsConfiguration` object is passed to the 
  [ConfigureRevisionsOperation](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section) 
  Store operation and applied to the database when the operation is executed, **replacing** the current 
  Revisions configuration in the database record.  

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

This object contains the default settings that apply to all collections, 
and a Dictionary of collection-specific configurations that override the default 
settings for the collections they are defined for.  
{CODE:csharp RevisionsConfiguration_definition@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisionsDefinitions.cs /}

* **Properties**  

    | Property | Type | Description |
    | - | - | - |
    | **Default** | `RevisionsCollectionConfiguration` | Optional default settings that apply to any collection not specified in `Collections` |
    | **Collections** | `Dictionary<string, RevisionsCollectionConfiguration>` | A Dictionary of collection-specific configurations, where - <br> The `keys` are collection names. <br> The `values` are the corresponding configurations. |

---

### `RevisionsCollectionConfiguration`

This object contains a collection-specific Revisions configuration.  
It can also be used to define the default settings for all database collections.  
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
2. If you want to define default settings, create a 
   `RevisionsCollectionConfiguration` object for them.  
3. Add all the `RevisionsCollectionConfiguration` objects you created to a 
   [RevisionsConfiguration](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section-1) 
   object.  
4. Pass the `RevisionsConfiguration` object to the 
   [ConfigureRevisionsOperation](../../../../document-extensions/revisions/client-api/operations/configure-revisions#section) 
   Store operation.  
   Executing the operation will replace the Revisions configuration in the database record.  
   {WARNING: }
    If you want to **modify** the existing configuration rather than replace it,  
    retrieve the current configuration and edit it 
    [as shown here](../../../../document-extensions/revisions/client-api/operations/configure-revisions#example-ii---modify-existing-configuration).  
    {WARNING/}

{PANEL/}

{PANEL: Examples}

### Example I - Replace Existing Configuration

In this example we **replace** the existing Revisions configuration 
(if there is one) with our own, applying default settings and two 
collection-specific configurations.  

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

In this example we **modify** the existing Revisions configuration's 
default settings and collection-specific configurations.  

We retrieve the existing configuration from the database record, 
and check its contents.  
If no configuration is found, we define a new configuration.  
If a configuration is already defined, we modify the existing configuration.  

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
