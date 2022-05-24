# Indexes: Indexing Hierarchical Data

---
{NOTE: }

Use the indexing `Recurse` method to recurse through the layers of a hierarchical document 
and index its elements.  

* In this Page:  
   * [Hierarchical Data](../indexes/indexing-hierarchical-data#hierarchical-data)  
   * [Indexing Hierarchical Data](../indexes/indexing-hierarchical-data#indexing-hierarchical-data)  

{NOTE/}

---

{PANEL: Hierarchical Data}

One of the greatest advantages of document databases is the very few limits they set on 
data structuring.  
**Hierarchical data structures** demonstrate this advantage beautifully; take, for instance, 
the commonly-used **Comment thread**, as implemented using objects such as these:  
{CODE indexes_1@Indexes\IndexingHierarchicalData.cs /}

Readers of a post created using the `BlogPost` structure, can add `BlogPostComment` comments to 
its Comments field.  
And readers of the comments left by post readers, can reply to them with comments of their own, 
creating a recursive hierarchical structure.  

Take `BlogPosts/1-A`, for example - a blog entry posted by John that contains several layers of 
comments left by various authors:  

{CODE-BLOCK:JSON}
{
    "Author ": "John",
    "Comments": [
        {
            "Author": "Moon",
            "Comments": [
                {
                    "Author": "Bob"
                },
                {
                    "Author": "Adel",
                    "Comments": {
                        "Author": "Moon"
                    }
                }
            ]
        }
    ],
    "@metadata": {
        "@collection": "BlogPosts"
    }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Indexing Hierarchical Data}

To **index** elements of a hierarchical structure like the one demonstrated above, 
we can use RavenDB's `Recurse` method.  

In the sample below, we use `Recurse` to recurse through comments in the post thread 
and index the comments by the authors that left them.  
{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexes_2@Indexes\IndexingHierarchicalData.cs /}
{CODE-TAB:csharp:Operation indexes_3@Indexes\IndexingHierarchicalData.cs /}
{CODE-TAB:csharp:JavaScript indexes_3@Indexes\JavaScript.cs /}
{CODE-TABS/}

---

### Querying the created index

The index we created can be queried using the API or via Studio.  

* **Query the index using the API**  
  In the code below, we run the index we created.  
  {CODE-TABS}
  {CODE-TAB:csharp:Query indexes_4@Indexes\IndexingHierarchicalData.cs /}
  {CODE-TAB:csharp:DocumentQuery indexes_5@Indexes\IndexingHierarchicalData.cs /}
  {CODE-TABS/}

* **Query the index via Studio**

   * Use Studio's [List of Indexes](../studio/database/indexes/indexes-list-view#indexes-list-view) 
     view to define and query the index.  
     
         !["List of Indexes view"](images/list-of-indexes-view.png "List of Indexes view")

   * Use the **Query** view to see the results and the list of [terms](../studio/database/indexes/indexes-list-view#indexes-list-view---actions) 
     indexed by the `Recurse` method.  
     
         !["Query View"](images/query-view.png "Query View")

         !["Click to View Index Terms"](images/click-to-view-terms.png "Click to View Index Terms")

         !["Index Terms"](images/index-terms.png "Index Terms")

{PANEL/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying 

- [Basics](../indexes/querying/basics)
