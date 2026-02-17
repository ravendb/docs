import type { DocFrontMatter, DocMetadata, DocContextValue } from '@docusaurus/plugin-content-docs';
import { DocsLanguage } from "@site/src/components/LanguageStore";

export interface CustomDocFrontMatter extends DocFrontMatter {
    supported_languages?: DocsLanguage[];
}

type CustomDocContextValue = Omit<DocContextValue, 'frontMatter' | 'metadata'> & {
    frontMatter: CustomDocFrontMatter;
    metadata: Omit<DocMetadata, 'frontMatter'> & {
        frontMatter: CustomDocFrontMatter;
    };
}

declare module '@docusaurus/plugin-content-docs/client' {
    export function useDoc(): CustomDocContextValue;
}
