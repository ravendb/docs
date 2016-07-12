#Search view

This view allows running queries according to Lucene syntax and browse files that match specified criteria.

![Figure 1. Studio. Search view](images/search-view.png)  

##Built-in filters

The text box is used to define filtering criteria. The criteria has to be a valid Lucene query. You can search by 
user defined metadata or built-in one set by RavenFS. To get full list of default metadata records read [indexing](../indexing) article.

The studio has a few, built-in filters in order to help you to create the query that contains common conditions:

![Figure 2. Studio. Search view. Filters](images/search-view-filters.png)

##Reset index

Reset index will remove all indexing data from a server for a given index so the indexation can start from scratch for that index.

![Figure 3. Studio. Search view. Reset Index](images/search-view-reset-index.png)

##Example

Lets find all files in `pdfs` directory which names start with `nosql`:

![Figure 4. Studio. Search view. Example](images/search-view-example.png)

