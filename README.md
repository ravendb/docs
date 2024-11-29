# RavenDB documentation

This repository contains documentation for [RavenDB](https://ravendb.net/docs).

## Found a bug?

Please create an issue at our [YouTrack](https://issues.hibernatingrhinos.com/issues/RDoc).

## Need help?

If you have any questions please visit our [community](https://github.com/ravendb/ravendb/discussions).

## Want to contribute?

You do not need anything special if you wish to modify or update an existing Markdown documentation file. You can even edit them straight on GitHub and submit a pull request.

For more comprehensive changes or additions, you will need:

- Visual Studio 2022
- .NET Framework 4.7.1 Developer Pack
- .NET Core 2.0 & 2.1
- RavenDB 4.2

### Setup

1. Clone the repo
1. Open `Raven.Documentation.sln` and build the solution
   - Visual Studio will warn you of any missing SDKs
   - You may need to install them and restart
1. Ensure your local RavenDB is up and running (e.g. `http://localhost:8080`)
   - It needs to be unauthenticated as the docs do not use a certificate
   - If you need to change the URL, edit `Raven.Documentation.Web/web.config` and set the `ServerUrl` setting
   - Take care not to commit this back to source control
   - You can use a Docker version of RavenDB if you are already running an instance for other work.
     - `docker run -p <port>:8080 ravendb/ravendb:4.2-ubuntu-latest`
     - Adjust forward port as necessary to not conflict

### Running Docs

1. Set the `Raven.Documentation.Web` as the Startup Project
1. Run the project (Ctrl-F5 to start without debugging)
1. Once running, in the bottom right of the browser, click "Development Tools" and then "Generate Documentation for {version}" (or for all versions)

## Language-specific docs inheritance between documentation versions

### Rules

1. If a documentation directory for a specific version does not contain any markdown file related to a documentation page, then all the documentation pages are inherited from the previous documentation version.
2. If a documentation directory contains any markdown file for a documentation page (for example - dotnet only), then the parser assumes that the current version has an entirely new set of documents. No document from the previous version is inherited.

This behavior ensures us that we don't show obsolete inherited documents for a documentation version. There is a reason why a new language-specific documentation page was introduced for a doc version (for example, the API was changed). This means that all other languages should be also treated as different from the previous version.

### Example

In the case of `client-api/data-subscriptions/creation/examples`, we have these files in the directories:

- v4.0: dotnet, java, js
- v4.1: dotnet, java
- v4.2: dotnet
- v5.0: no documents

So for v4.1 the parser assumes that the v4.1 version should support dotnet and java. No document from v4.0 is inherited.

The v5.0 directory does not contain any related document, so the documents are inherited from the previous version. v4.2 contains only dotnet document, so v5.0 will support dotnet only.

### Fixing missing language page

In order to fix the missing language page, the related markdown file should be copied from the previous version. The pasted file should be analyzed to indicate what needs to be updated for the current documentation version.

## Adding new documentation version

1. In your **File system**:  
   Create the following new empty directories under the `Documentation` directory.  
   For example, to create **6.1**:

    ```
    > 6.1
      > Raven.Documetation.Pages 
      > Samples
        > csharp
          > Raven.Documentation.Samples
        > java
        > nodejs
        > python
    ```

2. In your **File system**:   
   From the previous version, e.g. from 6.0, copy the existing `csproj` files to these new directories respectively:  
    * Copy `Raven.Documentation.Pages.csproj` to the `Raven.Documentation.Pages` folder.  
    * Copy `Raven.Documentation.Samples.csproj` to the `Raven.Documentation.Samples` folder.  
      
   From the previous version, also copy file `Northwind.cs` to the `Raven.Documentation.Samples` folder.

3. Edit the 2 csproj files in a text editor:  
   Update all occurrences of RavenDB `Version` to the matching version,  
   e.g.: `<PackageReference Include="RavenDB.Client" Version="6.1.0-nightly-20240709-0737" />`  

4. In your **Solution**:  
   Right click on the top `Documentation` folder, select "Add new solution folder", and create the `6.1` folder.  
   Right click on folder `6.1`, select "Add existing project", and add the `Raven.Documentation.Pages.csproj` file.  
   Right click on folder `6.1`, select "Add new solution folder", and create the `Samples` folder.  
   Right click on folder `6.1\Samples`, select "Add new solution folder", and create the `csharp` folder.  
   Right click on folder `6.1\Samples\csharp`, select "Add new existing project", and add the `Raven.Documentation.Samples.csproj` file.  

5. Open file `Models.cs` (under Raven.Documentation.Web.Core.ViewModels).  
   Add the new version to the `AllVersions` list.  

6. In your **file system**:  
   Open file `scripts/populateDocsJson.ps1` and update the params, e.g.:  
   ```
   param (
    [string] $FromVersion = "6.0",
    [string] $FromVersion = "6.1",
   )
   ```

7. Run `scripts/populateDocsJson.ps1` to populate the correct directory structure and `.docs.json` files in the newly added `Raven.Documentation.Pages` project.

8. Open file `RouteConfig.cs` (under Raven.Documentation.Web\App_Start).  
   Add the new version to the `RouteAvailableVersions` constant.  

## Changing document location between versions

For example, let's say in v4.1 there is a document introduced in the path `a/b`. It is present in this location also for v4.2. But in v5.0, it was moved to `c/d`.

### Mappings

First, you need to tell each version how to reach the new location for a moved document. This is done by adding document mappings.

In the example described above, you need to add the following mappings:

- in v4.1:

```
"Mappings": [
    {
        "Version": 5.0,
        "Key": "c/d"
    }
]
```

- in v4.2:

```
"Mappings": [
    {
        "Version": 5.0,
        "Key": "c/d"
    }
]
```

- in v5.0:

```
"Mappings": [
    {
        "Version": 4.1,
        "Key": "a/b"
    },
    {
        "Version": 4.2,
        "Key": "a/b"
    }
]
```

Basically, both v4.1 and v4.2 need to know where to go to when v5.0 is selected from the version dropdown. The same goes for v5.0 when you want to reach v4.1 or v4.2.

### Last supported version

The document bubbling algorithm assumes that each document will be present in the version it was introduced in and in all the consecutive versions. You can explicitly define the last supported version of an article in the given location to stop this behavior.

For this particular example, the article introduced in v4.1 in location `a/b` will be present in this location for v4.2, v5.0 etc. Assuming that the article was added to a new location in v5.0, you will end up with two articles: `5.0/a/b` and `5.0/c/d`. This will ignore the defined mappings.

In order to fix this, you need to tell the algorithm that the last supported version for location `a/b` is v4.2. You do this by adding the following entries to the related `.docs.json` file entries:

```
"LastSupportedVersion": "4.2"
```

This will tell the algorithm that it shouldn't generate the documentation page for v5.0 and all consecutive versions.

**Important:** you need to add the `LastSupportedVersion` entry to `.docs.json` for each version where a related documentation markdown file exists.

The algorithm skips the `LastSupportedVersion` analysis for the entries that don't have the markdown file present in the folder. If there are multiple markdown files for different versions, the algorithm will merge their supported versions. If there is no `LastSupportedVersion` entry for a given document page version, the algorithm will assume that all the consecutive versions are supported.

For example, let's assume that there are documentation files in both `4.1/a/b.markdown` and `4.2/a/b.markdown` locations. This means that you need to add the `"LastSupportedVersion": "4.2"` entries for both v4.1 and v4.2 `.docs.json` files. If you forget to do this for v4.2, the algorithm will assume that there is no supported version cap (for v4.2) and will generate the documentation pages for all consecutive versions.
