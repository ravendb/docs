# Custom Analyzers
---

{NOTE: }

* Analyzers are used by indexes to divide (tokenize) the indexed document fields into searchable 
tokens to allow a full-text search. Learn more at [Creating Custom Analyzers](../../../indexes/using-analyzers#creating-custom-analyzers).  

* Custom analyzers can be defined at either the **database** level, where they can only be used by 
indexes of that database, or at the **server-wide** level, where they can be used by all databases 
in the entire cluster.  

* If a database custom analyzer and a server-wide custom analyzer share the same name, the database 
custom analyzer will be the one used by the indexes of that database.  

* Once defined, the custom analyzer is added to the analyzer list, along with the Lucene analyzers 
that come with RavenDB out of the box. It can then be selected when configuring the 
[index field options](../../../studio/database/indexes/create-map-index#index-field-options).  

* These views allow you to write your own custom analyzers:  
  * [Database Custom Analyzer View](../../../studio/database/settings/custom-analyzers#database-custom-analyzer-view)  
  * [Server-Wide Custom Analyzer View](../../../studio/database/settings/custom-analyzers#server-wide-custom-analyzer-view)  
  * [Edit Custom Analyzer View](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view)  

{NOTE/}

---

{PANEL: Database Custom Analyzer View}

![Figure 1. Database Custom Analyzer View](images/custom-analyzer-1.png "Figure 1. Database Custom Analyzer View")

{WARNING: }
1. Create a new database custom analyzer. Takes you to the analyzer editing view, see [below](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view).  
2. Edit an existing analyzer.  
3. Go to the Server-Wide Custom Analyzer View, see [below](../../../studio/database/settings/custom-analyzers#server-wide-custom-analyzer-view).  
{WARNING/}

{INFO: }
1. Indicates that a database custom analyzer has the same name as a server-wide custom analyzer.  
The database custom analyzer will be the one used by indexes of this database.  
{INFO/}

{PANEL/}

{PANEL: Server-Wide Custom Analyzer View}

![Figure 2. Server-Wide Custom Analyzer View](images/custom-analyzer-2.png "Figure 2. Server-Wide Custom Analyzer View")

{WARNING: }
1. Create a new server-wide custom analyzer. Takes you to the analyzer editing view, see 
[below](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view).  
2. Edit an existing analyzer.  
{WARNING/}

{PANEL/}

{PANEL: Edit Custom Analyzer View}

![Figure 3. Edit Custom Analyzer View](images/custom-analyzer-3.png "Figure 3. Edit Custom Analyzer View")

{WARNING: }
1. The name of your custom analyzer. This _must_ be the same as the class name of your analyzer as it appears 
in your code. This cannot be changed afterwards.  
2. The code of your analyzer. Must be compilable and include all necessary `using` statements. Learn more at 
[Indexes: Analyzers](../../../indexes/using-analyzers#creating-custom-analyzers).  
3. Load an existing analyzer `*.cs`.  
4. Save your analyzer.  
{WARNING/}

{PANEL/}

## Related Articles

### Indexes
- [Indexes: Analyzers](../../../indexes/using-analyzers)  

### Studio
- [Create Map Index](../../../studio/database/indexes/create-map-index)  
