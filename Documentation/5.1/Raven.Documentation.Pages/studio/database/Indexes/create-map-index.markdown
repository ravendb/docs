﻿# Create Map Index
---

{NOTE: }

* A **Map index** consists of one or more LINQ-based mapping functions that indicate how to index selected document fields.  

* In this page:  
  * [Edit Index View](../../../studio/database/indexes/create-map-index#edit-index-view)  
  * [Index Fields & Terms](../../../studio/database/indexes/create-map-index#index-fields-&-terms)  
  * [Index Field Options](../../../studio/database/indexes/create-map-index#index-field-options)  
  * [Configuration](../../../studio/database/indexes/create-map-index#configuration)  
  * [Additional Sources](../../../studio/database/indexes/create-map-index#additional-sources)  
  * [Spatial Field Options](../../../studio/database/indexes/create-map-index#spatial-field-options)  
{NOTE/}

---

{PANEL: Edit Index View}

![Figure 1. Edit Index View](images/create-map-index-1.png "Figure-1: Edit Index View")

1. **Index Name** - An index name can be composed of letters, digits, `.`, `/`, `-`, and `_`. The name must be unique in the scope of the database.  
   * Uniqueness is evaluated in a _case-insensitive_ way - you can't create indexes named both `usersbyname` and `UsersByName`.  
   * The characters `_` and `/` are treated as equivalent - you can't create indexes named both `users/byname` and `users_byname`.  
   * If the index name contains the character `.`, it must have some other character on _both sides_ to be valid. `/./` is a valid index name, but 
   `./`, `/.`, and `/../` are all invalid.  

2. **Save** - Save index definition  
   **Clone** - Clone this index (available for an already saved index)  
   **Delete** - Delete this index (available for an already saved index)  

3. Options avaliable for an already saved index:  
   **Copy C#** - Click to view and copy the C# class that defines the index as set in the Studio.  
   **Query** - Click to go the the Query View and query this index.  
   **Terms** - Click to see the index terms, see [below](../../../studio/database/indexes/create-map-index#index-entries-&-terms).  

4. **The Map Function** of the index.  

  * In the above example, the index will go over documents from the `Products` collection and 
    will only index documents whose `Discontinued` property is _'true'_.  

  * Note: The range of documents from which the query on this index will supply results will be on only Products collection documents 
    that have _'true'_ in the _'Discontinued'_ document field property. If the index was defined without the _'where'_ clause, but only using _'from p in docs.Products'_, 
    then the documents range for the query result would have been the whole Products collection.  

  * Each **Index Entry** that will be created for this index will be composed of the following 4 fields:  
    `Name`, `Category`, `PricePerUnit` & `SupplierName`.  
    The first 3 are taken directly from the document fields, while _SupplierName_ is a calculated index field.  
    The supplier's name is derived from the _Name_ field that is taken from the _loaded_ supplier document.  

  * At **query time**, when querying this index, the resulting documents can be searched on and further filtered  
    by these **Index Fields** defined and by the created **Terms**. 
    See the query results on this index in Query View.  

  * See more Map-Indexes examples in [Map Index defined from Code](../../../indexes/map-indexes)  
{PANEL/}

{PANEL: Index Fields & Terms}

![Figure 2. Index Fields & Terms](images/create-map-index-2.png "Figure-2: Index Fields & Terms")

1. **Index Fields**  
   The index-fields that are indexed per index-entry with the above index-definition are:  
   `Name`, `Category`, `PricePerUnit` & `SupplierName`.  

2. **Terms**  
   The terms are listed under each field.  
   The terms are created from the value of the field that was requested to be indexed 
   according to the specified [Field Options](../../../studio/database/indexes/create-map-index#fields-(index-entries)-options).  
{PANEL/}

{PANEL: Index Field Options}

![Figure 3a. Index Field Options](images/create-map-index-3.png "Figure-3a: Index Field Options")

1. **Add a field**  
   Create indexing options for one document field in the collection this index applies to.  

2. **Add default field options**  
   Set default index field options for all indexed fields. [See below](../../../studio/database/indexes/create-map-index#default-index-field-options).  

3. **Select Field**  
   Select a field from the drop-down. The options for this will override the default options.  

4. **Advanced**  
   Set advanced indexing options for the selected field.  

5. * `Store` - Setting _'Store'_ will store the value of this field in the index itself.  
               At query time, if _'Store'_ is set, then the value is fetched directly from the index, instead of from the original document.  
               If the field value is not stored in the index then it will be fetched from the document.  
               Storing data in the index will increase the index size.  
               Learn more in [Storing Data in Index](../../../indexes/storing-data-in-index).  

  *  `Full-Text-Search` - Set this to _'Yes'_ to allow searching for strings inside the text values of this field.  
                          The terms that are indexed are _tokens_ that are split from the original string according to the specified [Analyzer](../../../indexes/using-analyzers).  
                          The Analyzer is set in the _'Indexing'_ dropdown. The default analyzer is a simple case-insensitive analyzer.  

  * `Highlighting` - Set to _'Yes'_ to enable [Highlighting](../../../indexes/querying/highlighting). Requires 
                     Storage to be set to 'Yes'. In the advanced options Indexing needs to be set to 'Search' and 
                     Term Vector set to 'WithPositionsAndOffsets'.  

  * `Suggestions` -  Setting _'Suggestions'_ will allow you to query what the user probably meant to ask about. i.e. spelling errors.  
                      Learn more in this [Blog Post](https://ayende.com/blog/180899/queries-in-ravendb-i-suggest-you-can-do-better), 
                      and in [Querying: Suggestions](../../../indexes/querying/suggestions).  

  * `Spatial` -  See [below](../../../studio/database/indexes/create-map-index#spatial-field-options)

#### Advanced Index Field Options:  

![Figure 3c. Advanced Index Field Options](images/create-map-index-advanced.png "Figure-3c: Advanced Index Field Options")

  *  `Term Vector` -  Term Vectors are used in RavenDB's query feature [More Like This](../../../indexes/querying/morelikethis), 
                      which suggests documents that are similar to a selected document, based on shared indexed terms. i.e. suggest similar catalogs.  
                      A _'Term Vector'_ for a text paragraph will contain a list of all unique words and how often they appeared.  
                      Set _'full-text-search'_ on the field (index entry) and define it to have a _'Term Vector'_.  
                      Learn more in [Indexes: Term Vectors](../../../indexes/using-term-vectors), 
                      and in this [Blog Post](https://ayende.com/blog/180900/queries-in-ravendb-gimme-more-like-this).  

  * `Indexing` -  This setting determines which [***Analyzer***](../../../indexes/using-analyzers) can be used:  
     * ***Exact*** - The _'Keyword Analyzer'_ is used. The text is not split into tokens, the entire value of the field is treated as one token.  
     * ***Default*** - The _'LowerCase Keyword Analyzer'_ is used. The text is not split into tokens. The text is converted to lower-case, and                     matches are case insensitive.  
     * ***Search*** - Select an analyzer to use from the dropdown menu. If you set Indexing to 'Search' and do not select an analyzer, the analyzer               is _'StandardAnalyzer'_ by default.  

#### Default Index Field Options:  

![Figure 3b. Default Index Field Options](images/create-map-index-default.png "Figure-3b: Default Index Field Options")

{PANEL/}

{PANEL: Configuration}

!["Setting Configuration via Studio"](images\configuration-setting-new-config.png "Setting Configuration via Studio")  

1. **Indexes Tab**  
   Click to see indexing options.
2. **List of Indexes**  
   Select to see the list of your current indexes.
   You can only configure static indexes, not auto-indexes. 
   Select the index for which you want to change the default settings.
3. **Configuration**  
   Scroll down and select the Configuration tab.
4. **Add customized indexing configuration**  
   Click to select configuration and change the value.  
5. **Indexing Configuration Key**  
   Paste or select the configuration key that you want to change.
6. **Value**  
   Enter the new value for this configuration.
   * **Click Save** at the top of the interface when finished.

{PANEL/}

{PANEL: Additional Sources}

![Figure 5. Additional Sources](images/create-map-index-5.png "Figure-5: Additional Sources")

* Use the Additional Sources feature to introduce additional classes and methods that can be used in the index definition.  
  This enables advanced scenarios since complex logic can be performed in the indexing process.  

* In the above example, file _'PeopleUtil.cs'_ was uploaded and method _'CalculatePersonEmail'_ is used to calculate the index entry _'SupplierEmail'_.  
{PANEL/}

{PANEL: Spatial Field Options}

![Figure 6. Spatial Field Options](images/create-map-index-6.png "Figure-6: Spatial Field Options")

* **Spatial Field**  
  Spatial searches allow you to search using geographical data.  
  In order to be able to do such searches, a _spatial index field_ has to be defined.  

* **CreateSpatialField()**  
  This method instructs RavenDB to use the provided longitude and latitude from the document field properties 
  and create the spatial field named _'Coordinates'_. 
  Spatial queries can then be made on the _'Coordinates'_ field.  

* **Spatial Type**  
  RavenDB supports both the _'Geography'_ and _'Cartesian'_ systems.  

* **Spatial Indexing Strategy**  
  _'Strategy'_ determines the format of the indexed term values.  
  The following indexing strategies are supported:  
  • Bounding box  
  • Geohash prefix tree  
  • Quad prefix tree  

* **Radius Units**  
  Set the units (miles, kilometers) to be used when querying with RQL _'spatial.circle'_.  
  Learn more about querying spatial fields in: [Querying: Spatial](../../../indexes/querying/spatial).  

* **Max Tree Level**  
  Control how precise the spatial queries are going to be.  

* **X & Y Min & Max Values**  
  Setting the min & max values for X & Y is relevant only for the _'Cartesian'_ system type.  

* Learn more about spatial indexes in: [Indexing: Spatial](../../../indexes/indexing-spatial-data).  
{PANEL/}

## Related Articles

### Indexes

- [Map Indexes](../../../indexes/map-indexes)
- [Multi-Map Indexes](../../../indexes/multi-map-indexes)
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio

- [Indexes: Overview](../../../studio/database/indexes/indexes-overview)
- [Index List View](../../../studio/database/indexes/indexes-list-view)
- [Create Multi-Map Index](../../../studio/database/indexes/create-multi-map-index)
- [Create Map-Reduce Index](../../../studio/database/indexes/create-map-reduce-index)

