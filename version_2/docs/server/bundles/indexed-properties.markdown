#Indexed Properties bundle

Another bundle available in RavenDB is `Indexed Properties` bundle. It enables auto-updating of document properties by Indexing process, but what does it mean? In real-life scenario it gives user the ability to utilize index calculated values.

Lets consider a common case where we have a Customer and bunch of Orders and we want, without much of effort, to be able to get average order amount for that customer. We can do it in couple of ways, but one of them is particularly interesting.

{CODE indexed_properties_1@Server\Bundles\IndexedProperties.cs /}

To use bundle, at the beginning we must activate it. It can be done server-wide or per database. To activate bundle during database creation process you need to add `IndexedProperies` to `Raven/ActiveBundles` setting.

{CODE indexed_properties_0@Server\Bundles\IndexedProperties.cs /}

Second step is to create an index that will calculate the average.

{CODE indexed_properties_2@Server\Bundles\IndexedProperties.cs /}

{CODE indexed_properties_3@Server\Bundles\IndexedProperties.cs /}

Next, we will have to create a document `Raven/IndexedProperties/Index_Name` that consists of document key property name (in our case `CustomerId`) and a mapping between index field names and our document properties.

{CODE indexed_properties_4@Server\Bundles\IndexedProperties.cs /}

After this, whenever we add new orders to our customers, the `AverageOrderAmount` property will be updated automatically.

{CODE indexed_properties_5@Server\Bundles\IndexedProperties.cs /}

{CODE indexed_properties_6@Server\Bundles\IndexedProperties.cs /}

{NOTE It may take some time for RavenDB to updated indexes and our indexed properties. /}
