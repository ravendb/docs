#Sorting

Indexed values are sorted lexicographically, but sorting is not applied until you request it by using one of the appropriate methods:

##OrderBy

To get ordered results we need to indicate a field name to sort by:

{CODE sorting_1@FileSystem\ClientApi\Session\Querying\Sorting.cs /}

##OrderByDescending

You can also retrieve results in descending order:

{CODE sorting_2@FileSystem\ClientApi\Session\Querying\Sorting.cs /}