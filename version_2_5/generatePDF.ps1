# pandoc docs\intro\index.markdown docs\intro\ravendb-in-a-nutshell.markdown -o RavenDB-2.5.pdf
# pandoc docs\intro\index.markdown safe-by-default.markdown docs\intro\ravendb-in-a-nutshell.markdown -o RavenDB-2.5.pdf
pandoc --latex-engine xelatex --template pdf-template.tex `
	docs\intro\index.markdown `
	docs\intro\what-is-nosql.markdown `
	docs\intro\what-is-a-document-database.markdown `
	docs\intro\ravendb-in-a-nutshell.markdown `
	docs\intro\basic-concepts.markdown  -o RavenDB-2.5.pdf
	#docs\intro\safe-by-default.markdown `
