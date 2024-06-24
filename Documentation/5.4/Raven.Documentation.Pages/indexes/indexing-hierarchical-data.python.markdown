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

One of the significant advantages offered by document databases is their tendency not to force 
limits upon data structuring. **Hierarchical data structures** demonstrate this quality beautifully: 
take, for example, the commonly-used **Comment thread**, implemented using objects such as:  
{CODE:python indexes_1@Indexes\IndexingHierarchicalData.py /}

Readers of a post created using the above `BlogPost` structure, can add `BlogPostComment` comments 
to its Comments field. And readers of these comments can reply with comments of their own, creating 
a recursive hierarchical structure.  

`BlogPosts/1-A`, for example, is a blog entry posted by John, that contains several layers of 
comments left by various authors.  

`BlogPosts/1-A`:
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

To index the elements of a hierarchical structure like the one demonstrated above, 
use RavenDB's `Recurse` method.  

In the sample below, we use `Recurse` to go through comments in the post thread 
and index them by their authors.  
{CODE-TABS}
{CODE-TAB:python:AbstractIndexCreationTask indexes_2@Indexes\IndexingHierarchicalData.py /}
{CODE-TAB:python:Operation indexes_3@Indexes\IndexingHierarchicalData.py /}
{CODE-TABS/}

---

### Querying the created index

* The index we created can be queried using code.  
  {CODE-TABS}
  {CODE-TAB:python:Query indexes_4@Indexes\IndexingHierarchicalData.py /}
  {CODE-TABS/}

* The index can also be queried using Studio.  

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

- [Query Overview](../client-api/session/querying/how-to-query)
