# Additional sources

Additional sources are refrences to source code that your index will be compiled with.  
This allows you to deploy custome logic that is hard to express with linq.  

## How to include additional sources

Additional sources are a property of the `AbstractIndexCreationTask` class and should be defined in the constructor of your index class.  
See the code example below on how to define addional sources in your index:  
{CODE additional_sources_1@Indexes\AdditionalSources.cs /}

{INFO:Referencing dlls}
If you are Referencing external dlls you would need to deploy them yourself to the folder where Raven.Server.exe reside.  
{INFO/}
