# Customizing the Behavior of the Client API #
The RavenDB .NET Client API includes the following classes (interfaces):

- <em>IDocumentStore</em>
- <em>IDocumentSession</em>

After creating a document store as an instance of the `DocumentStore` class and opening a session (an instance of `IDocumentSession`) from the document store created, we need to access the properties of the `DocumentConvertion`	 object in order customize the behavior of the RavenDB Client API.

## DocumentConvention Properties ##
The `DocumentConvention` class includes the following properties, which users can use to modify the behavior of the RavenDB Client API:

- <em>ApplyReduceFunction</em>. Holds a delegate which, by default, references an internally defined method for obtaining reduce results. The method is called to ensure that the reduce function is run over the merged results in a sharded environment.
- <em>CustomizeJsonSerializer</em>. References a `JsonSerializer` object, which serializes objects into the JSON format, deserializes objects from it, and, in particular, obtains reduce results for anonymous objects. The `JsonSerializer` object defines how objects are encoded into JSON.
- <em>DefaultQueryingConsistency</em>. Gets or sets a value from the `ConsistencyOptions` enumeration, which defines consistency options for all queries. The default value is `MonotonicRead`, which ensures that older values from querying an index will not be seen after a newer value has been obtained.
- <em>DisableProfiling</em>. Gets or sets a Boolean value that indicates whether all profiling support will be disabled. The default value is `true`. 
- <em>DocumentKeyGenerator</em> and <em>AsyncDocumentKeyGenerator</em>. Gets or sets the document key generator (returned by `GenerateDocumentKey`).
- <em>EnlistInDistributedTransactions</em>. Gets or sets a Boolean value that indicates whether RavenDB will automatically enlist in distributed transactions. The default value is `true`.
- <em>FailoverBehavior</em>. Gets a value of the  `FailoverBehavior` enumeration, which defines options for handling failover in a replicated environment when the primary server cannot be reached. The default value is `AllowReadsFromSecondaries`, which allows reading from secondary servers, but immediately fails writing to them.
- <em>FindClrType</em>. Gets or sets the function for finding the CLR type in a document. The default function is specified by an internal constant.
- <em>FindClrTypeName</em>. Gets or sets the function for finding the CLR type name that will be stored in the document metadata from the CLR type. The default function is set internally and depends on the browsing environment.
- <em>FindFullDocumentKeyFromNonStringIdentifier</em>. Holds the name of the function for finding the full document key from a non-string identifier. The default value is the function `DefaultFindFullDocumentKeyFromNonStringIdentifier`, which finds the full document name assuming that the standard conventions are used to generate the document key.
- <em>FindIdentityProperty</em>. Gets or sets the function for finding the identity property.
- <em>FindIdentityPropertyNameFromEntityName</em>. Gets or sets the function for finding the identity property name from the entity name.
- <em>FindIdValuePartForValueTypeConversion</em>. Gets or sets the function for converting a string identifier into a value type, such as an integer or a GUID.
- <em>FindPropertyNameForDynamicIndex</em>. Gets or sets the function for finding the property name from the indexed document type, the indexed name, the current path, and the property.
- <em>FindPropertyNameForIndex</em>. Gets or sets the function for finding the property name from the indexed document type, the indexed name, the current path, and the property.
- <em>FindTypeTagName</em>. Gets or sets the function for finding the tag name for the specified type. The default value is `DefaultTypeTagName`, which gives the default tag name for the specified type.
- <em>IdentityPartsSeparator</em>. Gets or sets the  separator between identity parts that is used by the HiLo generators. The default separator is the slash (/).
- <em>IdentityTypeConvertors</em>. Gets or sets a list of the type converters that can be used to translate the document key (string) to the type that is used on the entity if the type is not already a string. The defualt list consists of `GuidConverter()`, `Int32Converter()`, and `Int64Converter()`.
- <em>JsonContractResolver</em>
- <em>MaxNumberOfRequestsPerSession</em>
- <em>ShouldCacheRequest</em>
- <em>TransformTypeTagNameToDocumentKeyPrefix</em>
- <em>UseParallelMultiGet</em>

The default values of most of these properties are set internally by the <code>DocumentConvertion</code> constructor.





