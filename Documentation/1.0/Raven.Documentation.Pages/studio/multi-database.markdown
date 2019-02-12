# Multi-Database Support

RavenDB supports multi-tenancy, and the Management Studio provides an easy way to manage multiple tenants from the same UI, and to create new tenants.

{NOTE You cannot delete a database using the Management Studio. /}

At the top right corner of each screen, by the RavenDB logo, the name of the current tenant you are working against is shown. Clicking on it will open a list of other available tenants, and selecting another one from that list will shift the Studio to working against that tenant.

![Select Tenant](Images/studio_base_2.PNG)

## Creating a new tenant

When pressing on the link named "Databases" on the to right of each page a new page will load:

![Databases Fig 1](Images/studio_databases_1.PNG)

On the top right you can create a new database:

![Databases Fig 2](Images/studio_databases_2.PNG)

After selecting this option a window will pop up and you need to enter a name for the new database.

{NOTE Each database must have a unique name /e}

![Databases Fig 3](Images/studio_databases_3.PNG)

Once you have selected the name a new empty database will be created. The name of the selected database will appear on the top right next the databases link:

![Databases Fig 4](Images/studio_databases_4.PNG)

Once you have more the one database you can switch between them from any page by clicking on the name of the active database and a list of possible databases will open, click on the database you want to view next.