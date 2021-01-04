# Why are so Many NoSQL Databases Getting Hacked?

<div class="article-img figure text-center">
  <img src="images/broken_db.jpg" alt="Why are so Many NoSQL Databases Getting Hacked?" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<br/>

In its race to become the new standard of managing data, the NoSQL Database industry has hit a speedbump. 

To date, over 100,000 MongoDB databases have been hijacked. Hackers have also raided thousands of servers hosting data on ElasticSearch, Apache CouchDB, and Hadoop.  The hackers wiped out all of the data in the database, and held their business hostage by demanding money in return for the restoration of their database. While the ransom demand itself is typically in the range of a few hundred dollars, having to disclose a data breech can be devastating to a company’s reputation. There are no guarantees that paying the ransom will recover the data or prevent it from being leaked.
Why is this happening to most of the NoSQL databases in the industry? Is there a way to stop it?

## When Your Defaults Default

Most NoSQL Databases assume you will configure your security on your own. It’s default settings allow open access and control to any outside user. A developer will often download the database and can start coding using the default settings without having to consider the security of the system. To make things easy, most NoSQL databases will be installed with the doors wide open, welcoming everybody and anybody in.
Most NoSQL databases have adequate security features, but you must know how and when to configure them. Upon installing MongoDB, for example, the database accepts everyone as the admin. At this stage security is not a problem. The installer is usually a developer using a local machine as a server, and the users are also limited to this one machine. 

{SOCIAL-MEDIA-FOLLOW/}

The problem occurs further down the application life-cycle. It can be weeks, or even months later when the developer finishes coding his app, testing it, fixing all the bugs, and sending it to production for release. Security becomes an issue when you expose your database to the public internet, and the doors are still open! 
In order to lock the doors, you have to reconfigure the default settings. If you assume that the standard MongoDB defaults are enough to protect your database, your MongoDB is ripe to become another instance of the over 100,000 databases that were broken into and wiped clean.
Even if the hackers return your data, as they offered to do for anyone who paid their ransom, you have no idea whether or not they made a copy and sold it to the highest bidder. One small oversight can result in your business having to disclose a data breech, face regulatory action, suffer damage to your credibility in front of clients and prospects, and incur unplanned expenses to add additional layers of security going forward. According to Gartner Research, unplanned outages can cost companies over $100,000 per *hour*. 

## The RavenDB Solution

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

To meet these challenges head on, RavenDB equips the doors to your database with automatic locks. 
As long as you are using RavenDB over one local machine, you can perform a “yes, dear” install to start coding your app right away. While the users on your local machine are the ones accessing the database, RavenDB sees no security risk, and the doors to your database stay open. This makes it easy to start building your application with RavenDB.

Once you set RavenDB to listen to connections outside your local machine, your database will sound the alarm. RavenDB will immediately block the vulnerable configuration and refuse to run in such a blatant unsecured mode.

The administrator at this point will be required to change the defaults in one of two ways:

1. Configure the security options to protect your data for users both internal and external to the network.

2. If RavenDB is running in a trusted environment, you can tell RavenDB that you’ll take the onus of security and ensure that only valid connections will reach it.

Whenever you deploy RavenDB to an external network, endangering safety of your data, the automatic locks on the doors are triggered. RavenDB prompts you to either change the security configurations on your database, or confirm that your current security provides enough protection for you to go forward.

We don’t let you move forward with an unsecured configuration all the way to release. If your defaults are neglected, your data is exposed to anyone who wants to grab it. 

That’s why we force the locks on the doors shut. RavenDB makes sure that your data is *already* protected the moment it’s released into the wild. 

<p style="font-size: larger">RavenDB is among the few NoSQL databases that is both ACID and transactional. Built for speed, it pushes your server to the limits. Made for scale, RavenDB will accommodate the expansion of your data, and the success of your business. <a href="https://ravendb.net/downloads#server/dev">Take a free copy</a> and see for yourself!</p>

<p style="font-size: small">
    Copyright: <a href='https://www.123rf.com/profile_pisotskii'>pisotskii / 123RF Stock Photo</a> 
</p>