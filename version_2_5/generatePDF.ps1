pandoc --latex-engine xelatex --template pdf-template.tex `
	docs\intro\index.markdown `
	docs\intro\what-is-nosql.markdown `
	docs\intro\what-is-a-document-database.markdown `
	docs\intro\ravendb-in-a-nutshell.markdown `
	docs\intro\basic-concepts.markdown `
	docs\intro\safe-by-default.markdown `
	docs\intro\quickstart\index.markdown `
	docs\intro\quickstart\adding-ravendb-to-your-application.markdown `
	docs\intro\quickstart\adding-ravendb-through-nuget.markdown `
	docs\intro\system-requirements.markdown `
	docs\intro\building-from-source.markdown `
	docs\intro\what-is-new.markdown `
	docs\theory\index.markdown `
	docs\theory\what-is-raven.markdown `
	docs\theory\document-key-generation.markdown `
	docs\theory\document-structure-design.markdown `
	docs\theory\ravendb-collections.markdown `
	docs\client-api\index.markdown `
	docs\client-api\connecting-to-a-ravendb-datastore.markdown `
	docs\client-api\basic-operations\understanding-session-object.markdown `
	docs\client-api\basic-operations\opening-a-session.markdown `
	docs\client-api\basic-operations\saving-new-document.markdown `
	docs\client-api\basic-operations\loading-editing-existing-document.markdown `
	docs\client-api\basic-operations\deleting-documents.markdown `
	docs\client-api\basic-operations\basic-querying.markdown `
	docs\client-api\basic-operations\customizing-behavior.markdown `
	docs\client-api\querying\index.markdown `
	docs\client-api\querying\using-linq-to-query-ravendb.markdown `
	docs\client-api\querying\paging.markdown `
	docs\client-api\querying\stale-indexes.markdown `
	docs\client-api\querying\static-indexes\index.markdown `
	docs\client-api\querying\static-indexes\defining-static-index.markdown `
	docs\client-api\querying\static-indexes\customizing-results-order.markdown `
	docs\client-api\querying\static-indexes\configuring-index-options.markdown `
	docs\client-api\querying\static-indexes\indexing-hierarchies.markdown `
	docs\client-api\querying\static-indexes\indexing-related-documents.markdown `
	docs\client-api\querying\static-indexes\live-projections.markdown `
	docs\client-api\querying\static-indexes\indexes-error-handling.markdown `
	docs\client-api\querying\static-indexes\searching.markdown `
	docs\client-api\querying\static-indexes\spatial-search.markdown `
	docs\client-api\querying\static-indexes\boosting.markdown `
	docs\client-api\querying\static-indexes\suggestions.markdown `
	-o RavenDB-2.5.pdf


## TODO: Table of Contents
## TODO: styling code
## TODO: figure out how to only refer to images from one folder
## TODO: fix everywhere I'm using <pre></pre>
## why you need to have the first line in the file with a new line for it to continue
## figures have wrong figure #

## Table of contents?
## TODO: ## THEORY
## TODO: Client-API
## TODO: Client-API - BASIC OPERATIONS
## TODO: Client-API - QUERYING
## TODO: Client-API - ADVANCED
## TODO: HTTP-API
## TODO: HTTP-API - EXTENSIONS
## TODO: HTTP-API - INDEXES
## TODO: RavenLight
## TODO: studio
## TODO: studio - BUNDLES
## TODO; server
## TODO; server - Administration
## TODO; server - Deployment
## TODO; server - Extending
## TODO; server - Scaling Out
## TODO: Schema-Design (new - dom)
## TOOD: FAQ
## TODO: Appendixes