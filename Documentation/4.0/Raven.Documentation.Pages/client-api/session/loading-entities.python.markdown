# Session: Loading entities

The `load` method allow user to download a documents from database and convert them to entities.

## Syntax

{CODE:python loading_entities_1@ClientApi\Session\LoadingEntities.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key_or_keys** | string or list | Identifier of a document\s that will be loaded. |
| **object_type** | classObj | The class we want to get|
| **includes** | list or str | The path to a reference inside the loaded documents can be list (property name) |
| **nested_object_types** | dict {str : classObj} | A dict of classes for nested object the key will be the name of the class and the value will be the object we want to get for that attribute. |

| Return Value | |
| ------------- | ----- |
| object_type or None | instance of `object_type` or `None` if document with given Id does not exist. (If the object_type equal to None return `DynamicStructure` entity) |


### Example I

{CODE:python loading_entities_2@ClientApi\Session\LoadingEntities.py /}

In this example the load will return `DynamicStructure` entity because we don't specify any classObj in object_type

### Example II
A load with **object_type**:

{CODE:python loading_entities_3@ClientApi\Session\LoadingEntities.py /}

{WARNING:Important}

{CODE:python loading_entities_3.1@ClientApi\Session\LoadingEntities.py /}

If the class variables names are different from the method signature name. TypeError exception will be raise

{WARNING/}


{PANEL:load - multiple entities}

To load multiple entities at once use the `load` just pass to **key_or_keys** a list of keys.

{CODE:python loading_entities_4@ClientApi\Session\LoadingEntities.py /}

In this example the `load` method will return a list with the specific demanded documents

{NOTE:Note}
If some document couldn't be loaded we will put None in the list
{NOTE/}

{PANEL/}

{PANEL:load with includes}

When there is a 'relationship' between documents, then those documents can be loaded in a single request call using `load`.

{CODE:python loading_entities_5@ClientApi\Session\LoadingEntities.py /}

The return value will be the same but next time we will try to load an include document we will get the document without doing any requests to the server

{PANEL/}


{PANEL:load with nested object types}

Sometimes we will use some objects that have nested object with `load` you can specify your nested object type and try to convert those to the right type.

{CODE:python loading_entities_6@ClientApi\Session\LoadingEntities.py /}

{PANEL/}
## Related articles

- [Opening a session](./opening-a-session)  
