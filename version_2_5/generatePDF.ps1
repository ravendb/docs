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