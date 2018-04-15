# Numbers in RavenDB
---

{NOTE: }

Rational numbers are infinitely big and infinitely small, in computer science we are able to "model" the universe with a limited degree of accuracy and range.  
RavenDB is not an exception and has it's own boundaries of accuracy and range, which match most common cases of any business application.  
This article is planned for users who know that they are going to use uncommon numeric ranges. It will cover the way RavenDB's Server approaches numeric values and what level of range and accuracy can be expected from each of it's mechanisms.  
RavenDB's Client support of numbers depends on platform and chosen deserialization.  

In this page:  
* [About numbers](../../server/kb/numbers-in-ravendb#about-numbers)  
* [Numbers in documents](../../server/kb/numbers-in-ravendb#numbers-in-documents)  
* [Numbers in Javascript](../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)  
* [Numbers in indexes and queries](../../server/kb/numbers-in-ravendb#numbers-in-indexes-and-queries)  
* [Numbers in studio](../../server/kb/numbers-in-ravendb#numbers-in-management-studio)  
* [Numbers in client API](server/kb/numbers-in-ravendb#numbers-in-client-api)  


{NOTE/}

---

{PANEL: About numbers}
Although real numbers has no limits in size or percision, in computing there are limitations.  
Simplest type of numbers known to computers are integers. RavenDB fully supports integers of `int` type between [-2,147,483,648 to 2,147,483,647].  
Simplest type of fraction known to computers are floating point numbers. RavenDB fully supports double percision floating point number with approximate range of 15-16 digits between [±5.0 × 10^(−324) to ±1.7 × 10^308].  

RavenDB supports storing numbers in the range of the `double` type described above. RavenDB supports storing number in any percision, but it's indexing and javascript mechanisms are limited to the 16 digits percision of `double` numbers.  
In order to better understand the terms percision and range, please observe the next diagram, comparing range and percision of 3 common .net types: `long`, `double` and `decimal`:

![Percision and range in numeric types](images/NumberTypesPercisions.png)  

Numbers bigger then double percision max number will be rejected by server. Mechanisms supporting only double percision numbers will by default round the number to a `double`, loosing percision.  
Please follow the next paragraphs to learn more about those limitaions and possible work arounds.

{WARNING: Important edge cases}
Please note that `long`'s max and min numbers are beyond the accuracy range of `double`, therefore, it should be used with care, and it's recommended to avoid using it for global maximum or minimum notations.
{WARNING /}

{PANEL/}

{INFO: Examples in the page}
Examples in this page are based on the `InterstellarTrip` entity, describing an intergallactical journey of a brave pioneer:  
{CODE interstellar_trip@Server\ScalarToRawString.cs /}
{INFO/}

{PANEL: Numbers in documents}
Numbers in documents represented by either:  
 * `long` for integers in the `long` range (-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807)  
 *  `LazyNumberValue` for all the other numbers, including other integers and floating point numbers. `LazyNumberValue` Wraps a string representation of a number.  
  
RavenDB server will accept document with numbers in the range of 'double' with any percision.
{PANEL/}

{PANEL: Numbers in javascript engine}
RavenDB uses javascript in many mechanisms: projections in queries, subscriptions, ETL processes and more.  
The only type of numbers supported by javascript is double percision floating point number, and `Jint`, the javascript engine RavenDB uses is no exception.  
Percision of any number that is outside of the percision range of `double` will be rounded to the digits amount of a double.  
RavenDB provides a way to receive the original value, before the cast to double. The only limitation is that it won't be possible to treat it as a number, but you will be able to receive a string representation of the value.  
The way to do that is using the `scalarToRawString` extension method, example:  

{CODE query_with_big_number_projection@Server\ScalarToRawString.cs /}

{PANEL/}

{PANEL: Numbers in indexes and queries}  

RavenDB's indexes supports either integers in the `long` range, or fractions in the `double` range.  
Integers outside of the `long` boundaries will be treated and `double` and therefore their percision will be rounded to `double`'s.  
Because of that, those 'accuracy-rounded' numbers indexed value won't be equal to the original value, therefore, queries may not return the expected results.  
In order to overcome that, it is recommended to treat those values as strings. The implication is that it will be possible to perform only string related queries and not numeric ones.  

{INFO: Static indexes}
The way to treat numbers that exceed the percision level of `long` or `double` in static indexes, the string representation of the value should be indexed, It will use the raw value, rather the rounded one. 
{CODE interstellar_static_index@Server\ScalarToRawString.cs /}
{INFO /}

{INFO: Query}
Query work as expected with integers in the 'long' range and fractions in the `double` range and percision.  
In order to query numbers outside of that range, query the the field using the string representation of the value, whether if using an index or a collection query.
{CODE query_with_big_number_projection@Server\ScalarToRawString.cs /}

Note that alphanumeric sorting of string representation of numbers is accurate only with integers. Alphanumeric sorting of fractions and numbers with exponent notations will not analyze the value as a number.
{INFO /}

{PANEL/}

{PANEL: Numbers in management studio}
The management studio treats documents as javascript objects, therefore, it treats it's numbers as a javascript number, which is always a `double`.  
Note that editing documents with numeric data outside of the percision range of double will end up with rounding those numbers to a proper `double` and unintentional modification of those fields.  
{PANEL/}

{PANEL: Numbers in client API}
Numbers in RavenDB clients depend on the limitations of the platforms and the serialization/deserialization mechanisms. See articles in the desired languages:

[Number (de)serialization](../../client-api/configuration/serialization#working-with-numbers)

{PANEL/}

## Related Articles

- [Javascript engine](../../server/kb/javascript-eingine)
- [Indexing basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
