# Querying : How to work with suggestions?

Session `Query` method contains extensions (`Suggest`) that allow you to use suggestion feature.

## Syntax

{CODE suggest_1@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

**Parameters**   

query
:   Type: [SuggestionQuery](../../../glossary/client-api/querying/suggestion-query)  
A suggestion query definition containing all information required to query a specified index.

**Return Value**

Type: [SuggestionQueryResult]()  
Result containing array of all suggestions for executed query.

## Example

{CODE suggest_2@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

#### Related articles

TODO