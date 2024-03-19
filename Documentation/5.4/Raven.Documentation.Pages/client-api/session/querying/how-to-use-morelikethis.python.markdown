# Using MoreLikeThis

`more_like_this` is available through query extension methods, and will return similar documents according 
to the provided criteria and options.

## Syntax

{CODE:python more_like_this_1@ClientApi\Session\Querying\MoreLikeThis.py /}

| `more_like_this_or_builder` parameter | Description |
| ------------- | ------------- |
| `MoreLikeThisBase` | Defines the type of MoreLikeThis that should be executed |
| `Callable[[MoreLikeThisBuilder[_T]], None]]` | Builder with fluent API that constructs the `MoreLikeThisBase` instance |

### Builder

{CODE:python more_like_this_3@ClientApi\Session\Querying\MoreLikeThis.py /}

| Builder method | Parameter | Type | Description |
| ------------- | ------------- | ----- | ----- |
| **using_any_document** |  |  |  |
| **using_document** | `document_json_or_builder` (Union) | `str` | Inline JSON document that will be used for the operation |
| **using_document** | `document_json_or_builder` (Union) | `Callable[[DocumentQuery[_T]], None]` | Filtering expression to find a document that will be used for the operation |
| **with_options** | `options` | `MoreLikeThisOptions` | Non-default options to be used by the operation |

### Options

{CODE:python more_like_this_2@ClientApi\Session\Querying\MoreLikeThis.py /}

| Options | | |
| ------------- | ------------- | ----- |
| **minimum_term_frequency** | int | Ignores terms with less than this frequency in the source doc |
| **maximum_query_terms** | int | Returns a query with no more than this many terms |
| **maximum_number_of_tokens_parsed** | int | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **minimum_word_length** | int | Ignores words less than this length or, if 0, then this has no effect |
| **maximum_word_length** | int | Ignores words greater than this length or if 0 then this has no effect |
| **minimum_document_frequency** | int | Ignores words which do not occur in at least this many documents |
| **maximum_document_frequency** | int | Ignores words which occur in more than this many documents |
| **maximum_document_frequency_percentage** | int | Ignores words which occur in more than this percentage of documents |
| **boost** | bool | Boost terms in query based on score |
| **boost_factor** | float |  Boost factor when boosting based on score |
| **stop_words_document_id** | str  | Document ID containing custom stop words |
| **Fields** | List[str] | Fields to compare |

## Example I

{CODE-TABS}
{CODE-TAB:python:Query more_like_this_4@ClientApi\Session\Querying\MoreLikeThis.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "Body" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:python:Query more_like_this_6@ClientApi\Session\Querying\MoreLikeThis.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "Body" ] }') and Category == 'IT'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [MoreLikeThis](../../../indexes/querying/morelikethis)
