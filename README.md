# RavenDB documentation

This repository contains documentation for [RavenDB](https://ravendb.net/docs).

Found a bug?
------------
Please create an issue at our [YouTrack](https://issues.hibernatingrhinos.com/issues/RDoc).

Need help?
----------
If you have any questions please visit our [community group](http://groups.google.com/group/ravendb/).

## Adding new documentation version

1. Add `Documentation/[[version]]/Raven.Documentation.Pages/Raven.Documentation.Pages.csproj` project. Make sure that it references the correct RavenDB nuget packages versions.
2. Add `Documentation/[[version]]/Samples/csharp/Raven.Documentation.Samples/Raven.Documentation.Samples.csproj` project, similarly as above.
3. Add version to the `AllVersions` list in `Raven.Documentation.Web.Core.ViewModels.DocsVersion`.
4. Run `scripts/populateDocsJson.ps1` in order to populate the correct directory structure and `.docs.json` files in the newly added `Raven.Documentation.Pages` project. Please check the script parameters for details.
5. Add version to the `RouteConfig.RouteAvailableVersions` constant.
