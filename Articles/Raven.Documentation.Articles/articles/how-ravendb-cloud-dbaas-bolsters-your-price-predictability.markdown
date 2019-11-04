# A Cloud DBaaS that Predicts And Cuts Costs<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![How RavenDB Cloud DBaaS Bolsters Your Price Predictability](images/how-ravendb-cloud-dbaas-bolsters-your-price-predictability.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

<p class="lead" style="font-size: 24px;">The primary goal in using any DBaaS or SaaS tool is to cut costs. The challenge is to anticipate your next database cloud bill. RavenDB Cloud helps you accomplish both.</p>

According to Gartner, the average project saves 16% by migrating to the cloud. The primary reason projects migrate is to lower their costs by meeting or exceeding this number.

There are two challenges involved: The first is to maintain price predictability. Since the cloud charges you per usage for things like I/O and network traffic which is much harder to compute in advance, certainly than a fixed cost, it can be hard to anticipate what your next monthly bill will be. This can wreak havoc in maintaining your monthly budget.

The next is to leverage the new features of the cloud to convert into price savings.

<a href="https://cloud.ravendb.net" target="_blank">RavenDB Cloud</a> has created some unique features to enable you to meet both challenges.

**Load Balance in Burstable Instances**. This is something you get only with RavenDB!
<div class="pull-left margin-right">
  <div class="quote-textbox-left">
    The cloud has thrown up a new obstacle to high-availability.
  </div>
</div>
The cloud has thrown up a new obstacle to high-availability. It's payment. If you prepay for a certain level of usage for a month, what happens when you exceed the usage you paid for before the end of the month? It's like buying enough milk for the week. If you get to the last glassful by Wednesday you either have to take sips until the weekend, buy more in the middle of the week, or go without.

If a machine you provisioned gets overused, your cloud platform will reduce the computing power it gives you so you won't exhaust your prepaid resources until the next scheduled pay date or unless you manually prepay early. This means they will give you less than 1% of performance, literally bringing that machine to a standstill.

RavenDB views this as a downed node. It sees no difference between a fully throttled instance and a downed machine. It will work the same way to make sure that the machines which still have plenty of prepaid resources will take on the work burden to keep your application running at 100%.

RavenDB will balance the workload between your cloud instances based on the amount of CPU credits available to each instance. As you exhaust your credits on one instance, RavenDB will failover operations to another instance so you will not get throttled, or worse. As that instance runs low on credits, RavenDB will failover to another machine to make sure your <a href="https://ravendb.net/articles/nosql-database-for-digital-banking-applications">application database</a> is always running at full capacity with predictable pricing while using optimal resources.

RavenDB will also warn you when you are getting low on credits and need to replenish your instances to make sure your systems are always running. As a result of this feature, RavenDB users are *saving money on the cloud* by choosing lower tier systems and successfully running their applications on them.

They are also able to run at full speed knowing that if things get tight, they will be warned, and the contingency plan will be implemented.

<br/>
<a href="https://ravendb.net/learn/webinars/delegating-your-backend-work-to-ravendb-cloud-dbaas"><img src="images/delegating-your-backend-work-to-ravendb-cloud.png" class="img-responsive"/></a>
<br/>

**Scale in and out at no expense.** If you don't need all the resources you originally provisioned, why should have to pay to return them? *RavenDB is one of the few databases to offer* you the freedom to choose a more resource efficient plan at no cost to your business and no disruption to your systems. Simply choose the new plan, and your database will be on it in 15 minutes with no disruption of service.

Your application will continue to run, no need to hit the red button and have the ops team on call. No need to pay for twice the cost of the hardware or expensive migrations.

AWS Elastic requires you copy all your data and pay for the both plans while you are moving your data. *This makes moving plans prohibitively expensive*, taking the option off the table. CosmosDB requires you to purchase request units, effectively charging you per transaction to move. RavenDB lets you do it for free and on the fly, keeping your database running, your costs predictable, and your users unaware of anything happening outside of the smooth running of your application.
<div class="pull-right margin-left">
  <div class="quote-textbox-right">
    The RavenDB Management Studio GUI lets you see all the internals of your database (...)
  </div>
</div>
The RavenDB Management Studio GUI lets you see all the internals of your database, giving you every opportunity to choose the optimal resource allocation at any time and change it at will to restrain your costs and expand your returns.

**Automatic Management**. Your database is like a pipe connected to a pool where the pool is your storage and your pipe is your data IOPS. If the pool is too big and the pipe passing the water to the pool is too small, it's cost inefficient. If the pipe is too big or the pool is too small - water can overflow, and that's dangerous. Your RavenDB GUI gives you visual internals on the current size of your pool and your pipe so you can make sure everything is optimal at all times.

RavenDB even takes it one step further.

RavenDB can say, "IOPS is too small, not enough data is moving to storage based on the amount of data you are currently processing. We need more IOPS". Based on your configurations, RavenDB can automatically do this for you. RavenDB can also expand the size of the pool, or your data storage size, whenever the situation dictates.

Since you configure the limit to what you can invest in a specific time period, RavenDB can auto-adjust without going beyond what you expect to spend in the cloud. Your minimal cost will be as steady as your optimal performance.

<br/>
<a href="https://cloud.ravendb.net" target="_blank"><img src="images/ravendb-cloud.png" class="img-responsive"/></a>