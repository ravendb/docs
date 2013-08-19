# Loading & Editing an existing document

## Simple load and edit

Each _document_ is stored as part of a *collection*, where a _collection_ is a set of documents sharing the same entity type.

Therefore, if you have the id of an existing document (for example the previously saved BlogPost entry), it can be loaded in the following manner:

{CODE editing_document_1@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

Changes can then be made to that object in the usual manner:

{CODE editing_document_2@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}
	
Flushing those changes to the document store is achieved in the usual way:

{CODE editing_document_3@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}
	
You don't have to neither call the `Store` method nor track any changes yourself. RavenDB will do all of that for you.
	
{NOTE The entire document is sent to the server with the Id set to the existing document value, this means that the existing document will be replaced in the document store with the new one. Whilst patching operations are possible with RavenDB, the client API by default will always just replace the entire document in its entirety. /}

## More loading options

### Non string identifier

We just have shown how to load a document by it's string key. What if a type of you entity's identifier is not a string? The API also exposes an overload of the `Load` method that takes 
a parameter which type is `ValueType`. By default RavenDB allows you to pass there the most common types of identifiers: integer or Guid.

If you have an entity like this:

{CODE editing_document_4@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

you can load it by specifing an integer value as the identifier:

{CODE editing_document_5@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

#### Converting to string identifier

Even if your identifier is a string, you can use the `Load` overload presented here. The client is aware of the ID generation conventions (<em>collectionName/number</em>), so you could load
the entity with key _blogposts/1_ by using the code:

{CODE editing_document_6@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

In this case, before the load request is sent, first the conventions are applied on the provided id in order to translate it internally to the real document id. More about conventions you will find [here](customizing-behavior).
With regards to this topic these ones are applicable:

* TransformTypeTagNameToDocumentKeyPrefix
* FindFullDocumentKeyFromNonStringIdentifier
* IdentityTypeConvertors
* IdentityPartsSeparator

### Loading multiple docs

Another overload of the `Load` method allows you to get multiple documents by specifying theirs ids:

{CODE editing_document_7@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

The same would work for non string identifiers of course:

{CODE editing_document_8@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

### Load by prefix and pattern

Another option to load multiple documents, this time only if theirs ids fulfill the specified criteria, is `LoadStartingWith` method. As you can guess you can specify a prefix of an identifier
to return only documents that match it. For example to get `BlogPost` entities that ID is of the form _blogposts/1*_ use the following code:

{CODE editing_document_9_0@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

In result you will get posts the following sample ids: _blogposts/1, blogposts/10, blogposts/1/Author/Matt_ etc.

Except from the prefix you are also able to pass the regular expression that the identifiers (after the prefix part) of returned docs have to match. Example:

{CODE editing_document_9_1@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}

Here we provided the additional criteria, that limits the results only to documents that IDs have the form _blogposts/1\*/Author/\*t_ for example: <em>blogposts/11/Author/Matt</em>.
In the matches parameter you can specify \* and ? as special regular expression characters.

Multiple criterias can be separated using `|`. E.g. If we want to add to our previous results documents with IDs that have the form _blogposts/1\*/Type/\*t_ for example: <em>blogposts/11/Type/Innovation</em> then following query needs to be executed:   

{CODE editing_document_9_2@ClientApi\BasicOperations\LoadingEditingExistingDocument.cs /}