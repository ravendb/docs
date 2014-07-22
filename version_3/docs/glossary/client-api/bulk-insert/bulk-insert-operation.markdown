# Glossary : BulkInsertOperation

{CODE bulk_insert_operation@Glossary\Glossary.cs /}

OnBeforeEntityInsert
:   Type: event   
`event` that will be raised before entity will be processed.

IsAborted
:   Type: property   
`property` indicates if operation has aborted.

Abort
:   Type: method   
`method` used to abort the operation.

OperationId
:   Type: property   
Unique operation Id.

Report
:   Type: event   
`event` that will be raised every time a batch has finished processing and after the whole operation.

Store
:   Type: method   
`method` used to store the entity, with optional `id` parameter to explicitly declare the entity identifier (will be generated automatically on client-side when overload without `id` will be used).

