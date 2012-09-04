# Client API #
The RavenDB .NET Client API includes the following classes (interfaces):

- <em>IDocumentStore</em>
- <em>IDocumentSession</em>

After creating a document store as an instance of <code>IDocumentStore</code> and opening a session (an instance of <code>IDocumentSession</code>) from the document store created, we need to access the properties of the <code>DocumentConvertion</code> object in order customize the behavior of the RavenDB Client API.

## DocumentConvention Properties ##
Properties of the <code>DocumentConvention</code> class that users can use to modify the behavior of the RavenDB Client API:

- <em>ApplyReduceFunction</em>
- <em>CustomizeJsonSerializer</em>
- <em>DefaultQueryingConsistency</em>
- <em>DisableProfiling</em> (not in the <code>DocumentConvertion</code> constructor) 
- <em>DocumentKeyGenerator</em> (returned by <code>GenerateDocumentKey</code>)
- <em>EnlistInDistributedTransactions</em>
- <em>FailoverBehavior</em>
- <em>FindClrType</em>
- <em>FindClrTypeName</em>
- <em>FindFullDocumentKeyFromNonStringIdentifier</em>
- <em>FindIdentityProperty</em>
- <em>FindIdentityPropertyNameFromEntityName</em>
- <em>FindIdValuePartForValueTypeConversion</em>
- <em>FindPropertyNameForDynamicIndex</em>
- <em>FindPropertyNameForIndex</em>
- <em>FindTypeTagName</em>
- <em>IdentityPartsSeparator</em>
- <em>IdentityTypeConvertors</em>
- <em>JsonContractResolver</em>
- <em>MaxNumberOfRequestsPerSession</em>
- <em>ShouldCacheRequest</em>
- <em>TransformTypeTagNameToDocumentKeyPrefix</em>
- <em>UseParallelMultiGet</em>

The default values of most of these properties are set internally by the <code>DocumentConvertion</code> constructor.

