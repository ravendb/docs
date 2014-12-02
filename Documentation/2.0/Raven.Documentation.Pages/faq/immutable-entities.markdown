#Immutable entities with Raven DB

Whenever you are trying to use a non-default constructor for your entity, do the following to avoid falling into serialization problems with JSON.Net and raven:

* add a private constructor ... even if it is empty
* Ensure every readonly property has a "Private setter" on it ... else it won't get populated
* Use a custom Json resolver like this one:

{CODE immutable_entities_1@Faq/ImmutableEntities.cs /}