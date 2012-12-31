#RavenDB Authorization Bundle Design

I used to be able to just sit down and write some code, and eventually things would work. Just In Time Design. That is how I wrote things like Rhino Mocks, for example.

Several years ago (2007, [to be exact](http://ayende.com/blog/2958/a-vision-of-enterprise-platform-security-infrastructure)) I started doing more detailed upfront design, those designs aren't curved in stone, but they are helpful in setting everything in motion properly. Of course, in some cases those [design need a lot of time to percolate](http://ayende.com/blog/3897/designing-a-document-database). At any rate, this is the design for the Authorization Bundle for [RavenDB](http://ravendb.net/). I would welcome any comments about it. I gave some background on some of the guiding thoughts about the subject in [this post](http://ayende.com/blog/4559/real-world-authorization-implementation-considerations).

    Note: This design is written before the code, it reflect the general principles of how I intend to approach the problem, but it is not a binding design, things will change.

Rhino Security design has affected the design of this system heavily. In essence, this is a port (of a sort) of Rhino Security to RavenDB, with the necessary changes to make the move to a NoSQL database. I am pretty happy with the design and I actually think that we might do back porting to Rhino Security at some point.

##Important Assumptions
The most important assumption that we make for the first version is that we can trust the client not to lie about whose user it is executing a certain operation. That one assumes the following deployment scenario:

![Figure 1: Authorization Bundle](/images/authorization_bundle_faq.png)

In other words, only the application server can talk to the RavenDB server and the application server is running trusted code.

To be clear, this design doesn't not apply if users can connect directly to the database and lie about who they are. However, that scenario is expected to crop up, even though it is out of scope for the current version. Our design need to be future proofed in that regard.

##Context & User
Since we can trust the client calling us, we can rely on the client to tell us which user a particular action is executed on behalf of, and what is the context of the operation.

From the client API perspective, we are talking about:

    using(var session = documentStore.OpenSession())
    {
         session.SecureFor("raven/authorization/users/8458", "/Operations/Debt/Finalize");
        
        var debtsQuery =   from debt in session.Query<Debt>("Debts/ByDepartment")
                           where debt.Department == department
                           select debt
                           orderby debt.Amount;
        
         var debts = debtsQuery.Take(25).ToList();

        // do something with the debts
    }

I am not really happy with this API, but I think it would do for now. There are a couple of things to note with regards to this API:

* The user specified is using the reserved namespace "raven/". This allows the authorization bundle to have a well known format for the users documents.
* The operation specified is using the Rhino Security conventions for operations. By using this format, we can easily construct hierarchical permissions.

##Defining Users
The format of the authorization user document is as follows:

    // doc id /raven/authorization/users/2929
    {
        "Name": "Ayende Rahien",
        "Roles": [ "/Administrators", "/DebtAgents/Managers"],
        "Permissions": [
           { "Operation": "/Operations/Debts/Finalize", "Tag": "/Tags/Debts/High", "Allow": true, "Priority": 1, }
        ]
    }

There are several things to note here:

* The format isn't what an application needs for a User document. This entry is meant for the authorization bundle's use, not for an application's use. You can use the same format for both, of course, by extending the authorization user document, but I'll ignore this for now.
* Note that the Roles that we have are hierarchical as well. This is important, since we would use that when defining permissions. Beyond that, Roles are used in a similar manner to groups in something like Active Directory. And the hierarchical format allows to manage that sort of hierarchical grouping inside Raven easily.
* Note that we can also define permissions on the user for documents that are tagged with a particular tag. This is important if we want to grant a specific user permission for a group of documents.

##Roles
The main function of roles is to define permissions for a set of tagged documents. A role document will look like this:

    // doc id /raven/authorization/roles/DebtAgents/Managers

    {
       "Permissions": [
           { "Operation": "/Operations/Debts/Finalize", "Tag": "/Tags/Debts/High", "Allow": true, "Priority": 1, }
        ]
    }

##Defining permissions
Permissions are defined on individual documents, using RavenDB's metadata feature. Here is an example of one such document, with the authorization metadata:

    //docid-/debts/2931
    {
      "@metadata": {
        "Authorization": {
          "Tags": [
            "/Tags/Debts/High"
          ],
          "Permissions": [
            {
              "User": "raven/authorization/users/2929",
              "Operation": "/Operations/Debts",
              "Allow": true,
              "Priority": 3
            },
            {
              "User": "raven/authorization/roles/DebtsAgents/Managers",
              "Operation": "/Operations/Debts",
              "Allow": false,
              "Priority": 1
            }
          ]
        }
      },
      "Amount": 301581.92,
      "Debtor": {
        "Name": "Samuel Byrom",
        "Id": "debots/82985"
      }
      //more document data
    }

Tags, operations and roles are hierarchical. But the way they work is quite different.

* For Tags and Operations, having permission for "/Debts" gives you permission to "/Debts/Finalize".
* For roles, it is the other way around, if you are a member of "/DebtAgents/Managers", you are also a member of "/DebtAgents".

The Authorization Bundle uses all of those rules to apply permissions.

Applying permissions

I think that it should be pretty obvious by now how the Authorization Bundle makes a decision about whatever a particular operation is allowed or denied, but the response for denying an operation are worth some note.

* When performing a query over a set of documents, some of which we don't have the permission for under the specified operation, those documents are filtered out from the query.
* When loading a document by id, when we don't have the permission to do so under the specified operation, an error is raised.
* When trying to write to a document (either PUT or DELETE), when we don't have the permission to do so under the specified operation, an error is raised.

That is pretty much as detailed as I want things to be at this stage. Thoughts?