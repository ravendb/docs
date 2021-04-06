# Custom Analyzers
---

{NOTE: }

* These views allow you to write your own custom [full-text search analyzers](../../../indexes/using-analyzers). 
Analyzers are used by indexes to divide text into searchable tokens. Learn more at 
[creating custom analyzers](../../../indexes/using-analyzers#creating-custom-analyzers).  

* Your custom analyzers are added to the list of analyzers along with the Lucene analyzers that 
come with RavenDB out of the box. You then choose from this list when you configure the 
[index field options](../../../studio/database/indexes/create-map-index#index-field-options).  

* Custom analyzers can be defined at the **database** level, where they can only be used by indexes 
of that database, or at the **server-wide** level, where they can be used by all databases in the 
server.  

* If a database analyzer and a server-wide analyzer have the same name, the database analyzer 
will be used by indexes of that database.  

* In this page:  
  * [Database Custom Analyzer View](../../../studio/database/settings/custom-analyzers#database-custom-analyzer-view)  
  * [Server-Wide Custom Analyzer View](../../../studio/database/settings/custom-analyzers#server-wide-custom-analyzer-view)  
  * [Edit Custom Analyzer View](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view)  

{NOTE/}

---

{PANEL: Database Custom Analyzer View}

![Figure 1. Database Custom Analyzer View](images/custom-analyzer-1.png "Figure 1. Database Custom Analyzer View")

{WARNING: }
1. Create a new custom analyzer. Takes you to the analyzer editing view, see [below](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view).  
2. Edit an existing analyzer.  
3. Go to the server-wide analyzers, see [below](../../../studio/database/settings/custom-analyzers#server-wide-custom-analyzer-view).  
{WARNING/}

{INFO: }
1. Indicates that a database analyzer has the same name as a server-wide analyzer. Indexes in this database use 
the database version of the analyzer.  
{INFO/}

{PANEL/}

{PANEL: Server-Wide Custom Analyzer View}

![Figure 2. Server-Wide Custom Analyzer View](images/custom-analyzer-2.png "Figure 2. Server-Wide Custom Analyzer View")

{WARNING: }
1. Create a new custom analyzer. Takes you to the analyzer editing view, see [below](../../../studio/database/settings/custom-analyzers#edit-custom-analyzer-view).  
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
3. Load an existing analyzer `.dll`.  
4. Save your analyzer.  
{WARNING/}

{PANEL/}

## Related Articles

### Indexes
- [Indexes: Analyzers](../../../indexes/using-analyzers)  

### Studio
- [Create Map Index](../../../studio/database/indexes/create-map-index)  
