<h1>Document-oriented Databases - Saving Time and Money</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![Why can a relational database cost you and your users big time in poor data queries, while document-oriented databases can save everyone time and money.](images/document-oriented-databases-can-conquer-costs-to-you-and-your-users.jpg)

{SOCIAL-MEDIA-LIKE/}

In the late 90s, installing the Microsoft Office was no simple task. Once the setup wizard had everything it needed, you had to wait up to an hour for it to install the hundreds of parts and put them together.

It wasn't supposed to be that way. One minor decision about efficiency cost mankind *3,000 years* of lost time. People would have to wait up to an hour to use something they already paid $500 for.

In the final stages of production, the team at Microsoft simply didn't want to wait 40 minutes to install the system to build and test their release candidates.

They chose to save a little bit of time by not including the settings when they used Windows for production. As a result of lazy debugging, the 50 million plus users of Windows had to wait a combined 3 millennium to get started with their platform.

Things haven't changed.

A single inefficiency in how your application queries your database can cause the same frustration to your millions of users.

The cost of using your own data should be as close to zero as possible. These are assets you've already paid for. Little do we realize that the true costs are a lot higher, but they don't have to be.

#### The Costs of Getting the Data in a Relational Database

Here is the blueprint for a simple sales system database.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/document-oriented-databases-can-conquer-costs-to-you-and-your-users/1.jpg" class="img-responsive m-0-auto" alt="Simple sales system database blueprint"/>
</div>

Non-trivial to say the least.

What happens when you want sales for a particular product during a specific promotion? You have to start out by doing a lot of JOIN operations. That costs time.

Let's try something simpler, like a parking ticket.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/document-oriented-databases-can-conquer-costs-to-you-and-your-users/2.jpg" class="img-responsive m-0-auto" alt="Parking ticket blueprint"/>
</div>

Even for simple data models, you have a lot of tables that need to be JOINed. What happens when a user asks the system to *give me all the tickets issued by a particular police officer*?

You have to join several tables.

How about a citizen accessing the municipal website to see their unpaid tickets?

Here is an SQL query to produce the results:

<pre>
    <code class="language-sql" style="background:transparent;">
    select * from Violations where violation_id in (
            select violation_id from Tickets where date_ticket_paid is null
    )
    and violator_id = 4
    </code>
</pre>

But it's not so quick and easy:
<ol>
    <li class="margin-top-xs">First you have to scan the Violations table</li>
    <li class="margin-top-xs">Then you have to filter the violator_id</li>
    <li class="margin-top-xs">Finally, you have to filter for where the ticket isn't paid</a></li>
</ol>

That's three scans!

It's the local sheriff's office that has to flip the cloud bill to cover all of this, but using our tax money. *They* have to pay extra to access their own information, and *we* have to pay more to see how much we owe the city!

Poor choices in structuring your queries can lead to massive costs to everybody.

#### What if I Need More Information?

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/paper-and-pencil.jpg" class="img-responsive m-0-auto" alt="How did you store data in 1974? With a notebook and a pencil. This was the mindset of the data analyst when SQL was created. SQL bases itself on a pen and paper."/>
</div>

This is not a complex system. We are dealing with a very limited amount of straightforward information. What happens when the system scales up?

Notice that we are likely scanning through tens of thousands of parking tickets just to retrieve a handful.

Once I have the information I want, to get a full picture of all the details of the unpaid tickets, I need to perform even more JOINs to get the data from other places. What if I need to find out the total amount for all my tickets combined? Or if I have incurred any fines for paying late, or the status of contested tickets.

The costs to you and your users can be enormous.

#### Use a Document-oriented Database

More efficient data models can streamline this process, especially using a [document-oriented database](https://ravendb.net/articles/document-oriented-database-for-startups-offers-solution-for-27-dollars). The [roadblock of impedance mismatch](https://ravendb.net/articles/why-non-relational-databases-today-cost-less-in-time-money-and-headache) means a developer will have to spend more time than his team wants to in writing the best way to query a table, and quite often that query will still require a lot of work from your systems.

**Interested in exploring the world's first fully ACID document-oriented database? Say no more.**

<div class="margin-top-sm margin-bottom-sm">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule your free live demo presentation"/></a>
</div>