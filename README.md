# RavenDB Documentation

This documentation is built using [Docusaurus](https://docusaurus.io/), an open-source modern static website generator.

## Cloning the repository

To clone the repository and checkout the `main` branch:

```bash
git clone https://github.com/ravendb/docs.git
cd docs
git checkout main
```

## Contributing

1. Open the markdown file you want to change, commit the changes, and create a pull request.  
2. Describe the changes made in the commits and pull request description.

## Installation

```bash
npm install
```

## Local development

```bash
npm run start
```

This command starts a local development server and opens up a browser window.  
Most changes are reflected live without having to restart the server (hot reload).

## Build

```bash
docusaurus build
```

This command generates static content into the `build` directory and can be served using any static contents hosting service.

Note it may be necessary to increase Node.js max memory usage, e.g. to 8 GB  

```bash
$env:NODE_OPTIONS="--max-old-space-size=8192"
```

## Serving static content

Static content can be served using a static file server, such as [serve](https://www.npmjs.com/package/serve):

```bash
npm run serve
```

## Documentation structure

The documentation is organized into two main directories
- `docs`: Contains the documentation files for latest RavenDB version.
- `versioned_docs`: Contains the documentation files for all other RavenDB versions.

## Adding new version

```bash
npm run docusaurus docs:version version_label
```

This command creates a new version of the documentation by adding `version-version_label` subdirectory to `versioned_docs` directory, which contains a snapshot of `docs` directory.

## Modifying latest version

To modify the latest version of the documentation, you can edit the files in the `docs` directory. 

Any changes made here will be reflected in the latest version of the documentation.

## Modifying previous versions

To modify a previous version of the documentation, you can edit the files in the `versioned_docs/version-version_label` directory.

Versions are entirely separate, so changes made for one version will not affect other versions.

## Docusaurus components

- `Tabs` and `TabItem`: Create tabbed content for organizing related information.
  ```javascript
  <Tabs>
  <TabItem value="rql" label="RQL">
    // RQL Query
    from Orders
    where Total > 100
  </TabItem>
  <TabItem value="linq" label="LINQ">
    // LINQ C# code
    var orders = session
    .Query<Order>()
    .Where(x => x.Total > 100)
    .ToList();
  </TabItem>
  </Tabs>
  ```
  
- `CodeBlock`: Displays formatted code snippets with syntax highlighting:
  ```javascript
  <CodeBlock language="csharp">
  {`
    // Call 'Execute' directly on the index instance
    new Orders_ByTotal().Execute(store);
  `}
  </CodeBlock>
  ```
  
  Or use triple backticks (```) with a language identifier to create a fenced code block with syntax highlighting:
  ````markdown
  ```csharp
  // Call 'Execute' directly on the index instance
  new Orders_ByTotal().Execute(store);
  ```
  ````
  
- `Admonition`: Shows styled callout boxes to highlight important content.  
  Available types: note, warning, info, tip, danger.
  ```javascript
  <Admonition type="note" title="Important">
    Remember to back up your data before making major changes.
  </Admonition>
  ```
  
## Custom components

Apart from the standard Docusaurus components, this documentation uses the following custom components 

- `LanguageSwitcher`: A component responsible for handling switching between different language versions of the documentation.  
   Languages supported by particular document are stored in `supportedLanguages` array.
    ```javascript
    export const supportedLanguages = ["csharp", "java", "nodejs"];
  
    <LanguageSwitcher supportedLanguages={supportedLanguages} />
    ```
  
- `LanguageContent`: A component that displays documentation content based on the selected language.
    ```html
    <LanguageContent language="csharp">
        <!-- Content specific to C# language -->
    </LanguageContent>
    ```

## Page metadata and sidebar settings

Each `.mdx` file begins with a block delimited by `---` that defines metadata used by Docusaurus for rendering, sidebar navigation, and page behavior.

For example:

```
---
title: "Enable Data Archiving"
hide_table_of_contents: true
sidebar_label: "Enable Data Archiving"
sidebar_position: 1
---
```

Key fields:  

* `title`: The document’s title metadata. This is used in the browser tab and SEO,  
     and is also shown as the main heading only if you don’t provide your own `# Heading` in the MDX body.
* `sidebar_label`:  Sets the text displayed for this page in the left sidebar menu.
* `sidebar_position`: Controls the article’s position in the sidebar.  
     Use this to place the article relative to others in the same folder or section.  
     Lower numbers appear higher in the menu.

## Category metadata for subfolders

When adding a new subfolder, you must create a `_category_.json` file to define how the section appears in the sidebar.

Example of a `_category_.json` file:

```
{
  "position": 4,
  "label": "Data Archival"
}
```

Key fields:

* `label`: Sets the name of the folder as it will appear in the sidebar.
* `position` – Controls the folder’s order relative to sibling items in the sidebar. Lower numbers appear higher.
