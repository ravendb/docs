
### Loading & Editing an existing document

Each _document_ is stored as part of a *collection*, where a _collection_ is a set of documents sharing the same entity type.

Therefore, if you have the id of an existing document (for example the previously saved BlogPost entry), it can be loaded in the following manner:

	// blogposts/1 is entity of type BlogPost with Id of 1
	BlogPost existingBlogPost = session.Load<BlogPost>("blogposts/1");

Changes can then be made to that object in the usual manner:

	existingBlogPost.Title = "Some new title";

Flushing those changes to the document store is achieved in the usual way:

	session.SaveChanges();

You don't have to neither call the `Store` method nor track any changes yourself. RavenDB will do all of that for you.
	
{NOTE The entire document is sent to the server with the Id set to the existing document value, this means that the existing document will be replaced in the document store with the new one. Whilst patching operations are possible with RavenDB, the client API by default will always just replace the entire document in its entirety. /}
