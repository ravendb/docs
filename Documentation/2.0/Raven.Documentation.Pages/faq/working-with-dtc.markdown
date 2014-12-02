#Working with System.Transactions

fully supports System.Transactions as a durable resource manager. That means that you can write the following code and it will be fully transactional:

{CODE working_with_dtc_1@Faq/WorkingWithDtc.cs /}

This code will either save both documents, or neither of them.

That said, the way that System.Transactions is implemented leads to some interesting issues. Let us examine this code:

{CODE working_with_dtc_2@Faq/WorkingWithDtc.cs /}

What would you expect this code to produce? Probably you would expect this to output "Ayende". But as a matter of fact, "Oren" (the old value) will be outputted.

That is not a bug, actually. It is an implication of how System.Transactions work. When you dispose a completed transaction scope, the transaction doesn't actually commit. What happens is that the transaction commit process is started. Since this is a background process running in another thread, you are actually going to load the users/1 document before the commit is over, which means that you are going to read the committed value of "Oren" (vs. the uncommitted value "Ayende").

It may not be a bug, since everything works as it is designed to work, but it sure isn't clear what is going on. And it looks like a bug. RavenDB detects this situation and will inform you that the document that you have loaded has been modified by an uncommitted transaction (using 203 Non Authoritative Information as the HTTP response code and "Non-Authoritative-Information" metadata property), so you can decide what to do about it.

In practice, for most read-only scenarios, we can use the non authoritative value (since we don't want to show uncommitted data, we will show the committed data, even if it is currently being modified). But when we want to update the document, we can't really do that, since the document is being locked by another transaction. If we'll try, we will get a Conflict Exception (409 Conflict on the HTTP response code).

You can ask RavenDB to wait until the pending transaction fully commits, by setting AllowNonAuthoritativeInformation to false, like this:

{CODE working_with_dtc_3@Faq/WorkingWithDtc.cs /}

This code will print "Ayende".

If the transaction doesn't commit within 15 seconds, an exception is thrown. You can control the timeout by setting the NonAuthoritativeInformationTimeout property on the session.
