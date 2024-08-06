# Sharding by prefix
---

{NOTE: }

* The default technique of distributing data across shards uses a hashing algorithm based on document IDs  
  to [populate buckets](../../sharding/overview#buckets-population) that are assigned to shards. While this method is effective in distributing data evenly,  
  it may not meet specific data partitioning needs, as it doesn't allow you to fully control which shard a document will reside in.

* **Sharding by prefix**, also known as **prefixed sharding**, allows you to assign data to specific shards  
  by explicitly specifying on which shard a document should reside based on its ID prefix.

* With prefixed sharding, you only control the shard where a document is stored.  
  You cannot control the specific bucket within that shard where the document will reside.  
  This can be partially addressed by [anchoring documents](../../sharding/administration/anchoring-documents) to a bucket.
  Learn more [below](../../sharding/administration/sharding-by-prefix#prefixed-sharding-vs-anchoring-documents).
  
* In this page:  
  * [Why use prefixed sharding](../../sharding/administration/sharding-by-prefix#why-use-prefixed-sharding)  
  * [Overview](../../sharding/administration/sharding-by-prefix#overview)  
  * [Bucket management](../../sharding/administration/sharding-by-prefix#bucket-management)  
  * [Adding prefixes via Studio](../../sharding/administration/sharding-by-prefix#adding-prefixes-via-studio)
  * [Adding prefixes via Client API](../../sharding/administration/sharding-by-prefix#adding-prefixes-via-client-api)
      * [Add prefixes when creating a database](../../sharding/administration/sharding-by-prefix#add-prefixes-when-creating-a-database)
      * [Add prefixes after database creation](../../sharding/administration/sharding-by-prefix#add-prefixes-after-database-creation)
  * [Removing prefixes](../../sharding/administration/sharding-by-prefix#removing-prefixes)
  * [Updating shard configurations for prefixes](../../sharding/administration/sharding-by-prefix#updating-shard-configurations-for-prefixes)
  * [Prefixed sharding vs Anchoring documents](../../sharding/administration/sharding-by-prefix#prefixed-sharding-vs-anchoring-documents)

{NOTE/}

---

{PANEL: Why use prefixed sharding}

**Control over data placement**:
Prefixed sharding offers customized data distribution by allowing you to assign data to specific shards.
This facilitates tailored data partitioning aligned with your business logic and enhances the organization of your data management.

**Geographical Data Grouping**:
For applications serving users from different regions, storing data on a shard in a region near the end user is beneficial, as it can improve access speed and reduce latency.
Additionally, laws and regulations may mandate that data be stored within specific geographical boundaries.
Business requirements might also necessitate keeping data close to the audience to enhance query performance and user experience.

**Optimized query performance:**
Prefixed Sharding eliminates the need to query all shards in the cluster by allowing queries to target specific shards containing the relevant documents.
Grouping data on the same shard enhances query performance and reduces latency, particularly for regional operations.

**Scalability:**
As your system grows and you add more servers, prefixed sharding simplifies managing increased data volumes and traffic.
You can add servers and shards as needed, allowing for controlled scaling that maintains both performance and reliability.

**Overall performance**:
By grouping related data on the same shard, prefixed sharding optimizes data storage and access patterns,
reduces network latency for region-specific operations, and enhances overall system performance in distributed database environments.

{PANEL/}

{PANEL: Overview}

**Configure shards**:  

* To store documents on a specific shard, define the target shard for each document ID prefix that you add.  
  Documents with IDs matching any of the defined prefixes will be routed to the corresponding shard.  
  Learn how to [add prefixes](../../sharding/administration/sharding-by-prefix#adding-prefixes-via-client-api) below.

* For example, you can define that all documents with an ID starting with `users/us/` will be stored in shard **#0**.  
  Consequently, the following sample documents will be stored in shard **#0**:  
  * _users/us/1_  
  * _users/us/2_  
  * _users/us/washington_  
  * _users/us/california/company/department_  

**Configure multiple shards**:  

* You can assign multiple shards to the same prefix.

* For example, both shard **#1** and shard **#2** can be assigned to prefix `users/us/`.  
  In this case, any document with that prefix will be stored in either shard **#1** or shard **#2**.  

**Prefix rules**:  

* The maximum number of prefixes that can be defined is 4096.  

* The prefix string that you define must end with either the `/` character or with `-`.  
  e.g. `users/us/` or `users/us-`.

* Prefixes are case-insensitive.  
  RavenDB will treat `/users/us/` and `/users/US/` as equivalent document prefixes.  
  Therefore, documents such as "_/users/us/1_" and "_/users/US/2_" will be routed according to the same rule.

* RavenDB prioritizes the most specific prefix over more general ones.  
  For example, if you configure `/users/us/` to shard **#0** and `/users/us/florida/` to shard **#1**, then:  
  * Document "_/users/us/123_" will be stored in shard **#0**.  
  * Document "_/users/us/florida/123_" will be stored in shard **#1**, even though it also matches the `/users/us/` prefix.

{PANEL/}

{PANEL:  Bucket management}

**When you define a sharded database**:  
RavenDB reserves 1,048,576 buckets for the entire database. Each shard is assigned a range of buckets from this set.  
Any document added to the database is processed through a hashing algorithm, which determines the bucket number where the document will reside.
The initial bucket distribution for a sharded database with 3 shards will be:   

  * Buckets assigned to shard **#0**: `[0 .. 349,524]`  
  * Buckets assigned to shard **#1**: `[349,525 .. 699,049]`  
  * Buckets assigned to shard **#2**: `[699,050 .. 1,048,575]`  

**When you configure prefixes for sharding**:  
RavenDB will reserve an additional range of 1,048,576 buckets for each prefix you add, on top of the buckets already reserved.
So now, if we add prefixes **`users/us/`** for shard #0 and **`users/asia/`** for shard #1, we get:  

  * Additional buckets assigned to shard **#0** `[1048576 .. 2097151]` for documents with prefix `users/us/`
  * Additional buckets assigned to shard **#1** `[2097152 .. 3,145,727]` for documents with prefix `users/asia/`

--- 

When creating a new document with an ID that matches any of the defined prefixes -  
the hashing algorithm is applied to the document ID, but the resulting bucket number is limited to the set of buckets reserved for that prefix,
thereby routing the document to be stored in the chosen shard.

When creating a new document with an ID that does Not match any predefined prefix -   
the resulting hashed bucket number could fall into any of the 3 shards.

---

The reserved buckets ranges are visible in the database record in the Studio.  
Navigate to **Settings > Database Record** and expand the "Sharding > BucketRanges" property:

!["Bucket ranges"](images/assigned-bucket-ranges.png 'Bucket ranges across the shards')

{PANEL/}

{PANEL: Adding prefixes via Studio}

You can define prefixes when creating a sharded database via the Studio.

!["Create a new database"](images/create-new-database.png 'Create a new database')

1. From the database list view, click **New database** to create a new database.  
2. Enter a name for the new database.   
3. Click **Next** to proceed to the sharding settings on the next screen.

---

In this example, we define a sharded database with 3 shards, each having a replication factor of 2:

!["Enable sharding"](images/enable-sharding.png 'Enable sharding')

1. Turn on the **Enable Sharding** toggle.  
2. Set the number of shards you want.    
3. Turn on the **Add prefixes for shards** toggle.  
4. Set the replication factor and other options as desired, and then click **Next** to proceed to define the prefixes.

---

Add the prefixes and specify their destination shards:  

!["|Define prefixes"](images/define-prefixes.png 'Define prefixes')

1. Enter a prefix. The prefix string must end with either `/` or `-`.  
2. Select the target shard. Multiple shards can be selected for the same prefix.  
3. Click **Add prefix** to add additional prefixes.  
4. Click **Quick Create** to complete the process using the default settings for the remaining configurations and create the new database.
   Or, click **Next** to configure additional settings. 

---

New documents will be stored in the matching shards:

!["Document list view"](images/document-list-view.png 'Documents are stored in the requested shards')

1. Documents with prefix `users/us/` are stored in shard **#0**.  
2. Documents with prefix `users/asia/` are stored in shard **#1**.  
3. Documents with an ID that does Not match any prefix will be stored on any of the 3 shards.  
   e.g. document `users/uk/london/clients/1` is stored on shard **#2** since no matching prefix was defined.  

{PANEL/}

{PANEL: Adding prefixes via Client API}

Using the Client API, you can add prefixes when creating the database or after database creation.

{NOTE: }

#### Add prefixes when creating a database

{CODE-TABS}
{CODE-TAB:csharp:Sync prefix_1@Sharding\ShardingByPrefix.cs /}
{CODE-TAB:csharp:Async prefix_1_async@Sharding\ShardingByPrefix.cs /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

#### Add prefixes after database creation

* Use `AddPrefixedShardingSettingOperation` to add a prefix to your sharding configuration after the database has been created.

* In this case, you can only add prefixes that do not match any existing document IDs in the database.  
  An exception will be thrown if a document with an ID that starts with the new prefix exists in the database. 

{CODE-TABS}
{CODE-TAB:csharp:Sync prefix_2@Sharding\ShardingByPrefix.cs /}
{CODE-TAB:csharp:Async prefix_2_async@Sharding\ShardingByPrefix.cs /}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Removing prefixes}

* Use `DeletePrefixedShardingSettingOperation` to remove a prefix from your sharding configuration.

* You can only delete prefixes that do not match any existing document IDs in the database.  
  An exception will be thrown if a document with an ID that starts with the specified prefix exists in the database.

{CODE-TABS}
{CODE-TAB:csharp:Sync prefix_3@Sharding\ShardingByPrefix.cs /}
{CODE-TAB:csharp:Async prefix_3_async@Sharding\ShardingByPrefix.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Updating shard configurations for prefixes}

* Use `UpdatePrefixedShardingSettingOperation` to modify the shards assigned to an existing prefix.

* Unlike when defining prefixes for the first time,  
  the following rules must be observed when updating an existing prefix configuration:

    * **When adding a shard to an existing prefix configuration**,  
      RavenDB does not automatically reallocate buckets to the newly added shard.  
      Therefore, after assigning a new shard to a prefix, manual bucket [re-sharding](../../sharding/resharding) is required.  
      You must manually move some buckets initially reserved for this prefix from the existing shards to the new shard;
      otherwise, documents matching the prefix will not be stored on the added shard.
  
    * **When removing a shard from an existing prefix configuration**,  
      you must first manually move the buckets from the removed shard to the other shards that are assigned to this prefix.

* In the below example, in addition to shard **#2** that was configured in the [previous](../../sharding/administration/sharding-by-prefix#add-prefixes-after-database-creation) example,  
  we are adding shard **#0** as a destination for documents with prefix `users/eu/`.

{CODE-TABS}
{CODE-TAB:csharp:Sync prefix_4@Sharding\ShardingByPrefix.cs /}
{CODE-TAB:csharp:Async prefix_4_async@Sharding\ShardingByPrefix.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Prefixed sharding vs Anchoring documents}

**Anchoring documents**:  
With [anchoring documents](../../sharding/administration/anchoring-documents), you can ensure that designated documents will be stored in the same bucket  
(and therefore the same shard), but you cannot specify which exact bucket it will be.  
So documents can be grouped within the same shard, but the exact shard cannot be controlled.

**Prefixed sharding**:  
With prefixed sharding, you control which shard a document is stored in.  
However, you cannot specify the exact bucket within that shard.  

**Applying both methods**:  
When both methods are applied, prefixed sharding takes precedence and overrides anchoring documents.  
For example:  

Given:  

 * Using prefixed sharding, you assign shard **#0** to store all documents with prefix `users/us/`.
 * Your database already includes a document with ID `companies/456` stored in shard **#1**.

Now:

 * Using the anchoring documents technique, you create document `users/us/123$companies/456`,  
   with the intention that both `users/us/123` and `companies/456` will reside in the same bucket.  
 * In which shard will this new document be stored ?

The result:  

 * Since prefixed sharding is prioritized, this new document will be stored in shard **#0**,  
   even though the suffix suggests it should be stored in shard **#1**, where the Companies document resides.

{PANEL/}

## Related articles

### Sharding

- [Sharding overview](../../sharding/overview)
- [Anchoring documents](../../sharding/administration/anchoring-documents)

### Client

- [Sharding queries](../../sharding/querying)  
- [Querying a selected shard](../../sharding/querying#querying-a-selected-shard)
