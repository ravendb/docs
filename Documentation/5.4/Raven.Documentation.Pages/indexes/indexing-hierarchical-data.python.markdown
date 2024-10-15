# Indexing Hierarchical Data
---

{NOTE: }

* Use the `Recurse` method to traverse the layers of a hierarchical document and index its fields.

* In this Page:  
   * [Hierarchical data](../indexes/indexing-hierarchical-data#hierarchical-data)  
   * [Index hierarchical data](../indexes/indexing-hierarchical-data#index-hierarchical-data)  
   * [Query the index](../indexes/indexing-hierarchical-data#query-the-index)  

{NOTE/}

---

{PANEL: Hierarchical data}

One significant advantage of document databases is their tendency not to impose limits on data structuring.
**Hierarchical data structures** exemplify this quality well; for example, consider the commonly used comment thread, implemented using objects such as:

{CODE:python indexes_1@Indexes\IndexingHierarchicalData.py /}

Readers of a post created using the above `BlogPost` structure can add `BlogPostComment` entries to the post's _comments_ field,
and readers of these comments can reply with comments of their own, creating a recursive hierarchical structure. 

For example, the following document, `BlogPosts/1-A`, represents a blog post by John that contains multiple layers of comments from various authors.  

`BlogPosts/1-A`:  

{CODE-BLOCK:JSON}
{
    "Author": "John",
    "Title": "Post title..",
    "Text": "Post text..",
    "Comments": [
        {
            "Author": "Moon",
            "Text": "Comment text..",
            "Comments": [
                {
                    "Author": "Bob",
                    "Text": "Comment text.."
                },
                {
                    "Author": "Adel",
                    "Text": "Comment text..",
                    "Comments": {
                        "Author": "Moon",
                        "Text": "Comment text.."
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

{PANEL: Index hierarchical data}

To index the elements of a hierarchical structure like the one above, use RavenDB's `Recurse` method.

The sample index below shows how to use `Recurse` to traverse the comments in the post thread and index them by their authors.
We can then [query the index](../indexes/indexing-hierarchical-data#query-the-index) for all blog posts that contain comments by specific authors.

{CODE-TABS}
{CODE-TAB:python:AbstractIndexCreationTask indexes_2@Indexes\IndexingHierarchicalData.py /}
{CODE-TAB:python:Operation indexes_3@Indexes\IndexingHierarchicalData.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Query the index}

The index can be queried for all blog posts that contain comments made by specific authors.

**Query the index using code**:  

{CODE-TABS}
{CODE-TAB:python:Query indexes_4@Indexes\IndexingHierarchicalData.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "BlogPosts/ByCommentAuthor"
where Authors == "Moon"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

**Query the index using the Studio**:

  * Query the index from the Studio's [List of Indexes](../studio/database/indexes/indexes-list-view#indexes-list-view) view:  
     
      !["List of Indexes view"](images/list-of-indexes-view.png "List of Indexes view")

  * View the query results in the [Query](../studio/database/queries/query-view) view:  
    
      !["Query View"](images/query-view.png "Query view")
    
  * View the list of terms indexed by the `Recurse` method:

      !["Click to View Index Terms"](images/click-to-view-terms.png "Click to view index terms")

      !["Index Terms"](images/index-terms.png "Index terms")

{PANEL/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying 

- [Query Overview](../client-api/session/querying/how-to-query)
