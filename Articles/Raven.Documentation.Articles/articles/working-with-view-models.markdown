# Working with View Models in RavenDB

As modern app developers, we tend to work with conglomerations of data cobbled together to display a UI. For example, you've got a web app that displays tasty recipes, and on that page you also want to display the author of the recipe, a list of ingredients and how much of each ingredient. Maybe comments from users on that recipe. We want pieces of data from different objects, and all that on a single UI page.

For tasks like this, we turn to view models. A view model is a single object that contains pieces of data from other objects. It contains just enough information to display a particular UI page.

In relational databases, the common way to create view models is to utilize multiple JOIN statements to piece together disparate data to compose a view model.

But with RavenDB, we're able to store complex object, even full object graphs. This opens up some options for efficiently creating and even storing view models.

In this article, we'll look at a new ways to work with view models in RavenDB. I'll also give some pragmatic advice on when to use each approach in RavenDB.

## The UI

We're building a web app that displays tasty recipes to hungry end users. When a user clicks a recipe, he's show the recipe details page:+1:

(((IMAGE here showing UI mockup)))

At first glance, we see several pieces of data making up this UI.

- recipe name
- ingredients list
- name and email of the recipe's creator
- comments on the recipe
- a list of tags to help users find the recipe

A naive implementation might query for each piece of data independently. This has the downside of multiple trips to the database and the overhead performance implications.

A better implementation makes a single call to the database to load all the data needed to display the page. The view model is the container for all these pieces of data. It might look something like this:+1:

(((Code here that shows RecipeViewModel.cs)))

How do we populate such a view model from distinct pieces of data

## How we've done it in the past

In a relational database, we tackle this problem using JOINs to piece together a view model on the fly:

(((Code here that shows a LINQ joining all our data)))

This works: we can create a view model on the fly.

However, there are some downsides to this approach.

- Performance: JOINs have a non-trivial impact on query times. So when your user wants the data, we're making him wait while we perform the join.
- DRY modeling: If we want to display Recipe details in a different context, such as a list of recipe details, we'd forced to repeat our piece-together-the-view-model query code.

Can we do better with RavenDB?

## Using .Include

Perhaps the easiest and most familiar way to piece together a view model is to use RavenDB's .Include.

(((Code showing .Include )))

In the above code, we tellmake a single remote call to the database and load the Recipe along with the associated data via the .Include calls.

Then, after the Recipe returns, we can call session.Load to fetch the already-loaded related objects from memory.

This is conceptually similar to a JOIN in relational databases. Many devs new to RavenDB default to this pattern out of familiarity.

One difference, however, is that thanks to RavenDB's storing our objects as JSON, rather than as table rows, we can store complex object graphs. This reduces the need for calling .Include; encapsulated objects can be stored as part of the object itself.

For example, logically speaking, .Ingredients should be encapsulated in a Recipe, but relational databases don't support encapsulation (that is to say, we can't store a list of ingredients inside a Recipe table). Relational databases would require we: split each Ingredient into an Ingredient table, with a foreign key back to the Recipe it belongs to. Then, when we query for a recipe details, we need to JOIN them together.

With Raven, we can skip this step and gain performance. Since .Ingredients should logically be encapsulated inside a Recipe, there is no need to split them out into their own domain roots, and thus we don't have to .Include them. Instead, they're stored and loaded with the Recipe itself.

RavenDB's .Include is an improvement over relational JOINs. But this is all from following the familiar JOIN/Include pattern that loads disparate domain roots together. Raven gives us additional tools that go above and beyond old JOINs. We discuss these below.

## Transformers

RavenDB provides a means to build reusable server-side projections. In RavenDB we call these Transformers.

You can think of transformers as a C# function that converts a document into some other document. In our case, we want to take a Recipe and project it into a RecipeViewModel. We'll write a Transformer that looks like this:

(((transformer code)))

In the above code, we're accepting a Recipe and spitting out a RecipeViewModel. We can call .LoadDocument to load related documents, like our .Comments and .Creator. And since Transformers are server-side, we're not making extra trips to the database.

Once we've defined our Transformer, we can easily query any Recipe and turn it into a RecipeViewModel.

(((code using transformer)))

This code is a bit cleaner than calling .Include and .Load as in the previous section.

Additionally, using Transformers enables us to keep DRY. If we need to query a list of RecipeViewModels, there's no repeated piece-together-the-view-model code:

(((code querying for multiple RecipeViewModel)))

## Storing view models

Developers accustomed to relational databases may be slow to consider this possibility, but with RavenDB we can actually store view models as-is.

It's certainly a different way of thinking; rather than merely storing a domain root, we're storing pieces of multiple objects. Instead of only storing models, we're also storing view models. 

(((code to store view model)))

This has some important trade-offs:

- Query times are faster. We don't need to load other documents to display our Recipe details UI page. A single call to the database with zero joins - it's a beautiful thing!
- Data duplication. We're now storing Recipes and RecipeViewModels. If an author changes his recipe, we may need to also update the RecipeViewModel. This shifts the cost from query times to write times, which may be preferrable in a read-heavy system.

To help with data duplication, one may also use RavenDB's .Changes to subscribe to changes to Recipes and automatically update the corresponding RecipeViewModel.

Storing view models is certainly not appropriate for every situation. But some apps, especially read-heavy apps with a priority on speed, might benefit from this option. I like that Raven gives us the freedom to do this if it makes sense for our apps.

## Last words...

In this article, we looked at using view models with RavenDB. 3 techniques are at our disposal:

- .Include: loads multiple related documents in a single query.
- Transformers: reusable server-side projections that transform Recipes to RecipeViewModels.
- Storing view models: allows faster read times at the expense of possibly duplicated data.

For quick and dirty scenarios and one-offs, using .Include is fine. It's the most common way of piecing together view models in my experience, and it's also familiar to devs with relational database experience.

Transformers are the next widely used. If you find yourself converting Recipe to RecipeViewModel multiple times in your code, use a Transformer. They're easy to write, typically small, and familiar to anyone with LINQ experience. Using them in your queries is a simple one-liner that keeps your query code clean and focused.

Storing view models is rarely used, in my experience, but it can come in handy for read-heavy apps or for UI pages that need to be blazing fast.