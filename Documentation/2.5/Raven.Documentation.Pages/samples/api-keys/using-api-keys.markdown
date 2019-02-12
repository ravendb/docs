# Using API Keys With RavenDB

Api keys are used when trying to authorize with OAuth.

Here are the steps needed in order to set the api keys:  

1) Open the browser  
2) Navigate to: http://localhost:{your server port}  
3) Go to the Databases section and select "System Database" on the top right  
![Select System Database](Images/apikeys_1.PNG)  
4) Go to the settings (press the cog wheel next to the database name)  
![Go to the Settings](Images/apikeys_2.PNG)  
5) In the Api Keys section Add a new key, select a name, generate a secret and add a database for your database  
![Add New API Key](Images/apikeys_3.PNG)  
6) Don't forget to click the "Save Changes" button"  
![Save Changes](Images/apikeys_4.PNG)  

A few tips:  

- In order to set a key to all databases in the database name enter "*" (this will NOT grant access to the system database) 
- To grant access to the system database set the database name to "&lt;system&gt;".

Once all settings are entered give the user the "full api key" (you can right-click on it to copy).  
![Copy API Key to Clipboard](Images/apikeys_5.PNG)  

In order to use this api key the user needs to set the api key when settings a new DocumentStore like this:  

{CODE apikeys1@Samples\ApiKeys\ApiKeys\Program.cs /}