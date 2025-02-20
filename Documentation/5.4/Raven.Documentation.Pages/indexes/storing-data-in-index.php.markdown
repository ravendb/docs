# Indexes: Storing Data in Index
---

{NOTE: }

Once the [tokenization and analysis](../indexes/using-analyzers) process is completed, 
the resulting tokens created by the used analyzer are stored in the index.  
By default, tokens saved in the index are available for searching, but their original 
field values are not stored.  

Lucene allows you to store the original field text (before it is analyzed) as well.  

* In this page:
  * [Storing Data in Index](../indexes/storing-data-in-index#storing-data-in-index)

{NOTE/}

{PANEL: Storing Data in Index}

Lucene's original field text storage feature is exposed in the index definition object as 
the `storage` property of the `IndexFieldOptions`.  

When the original values are stored in the index, they become available for retrieval via 
[projections](../indexes/querying/projections).  

{CODE-TABS}
{CODE-TAB:php:AbstractIndexCreationTask storing_1@Indexes\Storing.php /}
{CODE-TAB:php:Operation storing_2@Indexes\Storing.php /}
{CODE-TABS/}

{INFO: }
The default `storage` value for each field is `FieldStorage.NO`.  
Keep in mind that storing fields will increase disk space usage.  
{INFO/}

{INFO: }
If **the projection requires only the fields that are stored**, the document will 
not be loaded from the storage and the query results will be retrieved directly from the index.  
This can increase query performance at the cost of disk space used.  
{INFO/}

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Querying

- [Projections and Stored Fields](../indexes/querying/projections#projections-and-stored-fields)
