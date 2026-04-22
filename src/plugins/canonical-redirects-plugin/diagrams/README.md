# Diagrams

Mermaid source + generated SVGs for the plugin README.

## Regenerating

```bash
cd src/plugins/canonical-redirects-plugin/diagrams
npx -p @mermaid-js/mermaid-cli mmdc -i data-flow.mmd -o data-flow.svg -b transparent
npx -p @mermaid-js/mermaid-cli mmdc -i resolve-chain.mmd -o resolve-chain.svg -b transparent
```

Edit the `.mmd` source files, re-run the commands, commit both the source and the regenerated SVG.
