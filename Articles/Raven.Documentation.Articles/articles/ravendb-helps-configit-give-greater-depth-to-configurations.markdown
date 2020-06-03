# NoSQL Database for Greater Insight<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![RavenDB NoSQL Database Helps Configit Give their Customers Greater Insight to their Configurations](images/ravendb-helps-configit-give-greater-depth-to-configurations.jpg)

{SOCIAL-MEDIA-LIKE/}

## RavenDB NoSQL Database Helps Configit Give their Customers Greater Insight to their Configurations

Technology and modern manufacturing methods have laid waste to the days of "You can have any color you want as long as its black." Customers, both individual and business, have plenty of options on how to put together exactly what they want.

<a href="https://configit.com/" target="_blank" rel="nofollow">Configit</a> develops configuration technology solutions for manufacturers worldwide. They enable them to make sure products with complex and diverse configurations are safe for the public to use and up to each customer's tailored standards.

<div class="pull-left margin-right">
  <div class="quote-textbox-left">
    The more sophisticated the product, the more complex the configuration.
  </div>
</div><p class="text-justify">Most products today need to be configured one way or another. The more sophisticated the product, the more complex the configuration.</p>

Take cars, for example. For each car you have different models that can run on different engines. Just on these two variables, you can have a huge selection of configurations. As more components are determined, the number of permutations expand, and the list of inputs to your joy ride get longer.

At the same time there are restrictions. Safety regulations, manufacturing standards, and company protocols will demand that some configurations are not permitted, while others need to include certain components no matter which configuration is chosen. If you are configuring a car for use in the UK, the steering wheel must be on the right side. If it is going to America, there must be turning signals in the front and the back.

Configit software takes all the components, options, possible configurations, standards, rules, prices, and more and maintains them. Once the customer enters what they want, Configit Quote will provide them with a detailed quote of every item they selected, every item they will need, its corresponding price, and the total amount.

## Utilizing the Advantages of a Schemaless Document Database

Configit uses RavenDB for their quotes application. They have been using RavenDB since 2014.

Once the customer has decided what they want, a document is created. It's a price quote containing the products, the prices, the specific configuration, the name of the customer, their shipping address, and more. A single quote can contain as much as 10,000 lines of information.

Imagine configuring something as simple as a cupboard. Your configuration would include the type of wood, color, which handles to use, what shelves to install, what types of hinges to attach. For one item you can have hundreds of different configurations, creating an itemized quote that can get rather lengthy. The document reaches thousands of lines once you decide to configure an entire kitchen.

Using a relational model for this application would have been a nightmare. For thousands of products per data set, you would be joining more tables than IKEA. Their performance would have killed the application. A [document database enables](https://ravendb.net/articles/cost-benefits-ravendb-nosql-acid-database) Configit to put all the information in one place and have it immediately ready for the customer on demand with minimal trips to the server.

<div class="pull-right margin-left">
  <div class="quote-textbox-right">
    RavenDB's schemaless data model is exactly what configit needs to make their quote application adaptable to each client.
  </div>
</div><p class="text-justify">Another challenge was flexibility. In order to serve configurations for products ranging from a 6-cylinder 8 seat Minivan to pantry cupboards, you have to offer your clients a dynamic way to model their data. A schema would cement how the data was classified, forcing the shiny wood varnish to double as number of brake pads per inspection.</p>

The sheer overhead involved with changing a set schema was untenable.

RavenDB's schemaless data model is exactly what configit needs to make their quote application adaptable to each client.

## Scaling Upward and Outward

As Configit continued to expand, so did the scale and the scope of their data. They quickly saw more quotes coming and going throughout their system. As they started servicing larger factories, the information provided in their quotes became more detailed and complex.

Some of the catalogs their software was supporting had up to 100,000 components and products inside.

Documents for configurations for these products expanded to tens of thousands of lines. This put functionality and performance into play. RavenDB needed to store and move documents with huge amounts of information and they had to do it while [continuing to meet high-performance standards](https://ravendb.net/why-ravendb/high-performance).

As Configit upgraded from RavenDB 3.0 to RavenDB 4.2, which is over 10 times faster, they were able to meet their functionality and performance needs.

<div style="margin: 30px">
    <img src="images/configit.jpg" class="img-responsive m-0-auto" alt="The fact that RavenDB can store the entire document and continue to perform well is really impressive.">
</div>

## The Bottom Line

Using RavenDB in its quotes application enables Configit to manage its data the exact way it accepts it from customers. Taking advantage of a schemaless data model lets Configit offer the flexibility to its clients that makes managing the complexity of its data easier. A NoSQL database saves massive overhead by eliminating the need to join tables for every line item in a data set that contains tens of thousands of them.

As Configit continues its rise to the top of their market, they have a database that can handle higher workloads both in terms of greater data sets and larger data quantities, continuing to give its customers superior performance and service.

Special thanks to Torsten B. Hagemann, Peter Tiedemann, Arian Kadkhoda at Configit for their generous help.

<div style="margin: 30px">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>
