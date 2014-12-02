# What are transformers?

The easiest explanation of Transformers is that they are  **LINQ-based server-side projection functions** with the ability to load external documents, including additional results, and even to make decisions based on passed parameters, and much more.

{INFO:Transformers and the session}
Because you are working with projections, and not directly with documents, 
they are _not_ tracked by the session and won't be saved in the database if you call `SaveChanges`.
{INFO/}

## Basic example

If we want to transform our query or load results, we need to perform the following steps:

- first we need to create a transformer. One way to create it is to use `AbstractTransformerCreationTask`, but there are other ways that you can read about [here](../transformers/creating-and-deploying).

{CODE transformers_1@Transformers/WhatAreTransformers.cs /}

- since transformers work server-side, next logical step would be to send it to the server. More information about how to deploy transformers can be found [here](../transformers/creating-and-deploying).

{CODE transformers_2@Transformers/WhatAreTransformers.cs /}

- now, while querying or even loading, we can project our results using appropriate transformer.

{CODE transformers_3@Transformers/WhatAreTransformers.cs /}

{CODE transformers_4@Transformers/WhatAreTransformers.cs /}

More examples with detailed descriptions can be found [here](../transformers/basic-transformations).

## Related articles

- [Creating and deploying transformers](../transformers/creating-and-deploying)
- [Basic transformations](../transformers/basic-transformations)
