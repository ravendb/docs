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
