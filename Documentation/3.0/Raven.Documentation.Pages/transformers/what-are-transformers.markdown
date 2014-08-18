# What are transformers?

Transformers have been introduced in RavenDB 2.5 as a substitution to TransformResults feature available back then. The easiest explanation of Transformers is that there are a **LINQ-based server-side projection functions** with the ability to load external documents, include additional results, even make decisions based on passed parameters and many more.

## Basic example

If we want to transform our query or load results we need to perform few steps.

- first we need to create a transformer. One way to create it is to use `AbstractTransformerCreationTask`, but there other ways and you can read about them [here](../transformers/creating).

{CODE transformers_1@Transformers/WhatAreTransformers.cs /}

- since transformers work server-side next logic step would be to send it to the server. More information about how to deploy transformers can be found [here](../transformers/creating).

{CODE transformers_2@Transformers/WhatAreTransformers.cs /}

- now, when querying or even loading we can project our results using appropriate transformer.

{CODE transformers_3@Transformers/WhatAreTransformers.cs /}

{CODE transformers_4@Transformers/WhatAreTransformers.cs /}

More examples with detailed description can be found [here](../transformers/using).

## Related articles

- [Creating transformers](../transformers/creating)
- [Using transformers](../transformers/using)
