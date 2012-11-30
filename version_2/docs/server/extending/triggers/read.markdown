#Read Triggers
Read triggers implement the AbstractReadTrigger interface and allow filtering, modifying and denying document reading and querying capabilities.
Like PUT and DELETE triggers, the read trigger need to handle two operations, allowing the read operation to proceed and modifying the document before it is read by the user.

![Figure 1: Triggers - Read](images\triggers_read_docs.png)

**Example: Information hiding**

{CODE read_1@Server\Extending\Triggers\Read.cs /}

In the example above, we only let the owner of a document to read it. You can see that a Read trigger can deny the read to the user (returning an error to the user) or ignoring the read (hiding the presence of the document. You can also make decisions based on whatever that specific document was requested, or if the document was read as part of a query.

**Example: Linking document on the server side**

{CODE read_2@Server\Extending\Triggers\Read.cs /} 

In this case, we detect that a document with a link was requested, and we stitch the document together with its link to create a single document.