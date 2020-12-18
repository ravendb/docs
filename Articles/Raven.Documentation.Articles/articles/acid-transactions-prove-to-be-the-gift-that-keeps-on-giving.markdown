<h1>ACID Transactions for Shopper ID and Security Authentication</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/acid-transactions-prove-to-be-the-gift-that-keeps-on-giving.jpg" alt="Why can a relational database cost you and your users big time in poor data queries, while document-oriented databases can save everyone time and money." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <a href="https://solutions.incommincentives.com" target="_blank" rel="nofollow"><img src="images/incomm.png" class="img-responsive m-0-auto" alt="Incomm Incentives"/></a>
</div>

A lasting connection with your consumer is one of the most valuable assets your business can have. Building loyalty is the fast lane to capturing your customers' automatic attention, encouraging them to come back again whenever you have something new to offer.

A first-class rewards program keeps employees connected, always striving to win their next reward, constantly increasing performance. Gift cards are a great way to tell employees how grateful you are for their hard work. It can be given on birthdays, work anniversaries, holidays, special events in the company, along with reviews, and so many other ways to make sure people remember your organization and that their eyes will perk up at the mention of your professional brand.

It is also great for customers. Loyal customers also serve as brand ambassadors, telling their friends and family about your product, writing reviews, and even mentioning their experience on social media. A gift card is an effective way to encourage happy users to tell their friends.

InComm has pioneered the gift card industry.

Founded in 1992, they were the first to offer plastic gift cards. For the past 20 years, they have been offering similar solutions to companies, customers, and friends online. They boast clientele including Amazon, Walgreens, Dunkin' Donuts, Target, Apple, and more.

Processing thousands of transactions every second, they are transforming the shopping experience with RavenDB.

<div class="margin-top-sm margin-bottom-sm">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>

### ACID Transactions with Performance and Security

InComm has been using RavenDB since 2013.

They needed a NoSQL solution, and like them, RavenDB is a proven leader.

They use it for its clustering, replication, disaster recovery, high availability, and most of all, its ability to process [ACID Transactions](https://ravendb.net/articles/acid-transactions-in-nosql-ravendb-vs-mongodb) while keeping high performance. It's easier to enable clustering than with Microsoft SQL due to less overhead and engineering to get rolled out, saving them lots on DevOps.

They use RavenDB for catalog and pricing information. A classic for OLTP use, their NoSQL database manages discounts, especially for bulk orders. When employees of large clients buy gift cards, the database applies discounts to their purchase. RavenDB also takes care of managing revenues, commissions, and related costs.

Due to the high level of fraud in the gift card industry, security is vital.

RavenDB is used for shopperID and security authentication. To interface with their API, users get a client userID and password. RavenDB manages that along with all the token authorizations for all their financial transactions.

When a customer is shopping online on one of their client sites, they will have the option to buy a gift card. When they check out, a call must be made to InComm to process the gift card's purchase. RavenDB helps enable a secure connection. The authorization token is used over encryption to maintain authentication.

They also use RavenDB for holding email templates. Say you are Amazon, and you want InComm to handle their digital distribution. Amazon partners with them to give them emails they want to be sent, and RavenDB manages it all. Email templates, cards, gift-card amounts, pre-defined text, even company logos are stored so every client can decide how they want to present their gift cards to the lucky owners receiving them.

As the holidays approach and companies, families, and friends look for the best ways to say "You're tops", we at RavenDB are delighted to play such an essential role in the scaling out, replication, and distribution of the greatest asset of all: Brotherly Love.

<div class="margin-top-sm margin-bottom-sm">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule your free live demo presentation"/></a>
</div>