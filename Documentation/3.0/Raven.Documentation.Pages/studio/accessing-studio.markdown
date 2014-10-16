# Accessing Studio

RavenDB comes with a Management Studio (aka Studio) that can be accessed by any modern browser using the server url (e.g. `http://localhost:8080/` when your server runs at 8080 port). The Studio lets you manage data, manage indexes, issue queries, perform various maintenance actions, and more in easier manner.

As said before, the Studio can be accessed on any RavenDB server, regardless of how it is deployed (you need to start the http server when Embedded instance is used). However, it does require the user to authenticate against the server, most commonly using their Windows credentials.

## First run & creating database

When you just deployed your server and there are no databases, following screen will show up:

![Figure 1. Studio. First run. Creating database.](images/accessing-studio-first-run.png)  

Here you can create your first database by specifying its name, list of active bundles (optionally) or more advanced settings like [storage engine]() or various paths.

## Selecting database

If your server contains one or more databases, the database creation windows will not show up. Instead of it, you will be navigated to database selection screen, when you can see all your databases, disable them or even delete.

![Figure 2. Studio. Selecting database.](images/accessing-studio-database-selection.png)  

## Navigation

If the database have been selected, then top navigation bar will be filled with following menu items that will navigate your to various parts of the Studio.

![Figure 3. Studio. Top navigation bar.](images/accessing-studio-nav.png)  

On the other hand, the bottom navigation bar contains information about server build number, license and basic statistics for selected database.

![Figure 4. Studio. Bottom navigation bar.](images/accessing-studio-nav-bottom.png)  



