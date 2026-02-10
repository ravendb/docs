import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import EditMetaRow from "@theme/EditMetaRow";
import { HIDDEN_EDIT_PAGE_ROUTES } from "@site/src/typescript/hiddenEditPageRoutes";
import { DocsLanguage, useLanguage } from "@site/src/components/LanguageStore";
import SeeAlso from "@site/src/components/SeeAlso";
import Tag from "../../Tag";

const getEditUrlWithLanguage = (url: string, language: DocsLanguage, supportedLanguages: DocsLanguage[]) : string => {
    if (!supportedLanguages || supportedLanguages.length === 0) {
        return url;
    }

    const lastSlashIndex = url.lastIndexOf('/');
    const path = url.substring(0, lastSlashIndex + 1);
    const filename = url.substring(lastSlashIndex + 1).replace('.mdx', '');

    return `${path}_${filename}-${language}.mdx`;
};

export default function DocItemFooter(): ReactNode {
    const { language } = useLanguage();
    const { metadata } = useDoc();
    const { editUrl, lastUpdatedAt, lastUpdatedBy, tags, permalink, frontMatter } = metadata;
    const seeAlso = (frontMatter as any).seeAlso;

    const isPathHidden = HIDDEN_EDIT_PAGE_ROUTES.some((route) => {
        return permalink.endsWith(route);
    });

    const canDisplayTagsRow = tags.length > 0;
    const canDisplayEditMetaRow = !!editUrl && !isPathHidden;
    const canDisplaySeeAlso = seeAlso && seeAlso.length > 0;

    if (!canDisplayTagsRow && !canDisplayEditMetaRow && !canDisplaySeeAlso) {
        return null;
    }

    return (
        <footer className={clsx(ThemeClassNames.docs.docFooter, "mt-4")}>
            {canDisplayTagsRow && (
                <div className="flex flex-wrap gap-2">
                    {tags.map((tag) => (
                        <Tag key={tag.label} permalink={tag.permalink}>
                            {tag.label}
                        </Tag>
                    ))}
                </div>
            )}
            {canDisplayEditMetaRow && (
                <EditMetaRow
                    className={clsx(ThemeClassNames.docs.docFooterEditMetaRow)}
                    editUrl={getEditUrlWithLanguage(
                        editUrl,
                        language,
                        metadata.frontMatter.supported_languages,
                    )}
                    lastUpdatedAt={lastUpdatedAt}
                    lastUpdatedBy={lastUpdatedBy}
                />
            )}
            {canDisplaySeeAlso && <SeeAlso items={seeAlso} className="mb-6" />}
        </footer>
    );
}
