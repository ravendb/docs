#Indexing Sorting Options

Sorting in RavenDB's indexes is usually based on lexical sorting of the data. You can override this in one of several ways.

* For sorting using specific culture sorting rules, check out [RavenDB's collation support](http://old.ravendb.net/faq/collation).//"TODO: link to new site"
* If you want to sort it by dates, they are written to the index in a form that allow easy lexical sorting already, so you don't need to do anything.
* For sorting numbers, you have to explicitly specify what is the number type (long, short, double, float, etc).

You can specify the sort option when you create the index, like so:

    new IndexDefinition
    {
        Map = "from user in docs.Users select new { user.Age }",
        SortOptions = {{"Age", SortOptions.Short}},
    }

The index outlined above will allow sorting by value on the user's age (1, 2, 3, 11, etc). If we wouldn't specify this option, it would sort lexically (1, 11, 2, 3, etc).
