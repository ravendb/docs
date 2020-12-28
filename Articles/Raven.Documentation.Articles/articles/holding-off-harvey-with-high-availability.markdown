# Holding off Harvey with High Availability

![RavenDB is an open source NoSQL document database](images/world-map-connections.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

We were saddened to see the tragedy Hurricane Harvey brought to the good people of the United States.

As the world is connected into everything, any type of disruption to these connections can threaten the lives of the people cut off. Analyzing where we can keep data connected to everyone throughout emergency situations enables us to better serve people facing similar crises in the future. As Hurricane Irma creeps towards the American East Coast, we want to be vigilant in learning every new lesson quickly, and applying it fast. This is an area of continuous improvement that will always take high priority.

## What Hurricane Harvey Taught Us

> *Right when the people of Houston needed real-time information the most, their data centers became highly vulnerable, and their ability to serve rescue workers was compromised.*

With 130 miles per hour gusts and torrential rainstorms, Hurricane Harvey pummeled the Lone Star State for an unprecedented 5 days. One of the most vital signs of Houston's lifeblood turned critical: it's data infrastructure. While the storm was raging, personnel on the ground needed the following:

- Traffic data for relief trucks coming in and out of the area. Aid convoys carrying perishables couldn't afford to get stuck, take a wrong turn, or wind up driving into 3 feet of water. One wrong turn could put the lives of thousands of people at risk.

- Search teams needed real time information on who is missing, who has been found, and where separated family members are located. Priorities needed to be set and constantly updated to determine which residents required more resources for their rescue. 

- Medical records needed to be made available to first aid responders on site. They needed to call up on their mobile devices and tablets personal histories of the wounded before administering treatment. The data needed to be there the moment they pressed the “submit” button.

Now that the storm has passed, the recovery begins. Despite whatever immediate damage, data services must remain available:

- Social services and nonprofits have to keep track who has received what, in order to stretch their resources as far as they can to provide for a homeless population whose needs far outweigh what's available.

- Businesses must get back on their feet as soon as possible. The Houston metropolitan area employs over 3 million, hosting company headquarters for 4% of the Fortune 500. Huge employers include The Johnson Space center, which employs over 10,000, and Apple, Inc, which employs 8,700. A crack in the crown jewel of business, their data, can endanger the ability of Huston's local offices to continue to employ so many people. As the 4th largest city in America, prolonged disruption to information can prolong the local economic downturn, which can bring the entire nation into a recession. A recession to the world's largest economy will send economic shockwaves everywhere. 

Data continuity is vital. Current methods of data preservation have worked for now, but new technology has enabled safer options for emergency services, organizations, and businesses to contend with natural disasters of Biblical proportions. 

## Classical Solution: Backup the Data 

{SOCIAL-MEDIA-FOLLOW/}

Relational databases are ideal for a primary server architecture. As the scale of the data increases, an enterprise typically will add capacity to their main server to handle the added data flow. The best way to back up the data would be with a physical copy, or a digital backup hosted separately.

This has been the incumbent solution, but there are inherent risks:

1. It takes time to load the backup to the machines still working. This costs money. According to Gartner research, data downtime can cost a business $42K per hour.

2. All data committed to the system from the time of the final backup to the time of the disruption is lost. Some companies update their backups at the end of business every day, some do it once a week. For wide stretches of time you have blocks of data that has no backup.

3. Most backup versions are saved without being encrypted creating a serious security problem. 

4. Maintaining an external backup contingency costs money. You have to constantly test your backup to make sure the copy isn't broken. 

## Holding Off Harvey With High Availability 

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

What if you didn't need a backup? What if you no longer need to keep all your data in one place?

Instead of backing up your data, you can replicate it. You can set up a cluster of servers in different locations that will replicate multiple copies of your database with every update you make.

According to Oren Eini, CEO of Hibernating Rhinos, the developer of the Raven 4.0 Database, “Geo distributed data allows you to set up a cluster of nodes to replicate your data throughout your organization. You can set up instances of your database on several servers, all located in different countries, cities, even parts of town, and connect these servers, or nodes, to a cluster that updates the database throughout all locations in real time.”

Oren adds, “If your node in Huston were to go down, the nodes in St. Louis and Singapore will keep running. If your primary node was the Texas server, in an instant your database can promote the node in Missouri to the lead role. It has all the data in the Houston node, and will replicate updates to the server in Singapore. Your users won't feel any interruption in service.“

Once the disabled node is restored, the working nodes will automatically update it with the latest state of the database. Only then will the node be “readmitted” back into the cluster.

High availability with distributed data keeps users connected all the time. This can be vital in emergency situations where access to information is not merely an issue of ones and zeroes, but a matter of life and death.

<p style="font-size: larger">RavenDB is among the few NoSQL databases that is both ACID and transactional. Built for speed, it pushes your server to the limits. Made for scale, RavenDB will accommodate the expansion of your data, and the success of your business. <a href="https://ravendb.net/downloads#server/dev">Take a free copy</a> and see for yourself!</p>