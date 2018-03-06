# Using optimistic concurrency in real world scenarios

When we develop an application that will be used by different users at the same time, there comes a time where we need to think about how we will handle the fact that multiple users will be working on the same document. How can we make our application aware of document changes so we can take the appropriate action when multiple users wants to store the same document without knowledge of each others changes?

This can be achieved by using RavenDB’s optimistic concurrency option. Let's discuss two cases: one where we need only one session to load and store the document, and another one where we need two sessions.

We will use a simple scenario to explain these cases: users of our application can change the name of a person who is stored as a document in RavenDB .

{CODE concurrency_1@ClientApi\Concurrency\UsingOptimisticConcurrencyInRealWorldScenarios.cs /}

## Optimistic concurrency – Using one session ##

The easiest way to insure document changes aren't overwritten is when we only need one session to load and store our document. It's not hard to develop an application where this approach would suffice. In this case we just need to set `UseOptimisticConcurrency` to `true`, and RavenDB will handle all checks for us: we will receive a `ConcurrencyException` when the document we want to store was already changed by another user.

{CODE concurrency_2@ClientApi\Concurrency\UsingOptimisticConcurrencyInRealWorldScenarios.cs /}

Our `Person` document will be effectively locked in the scope of the session. How is this done by RavenDB?

The server holds a `change vector` string value, which is being updated on every change made to the database. Every time an action is performed on a document, it is being stamped with the current change vector value, just before that change vector changes. By comparing the actual and expected change vector of the document, RavenDB is able to tell us if a document was changed outside the session we are working with.

If you try to update a document and the change vectors don't match while `UseOptimisticConcurrency` is set to `true`, a `ConcurrencyException` will be thrown. The actual and expected change vector are exposed as properties in the class `ConcurrencyException`, and that should allow you to implement your own logic as of how to continue.

So, in this case the solution to handle document changes is nice and easy. But what if we need to return a document, going out of scope of the session, change it, and save it in another session?

## Optimistic concurrency – Using different sessions ##

Let’s say we want to develop a more "complicated" application, for example an ASP MVC application. In the controller's `Load` method a session is started which is used to load a `Person` document, map the object to its model representation, which is then sent back to the user as a `JsonResult`. The session is then disposed.

The controller has another method to store the `Person` document, which uses another session. Because we use another session to store the document, RavenDB can't automatically derive anymore what the change vector of the document was when it was loaded. So we will need to give that information.

Let's declare a new property to hold the change vector.

{CODE concurrency_3@ClientApi\Concurrency\UsingOptimisticConcurrencyInRealWorldScenarios.cs /}

The `JsonIgnore` attribute will make sure the ChangeVector property will not be stored. Now all we need to do is set the property when we `Load`, `Query` or `Store` the document:

{CODE concurrency_4@ClientApi\Concurrency\UsingOptimisticConcurrencyInRealWorldScenarios.cs /}

Remember the sessions used in the `Get`, `GetAll` and `Update` functions are different. Also notice we need to set `UseOptimisticConcurrency` to `true` on the session used in the `Update` function. If we don’t do this, no checks will be made, and the document will be saved when `SaveChanges` is called, overwriting previous changes. This behavior is used in RavenDb versions until stable build 888 *(current build when writing this article)*. In later builds, checks will be done automatically when you provide a change vector, so setting `UseOptimisticConcurrency` will not be needed anymore.

Setting the change vector like this has its flaws though: if you forget to set the change vector, no checks will be done. 

