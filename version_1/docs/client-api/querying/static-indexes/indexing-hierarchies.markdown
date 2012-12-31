# Indexing Hierarchical Data

One of the greatest advantages of a document database is that we have very few limits in how we structure our data. One very common such scenario is the usage of hierarchical data structures. The most trivial of them is the comment thread, such as the following document:

    {  //posts/123
      'Name': 'Hello Raven',
      'Comments': [
        {
          'Author': 'Ayende',
          'Text': '...',
          'Comments': [
            {
              'Author': 'Rahien',
              'Text': '...',
              "Comments": []
            }
          ]
        }
      ]
    }

While it is very easy to work with such a document in all respect, it does bring up an interesting question, how can we index all comments in the post?

The answer to that is that Raven contains built-in support for indexing hierarchies, we can define an index using the following syntax:

    from post in docs.Posts
    from comment in Hierarchy(post, "Comments") 
    select new { comment.Text }

This will index all the comments in the thread, regardless of their location in the hierarchy.
