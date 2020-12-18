<h1>Non Relational Databases Today Cost Less in Time, Money, and Headache</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/why-non-relational-databases-today-cost-less-in-time-money-and-headache.jpg" alt="Why Non Relational Databases Today Cost Less? How with one simple change you can reduce developer time, cost and latency in your applications." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">With one simple change you can reduce developer time on your database, reduce cost to your cloud platform, and reduce latency in your applications.</p>

Impedance mismatch is when there is a nontrivial difference between the way that your data is modeled and the way the data is stored.

That's why the right data model is a huge time saver for your release cycle and DevOps. Using a relational database to store your data against an application using objects to manage it is like working with a square key and a round keyhole.

Your application code will need a lot of sandpaper and duct tape to get your systems to work together.

You may ask yourself, can't I just round the edges by creating classes with properties and methods that smooth the data from the database to the app and back?

The challenge is not just in the application objects, but in the data queries.

Let's start with a simple class and some basic data about fathers:

<pre>
    <code style="background:transparent;">
    class User {
        string Name;
        Int Age;
    }
    </code>
</pre>

Now, in my SQL database, I make my table:

<pre>
    <code style="background:transparent;">
    Create table Users (Name string, Age int);
    </code>
</pre>

Here is my data table:

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Age</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Peter</td>
        <td>35</td>
      </tr>
      <tr>
        <td>Mike</td>
        <td>43</td>
      </tr>
    </tbody>
  </table>
</div>

Now, let's add just one more property to make it interesting. How about, array `Children[]` to track their kids?

<pre>
    <code style="background:transparent;">
    class User {
        string Name;
        Int Age;
        string[] Children;
    }
    </code>
</pre>

Now, how can we store an array into a table of rows and columns?

Here is one approach:

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Age</th>
        <th>Children</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Peter</td>
        <td>35</td>
        <td>Meg, Chris, Brian, Stewie</tr>
      </tr>
      <tr>
        <td>Mike</td>
        <td>43</td>
        <td>Greg, Marcia, Jan, Peter, Bobby, Cindy</td>
      </tr>
    </tbody>
  </table>
</div>

But this is not an array inside the database. It is a string.

Now I have to redo my class code to adapt:

<pre>
    <code style="background:transparent;">
    class User {
        string Name;
        Int Age;
        string[] Children => DB_Children.Split(‘,’);
    }
    </code>
</pre>

This is an excellent example of an impedance mismatch. I have the model of my object in my class, and I have how it's stored in the database.

This is an elementary example, and already you have to dedicate the majority of your resources to manage the mismatch. Adjusting your object to fit the data is not a simple job.

What happens when our examples get hairy and start to resemble the real world?

How about querying my database for all users that have a child named Greg?

Here is the query:

<pre>
    <code style="background:transparent;">
    select * from Users where Child.Name = ‘Greg’
    </code>
</pre>

but the way it's worded will be:

<pre>
    <code style="background:transparent;">
    select * from Users where Children like ‘%greg,%’
    </code>
</pre>

This is because the data is modeled to make all the children one big string array with elements separated by commas. It's going to take longer to process and return a results set.

You will also get a match for names like:

"Gregory"

But if a User has a single child named Greg, there wouldn't be a match because there will be no ‘,’ comma after the name Greg. This will also not match "Greg" the Youngest.

In one query, something as simple as finding users with a child with a specific name becomes an enormous task. You now have to optimize things on the database end as well. Queries using LIKE in this manner also not use indexes, so will be forced to do a full scan, slowing things down when you have a lot of data.

See how the data is stored is different from how the information is represented in memory. The database vs. the application's "version" of the data creates the impedance mismatch.

*The challenge for developers using relational databases is that it is too easy to create queries that don't deliver the exact data sets you are trying to capture with your queries. This requires more time to create different queries and to test them to confirm their users are always getting exactly what they are asking for, putting stress on your release cycle and DevOps process.*

This forces developers to spend more time on their database and less time on their applications. It produces lots of potholes when developing how your application works with your data.

### Non Relational Databases Today Can Avoid This Nightmare

<div class="margin-bottom">
    <img src="images/tended-quote-12.jpg" class="img-responsive m-0-auto" alt="RavenDB lets me get to the business of writing code and not mess around with schema, migrations, and all the misery that comes with a relational database."/>
</div>

*What if we made 2 tables, one for parents and one for kids with the key-value being the parent ID?*

Here is what is stored in the relational database:

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Age</th>
        <th>Parent ID</th>
        <th>Child ID</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Peter</td>
        <td>35</td>
        <td>3</td>
        <td></td>
      </tr>
      <tr>
        <td>Mike</td>
        <td>43</td>
        <td>2</td>
        <td></td>
      </tr>
      <tr>
        <td>Greg</td>
        <td>18</td>
        <td>2</td>
        <td>1</td>
      </tr>
      <tr>
        <td>Marcia</td>
        <td>17</td>
        <td>2</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Peter</td>
        <td>14</td>
        <td>2</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Jan</td>
        <td>12</td>
        <td>2</td>
        <td>4</td>
      </tr>
      <tr>
        <td>Bobby</td>
        <td>8</td>
        <td>2</td>
        <td>5</td>
      </tr>
      <tr>
        <td>Cindy</td>
        <td>6</td>
        <td>2</td>
        <td>6</td>
      </tr>
      <tr>
        <td>Meg</td>
        <td>12</td>
        <td>3</td>
        <td>7</td>
      </tr>
      <tr>
        <td>Chris</td>
        <td>14</td>
        <td>3</td>
        <td>8</td>
      </tr>
      <tr>
        <td>Brian</td>
        <td>5</td>
        <td>3</td>
        <td>9</td>
      </tr>
      <tr>
        <td>Stewie</td>
        <td>0.9</td>
        <td>3</td>
        <td>10</td>
      </tr>
    </tbody>
  </table>
  <p class="text-center">
    <small>
      <em>Table 1. People Table</em>
    </small>
  </p>
</div>

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>Child ID</th>
        <th>Parent ID</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>1</td>
        <td>2</td>
      </tr>
      <tr>
        <td>2</td>
        <td>2</td>
      </tr>
      <tr>
        <td>3</td>
        <td>2</td>
      </tr>
      <tr>
        <td>4</td>
        <td>2</td>
      </tr>
      <tr>
        <td>5</td>
        <td>2</td>
      </tr>
      <tr>
        <td>6</td>
        <td>2</td>
      </tr>
      <tr>
        <td>7</td>
        <td>3</td>
      </tr>
      <tr>
        <td>8</td>
        <td>3</td>
      </tr>
      <tr>
        <td>9</td>
        <td>3</td>
      </tr>
      <tr>
        <td>10</td>
        <td>3</td>
      </tr>
    </tbody>
  </table>
  <p class="text-center">
    <small>
      <em>Table 2. Children Table</em>
    </small>
  </p>
</div>

Let's try a new query:

**Give me all the parents who have children under the age of five.**

I have an object with properties, and I want to query that. Here is the question:

<pre>
  <code style="background:transparent;">
  select * from Users where Children.Age <= 5
  </code>
</pre>

In RavenDB, a document oriented [non relational database](https://ravendb.net/articles/save-time-and-money-with-non-relational-database-data-compression), this is your query:

<pre>
  <code style="background:transparent;">
  from Users where Children[].Age <= 5
  </code>
</pre>

using today's databases in SQL, this is your query:

<pre>
  <code style="background:transparent;">
  select * from Users as parent
  join FilialTies as ft on parent.Id = ft.ParentId
  join Users as child on ft.ChildId = child.Id
  where child.Age <= 5
  </code>
</pre>

That's a lot of work for your system!

What are the results set? It should be Peter (parent ID = 3).

The RQL query is straightforward. Just grab the document property Parent where there is a child under the age of five.

The SQL asks you to do some more work.

Let's grab the table again. Here we have the parents and the children with their parent Ids.

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Age</th>
        <th>Parent</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Peter</td>
        <td>35</td>
        <td></td>
      </tr>
      <tr>
        <td>Mike</td>
        <td>43</td>
        <td></td>
      </tr>
      <tr>
        <td>Greg</td>
        <td>18</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Marcia</td>
        <td>17</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Peter</td>
        <td>14</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Jan</td>
        <td>12</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Bobby</td>
        <td>8</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Cindy</td>
        <td>6</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Meg</td>
        <td>12</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Chris</td>
        <td>14</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Brian</td>
        <td>5</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Stewie</td>
        <td>0.9</td>
        <td>3</td>
      </tr>
    </tbody>
  </table>
</div>

We need to join them where children are indicated.

Note that we are talking here about the simplest possible scenario. A real-world schema could drown you in the details, especially when you build something that scales out.

<div class="table-container">
  <table class="table table-dark table-responsive table-bordered table-striped">
    <thead>
      <tr>
        <th>child.Name</th>
        <th>child.Age</th>
        <th>parent.Name</th>
        <th>parent.Id</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Greg</td>
        <td>18</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Marcia</td>
        <td>17</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Peter</td>
        <td>14</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Jan</td>
        <td>12</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Bobby</td>
        <td>8</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Cindy</td>
        <td>6</td>
        <td>Mike</td>
        <td>2</td>
      </tr>
      <tr>
        <td>Meg</td>
        <td>12</td>
        <td>Peter</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Chris</td>
        <td>14</td>
        <td>Peter</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Brian</td>
        <td>5</td>
        <td>Peter</td>
        <td>3</td>
      </tr>
      <tr>
        <td>Stewie</td>
        <td>0.9</td>
        <td>Peter</td>
        <td>3</td>
      </tr>
    </tbody>
  </table>
</div>

That's a query within a query just to put the dataset of potential matches together.

*So even if you rearrange your classes to take in relational data more efficiently, the queries themselves are inefficient, resulting in more calls to the database, computing costs, and performance lags.*

As the query stands, we are taking every parent.Id with a child under the age of five. But Peter, parent.Id 3, has two children under or equal to age 5.

Wouldn't his name appear twice in the results set? Your query is returning a Cartesian Product, or redundant data.

Based on a simple query, we see the added workload using SQL versus non relational databases. While there are ways around all of these issues, they add friction to the process and require additional developer time and resources to solve.

Here are the main costs that come with the relational model:

<ol>
  <li>You need to divert more developer time to create, deploy, and test queries until you are confident those queries return the most accurate results to your users.</li>
  <li class="margin-top-sm">The amount of work your database has to do can be exponentially greater by answering multiple questions to return results for a single query. Using the document model, databases today can serve a single query with a single query.</li>
  <li class="margin-top-sm">You run the risk of getting multiple data processed in your results. This requires further developer effort on your database and added memory and computing costs from your cloud platform. You get hit with added expenses in time and money.</li>
</ol>

<div class="margin-top">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule a Live Demo"/></a>
</div>