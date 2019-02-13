#Profiling ASP.NET MVC application using RavenProfiler

To setup profiling in RavenDB using `RavenProfiler` you must follow few simple steps defined below:   
1\.	Add `Raven.Client.MvcIntegration` assembly to MVC project. Alternative way is to install NuGet package that can be found [here](https://www.nuget.org/packages/RavenDB.Client.MvcIntegration).   
2\.	Next step is to initialize profiler on our document store. This can be achieved by adding following code in `Global.asax.cs` file.   

{CODE profiling_mvc_application_1@Appendixes\ProfilingMvcApplication.cs /}

{CODE profiling_mvc_application_2@Appendixes\ProfilingMvcApplication.cs /}

3\.	Finally add `@Raven.Client.MvcIntegration.RavenProfiler.CurrentRequestSessions()` to `_Layout.cshtml`.   

##Viewing profiling results

To demonstrate profiler capabilities, lets create simple ASP.NET MVC 4 application and setup profiling in it.

1\.	How to setup ASP.NET MVC 4 application to use RavenDB can be found [here](../samples/mvc/createaspnetmvc4project?version=2.0).   
2\.	Setup profiling using steps from section above.   
3\.	Add following code to `HomeController`:   

{CODE profiling_mvc_application_5@Appendixes\ProfilingMvcApplication.cs /}

4\. Create `Index` view that will list all users.   
5\. Create `Create` view that will be used to add new user.   
6\. Add following `User` model:   

{CODE profiling_mvc_application_6@Appendixes\ProfilingMvcApplication.cs /}

7\. Create new user e.g. `Ayende` with password `Rahien`.  
8\. View profiling results in `RavenProfiler`, that can be found in upper left corner of the page.   

![Figure 1: `RavenProfiler` window](Images/profiling_mvc_application_1.PNG)

![Figure 2 Request Details in `RavenProfiler`](Images/profiling_mvc_application_3.PNG)

Various information can be pulled from the profiler, starting from request duration, method, url and ending with exact request response results.

### Excluding sensitive information

There are vast number of situations, where user does not want to include all the data in profiling information. For such scenarios we have included an option to filter out specific field values from results from provided list of fields.

In our previous results you may have noticed that user password has been displayed in profiling results. To filter it out, just pass array of fields that you want to filter as a second parameter of `RavenProfiler.InitializerFor` method.

{CODE profiling_mvc_application_7@Appendixes\ProfilingMvcApplication.cs /}

As a result of this setting, our profiling results will now hide data from all fields with name `Password`.

![Figure 3: Request Details in `RavenProfiler` with filter out `Password`](Images/profiling_mvc_application_2.PNG)

##Profiling only in debug mode

To enable profiling only in debug mode one can use [Preprocessor Directives](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives/index) or [ConditionalAttribute](https://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute.aspx).

To use `Preprocessor Directives` simply wrap `InitializeRavenProfiler` method execution with `#if` directive:

{CODE profiling_mvc_application_3@Appendixes\ProfilingMvcApplication.cs /}

If you want to use `ConditionalAttribute` then do as follows:

{CODE profiling_mvc_application_4@Appendixes\ProfilingMvcApplication.cs /}

The `InitializeRavenProfiler` method will not be executed unless `DEBUG` constant is specified.
