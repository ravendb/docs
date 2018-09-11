# 7 Ways The Right Database Pays for Itself<br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a>, CEO RavenDB</small>

![7 Ways The Right Database Pays for Itself](images/cost-benefits-ravendb-nosql-acid-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

Operating deep inside your application, a good database should function like your heart:

* You <em>never</em> see it. 
* You don’t know how it works, and you don’t need to know how it works – as long as it’s working.
* In pumping blood to the entire body, it feeds the performance of every other part of your system. To keep the system at peak performance, it must be working all the time. 
* The better it works, the better every other part of your system works.

To reach this ideal state, you have to pay lots of money for a good cardiologist, a health and fitness instructor, and all the right foods to  reach your body’s peak performance. The same with a database. To optimize all it can do for you, it will <em>cost money</em>. 

But what if investing in the best database on the market returned you so much in cost savings and enhanced revenue it exceeded the price of the database? The right database should ultimately pay you huge dividends in function, performance, and productivity.

Here are the 7 must haves the right database must offer to generate you a positive return on your next project:

## 1. It Must be Easy to Learn

A developer earns, on average, $140,000 a year to make an app that will impact the company by millions, even tens of millions of dollars in increased revenue, lower expenses, or a combination of both achieved by productivity gains. 

Every moment he or she is not working to improve their app is a moment you are not getting the most on your investment. A database shouldn’t take too much time diverting a developer from their art by forcing them to relearn the alphabet in order to query the database.

We made RavenDB easy to learn, with an SQL based language that you can pick up <em>fast</em>.

Other databases require companies to pay large amounts in the form of training sessions, and downtime for the developers to train their entire staff on how to use a database that is complicated to navigate. As you scale out, developers in other offices of your business will also have to go through the process of reading stereo instructions to see how the database works.

We make it fast and simple for <em>everyone</em>.

{SOCIAL-MEDIA-FOLLOW/}

## 2. Easy to Set Up and Secure

One of our competitors wrote a 60-page document on how to install and use their security. No wonder they were hacked into over <em>100,000 times</em>! It can take days, even weeks to install and secure a database. We have a setup wizard that can tackle this in minutes. We treat security as a baseline requirement, not something that your admin will tackle sometime down the road, so we include security in the setup wizard to make your initial steps as simple as possible.

This saves your developers time, and you lots of worry.

## 3. A GUI Anyone Can Use that tells you What You Really Need to Know

<img class="floating-right img-responsive" alt="RavenDB aims to make your Database Administrator 25% more productive" src="images/management-studio-screenshot.jpg" />

RavenDB aims to make your Database Administrator <em>25% more productive</em> with Management Studio that monitors performance metrics on a real time scale. We tell you CPU usage, which nodes are functioning, which ones are down, and the recommended fix needed. RavenDB will automate a lot of the work DBAs normally require more time to perform.

## 4. Easy to Scale

What happens when your application is a stunning success and everyone just has to have it on their device? You have more traffic, more users, and more load to manage.

You have to scale out your database. But at what cost? How much time will your IT team have to dedicate to scaling instead of focusing on your app right when it makes it to prime time?

<a href="https://ravendb.net/features"><strong>RavenDB</strong></a> makes scaling out <em>quick and easy</em>.

You can add additional nodes to your database cluster with just a point and a click. You can replicate your database to each new node within hours and offer your users faster performance with an expanded distributed data network.

With a relational database, a good server can cost you $20,000.  Once your data storage and load has filled up, you need additional servers. RavenDB reaches <em>1 million reads and 100,000 writes per second</em> using commodity hardware costing <em>less than $1,000</em>. Scaling out saves you time and money.

## 5. Less Need for Technical Support

A good database is one that you don’t have to worry about, it just works. We created diagnostic tools to always monitor what’s working, and what isn’t. If something goes down, or simply isn’t working optimally, you will be sent a notification with a recommended fix. Our aim is to optimize the time you are in control of your application and can keep on developing it without interruption.

In those situations where you do need to get in touch with us, we don’t make you sweat. We know that there is nothing more frustrating than handing the fate of your application over to a third party who may not share your sense of urgency. Even if a company upholds their end of the SLA bargain by shooting out an automated “We got your issue and will reply shortly,” if it takes more than a day to fix, it puts at least one developer out of service. That can be $560 a day squandered on low priority work as the next release of your app remains stalled just short of the finish line.

We aim to reply to any issues <em>within 2 hours</em> and strive to start working on them as soon as possible. You get dedicated access to our development team who know the intimate details of <a href="https://ravendb.net/downloads"><strong>RavenDB</strong></a>, rather than someone with limited familiarity who forces you to jump through hoops while they walk you through the support scripts. This saves you time, money, and lots of frustration.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## 6. Speed Up Your Release Cycle

<img class="floating-left img-responsive" alt="A schemaless database makes changes easy" src="images/devops.jpg" />

A relational database makes any type of change difficult and dangerous. Changing the DB schema typically means taking down the application, maybe for hours.

It will require an expert to go through all the queries your application makes, comparing them to the existing indexes and verifying that there hasn’t been any performance regression.

This forces your IT team to invest more hours into the new release while compromising your data network.

A schemaless database makes changes easy. Changes to the schema in our document database are less costly in developer resources needed and are less disruptive to the steady running of your application.

All of this shortens the time to your next release, enabling you to make that leap forward in revenue or cost savings that much sooner.

## 7. Dynamic Querying to Keep Your Process Agile

Distractions are momentum killers. Any type of down time, freeze, or crash can turn away countless potential customers and keep them from buying. 1 in 4 shopping carts are abandoned before purchase – mostly <em>due to performance</em>. If the user has a few extra seconds to reconsider, especially while the payment page is taking its sweet time to load, they have more time to walk away.

One of the hidden dangers of an effective Agile Development process is that you are constantly tweaking the application. When you first create your application, you set up queries and indexes so when a user hits a button, the database knows exactly what to do in the most efficient manner. Peak performance becomes <em>a given</em>.

When you make a new version of your app, you change the way the application makes queries from the database. This can render your indexes obsolete, forcing the database to go over every piece of info it has, bit by bit, until it can answer the new queries of your latest build. This may kill performance every time you release a new version of your app.

You can avoid this mess with RavenDB’s dynamic querying. When a query is made to find or aggregate data, RavenDB will check to see if there are indexes available to do it faster. If there aren’t, RavenDB will make one. Once done, RavenDB will see if it can take the indexes already available and create more optimal indexes for future use.

This is an under the hood process you don’t have to worry about. The more you query, the faster your database works and the better your application will perform. This relieves you of the need to recreate queries and indexes when you rearrange your data in your next release. Once you release your next version, RavenDB will quickly have it running as fast as the last version, if not faster.

## The Bottom Line

<img class="floating-right img-responsive" alt="With RavenDB you don’t pay us to play, we pay you!" src="images/bottom-line.jpg" />

* A developer can make $140,000 per year. Assume 2 weeks vacation and 5 extra days for national holidays. That’s 49 weeks. On average, a developer works around 40 hours a week, or 1,960 hours per year. That’s around $70 per hour for each developer.
* Our easy to learn training guide can save a 5 person development team 20 hours, or $1,400. 
* Our setup wizard can save a development team at least 40 hours, or, $2,800.
* Using commodity hardware for a 7-node cluster rather than a huge server can save you $13,000.
* The median salary for a DBA is $100k. Productivity gains you $25,000 net.
* Rapid tech support can save you thousands in not having to idle your developers or delay the release of your next build.
* A schemaless data architecture and dynamic querying save your developers time in their next release. It also keeps performance high, especially at the point of sale. This not only saves you days of development, it increases sales right where the buying decision is solely up to the development team.

When you tally up all these savings, the total returns for using RavenDB on your next application far outweigh the investment! 

With RavenDB you don’t pay us to play, we pay you!

<div class="bottom-line">
    <p>
        Try it out! <a href="https://ravendb.net/downloads"><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running for your next project. RavenDB can be used on cloud solutions like Amazon Web Services (AWS) and Microsoft Azure.
    </p>
    <p>
        <a href="http://ravendb.net/"><strong>RavenDB 4.0</strong></a> is an ACID document database that specializes in online transaction processing (OLTP). It's fully transactional (ACID) across entire database, and features an SQL-like querying language. You can have the best of relational databases while enjoying the high performance of a non-relational solution. RavenDB gives you a distributed data cluster, flexibility, and rapid scalability with low overhead. RavenDB is an easy to use all-in-one database, striving to minimize your need for third party applications, tools, or support. You can set up RavenDB in a matter of minutes. 
    </p>
    <p>
        RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 reads and 100,000 writes per second on a single node. You can build high-performance, low-latency applications quickly and efficiently.
    </p>
</div>
