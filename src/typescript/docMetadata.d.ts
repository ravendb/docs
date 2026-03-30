import type { DocFrontMatter, DocMetadata, DocContextValue } from "@docusaurus/plugin-content-docs";
import { DocsLanguage } from "@site/src/components/LanguageStore";
import { SeeAlsoItemType } from "@site/src/components/SeeAlso/types";
import { IconName } from "@site/src/typescript/iconName";

export interface GalleryImage {
    src: string;
    alt?: string;
}

export interface CustomDocFrontMatter extends DocFrontMatter {
    supported_languages?: DocsLanguage[];
    see_also?: SeeAlsoItemType[];
    author?: string;
    icon?: IconName;
    image?: string;
    publishedAt?: string;
    proficiencyLevel?: string;
    keywords?: string[];
    gallery?: GalleryImage[];
    challengesSolutionsTags?: string[];
    featureTags?: string[];
    techStackTags?: string[];
    category?: string;
    license?: string;
}

type CustomDocContextValue = Omit<DocContextValue, "frontMatter" | "metadata"> & {
    frontMatter: CustomDocFrontMatter;
    metadata: Omit<DocMetadata, "frontMatter"> & {
        frontMatter: CustomDocFrontMatter;
    };
};

declare module "@docusaurus/plugin-content-docs/client" {
    export function useDoc(): CustomDocContextValue;
}
