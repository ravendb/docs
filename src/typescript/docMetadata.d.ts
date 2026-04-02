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
    img_alt?: string;
    published_at?: string;
    proficiency_level?: string;
    keywords?: string[];
    gallery?: GalleryImage[];
    challenges_solutions_tags?: string[];
    feature_tags?: string[];
    tech_stack_tags?: string[];
    category?: string;
    license?: string;
    license_url?: string;
    repository_url?: string;
    languages?: string[];
    external_url?: string;
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
