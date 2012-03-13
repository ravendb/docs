#Dynamic Indexing Syntax

RavenDB supports both static indexing (explicitly defined by the user) and dynamic indexes, which are defined by the server to support ad hoc queries.

For the most part, you don't need to think about querying dynamic indexes, the RavenDB Client API has a Linq provider that will do that for you, but there are some advanced cases where it is important to understand that.

For the rest of this page, we will use the following document:

    {
      "Title": "RavenDB Indexing",
      "Author": { "Id": "users/ayende", "Name": "Ayende" },
      "Tags": ["Indexing", "AdHoc"],
      "Images": [
         { "Url": "/static/ayende-on-beach.jpg", "Title": "Ayende's on the Beach" },
         { "Url": "/static/arava.jpg", "Title": "Arava with a bone" },
      ]
    }

We will show the appropriate Linq query and the actual Lucene query generated for each example:

    // Simple property - linq
    from doc in docs
    where doc.Title == "RavenDB Indexing"
    select doc;

    // Simple property - Lucene
    Title:"RavenDB Indexing"

    // Nested property - linq
    from doc in docs
    where doc.Author.Name == "Ayende"
    select doc;

    // Nested property - lucene
    Author.Name:Ayende

    // Primitive list - linq
    from doc in docs
    where doc.Tags.Any( tag => tag == "Indexing")
    select doc;

    // Primitive list - lucene

    Tags,:Indexing

    // List of objects - linq
    from doc in docs
    where doc.Images.Any( img => img.Url == "/static/arava.jpg")
    select doc;

    // List of bojects - lucene

    Images,Url:"/static/arava.jpg"

For querying into nested objects, we use the dot operator, just like in C#. For querying into collections, we use the comma operator.
