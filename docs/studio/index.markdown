# The Management Studio

The RavenDB Management Studio is a lightweight Silverlight client to let you easily manage the data in a RavenDB server instance. Using the Studio you can view, create, edit or delete documents, manage indexes, issue queries, view the errors log, import / export, and more.

The Studio is accessible from any RavenDB server, regardless of how it is deployed. However, it does require the user to authenticate against the server, most commonly using his Windows credentials.

Assuming that you are running RavenDB on port 8080 (the default when running in debug / service mode), you can access the studio by pointing your browser to http://localhost:8080.

// TODO: Studio screenshot

## Basic navigation

At the top left of the page there are the navigation tabs, a link for each of the screens in the Studio:

![](Images/studio_base_1.PNG)  

Some database-wide statistics are shown at the bottom of the screen:

![](Images/studio_base_3.PNG)

There you will also find the Licensing status and the build number:  
![](Images/studio_base_4.PNG)

## The Summary Screen

The summary screen is the default screen shown whenever you access the RavenDB Management Studio, showing the latest documents updated (or created) in the database.

![](Images/studio_summery_2.PNG)

When accessing an empty database (or tenant) the page will look like this:

![](Images/studio_summery_1.PNG)

Clicking on "Create sample data" will add some dummy MvcMusicStore data and indexes to the database. This is usually helpful to jumpstart the RavenDB learning process.

## Features overview

{FILES-LIST /}