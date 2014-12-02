# Creating ASP.NET MVC 4 Project with RavenDB
In this section we will go over the steps to creating you own ASP.NET MVC4 Application.

## Prerequisites
1) Make sure you have the Microsoft ASP.NET MVC 4 installed.  
2) You have installed NuGet package manager
3) You are running Visual Studio 2010 or greater

## Step by Step Instructions
1) Open the Micrsooft Visual Studio IDE.  Create a new "ASP.NET MVC 4 Web Application" project.  
2) As Project template select "Basic".  
3) Add the NuGet Package named "RavenDB Client".  
4) Create the following controller:

{CODE create_asp_net_mvc_4_project_1@Samples\Mvc\CreateAspNetMvc4Project.cs /}

5) From now on, write you application as you would normally, but make any controller that needs to talk to RavenDB inherit from RavenController.

Example of a controller: 

{CODE create_asp_net_mvc_4_project_2@Samples\Mvc\CreateAspNetMvc4Project.cs /}
