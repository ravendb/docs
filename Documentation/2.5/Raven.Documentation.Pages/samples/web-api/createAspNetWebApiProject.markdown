# Create ASP.NET Web API Project with RavenDB
In this section we will go over the steps to creating you own ASP.NET Web API Application.

## Step by Step Instructions
1) Make sure you have ASP.NET Web API installed.  
2) In visual studio and create a new "ASP.NET MVC 4 Web Application" project.  
3) As Project template select "Web API".  
4) Add the NuGet Package named "RavenDB Client".  
5) Create the following controller:

{CODE create_asp_net_web_api_project_1@Samples\WebApi\CreateAspNetWebApiProject.cs /}

6) From now on write you application as you would normally but Inherit from RavenDbController in any controller you want to contact RavenDB

Example of a controller: 

{CODE create_asp_net_web_api_project_2@Samples\WebApi\CreateAspNetWebApiProject.cs /}
