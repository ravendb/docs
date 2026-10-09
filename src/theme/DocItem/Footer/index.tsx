import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useActivePlugin, useDoc } from "@docusaurus/plugin-content-docs/client";
import EditMetaRow from "@theme/EditMetaRow";
import { HIDDEN_EDIT_PAGE_ROUTES } from "@site/src/typescript/hiddenEditPageRoutes";
import { DocsLanguage, useLanguage } from "@site/src/components/LanguageStore";
import SeeAlso from "@site/src/components/SeeAlso";
import GuideCtaFooter from "@site/src/components/Guides/GuideCtaFooter";

const getEditUrlWithLanguage = (url: string, language: DocsLanguage, supportedLanguages: DocsLanguage[]): string => {
    if (!supportedLanguages || supportedLanguages.length === 0) {
        return url;
    }

    const lastSlashIndex = url.lastIndexOf("/");
    const path = url.substring(0, lastSlashIndex + 1);
    const filename = url.substring(lastSlashIndex + 1).replace(".mdx", "");

    return `${path}content/_${filename}-${language}.mdx`;
};

export default function DocItemFooter(): ReactNode {
    const language = useLanguage();
    const { metadata } = useDoc();
    const { editUrl, lastUpdatedAt, lastUpdatedBy, tags, permalink, frontMatter } = metadata;
    const { see_also, external_url } = frontMatter;
    const pluginId = useActivePlugin()?.pluginId;

    const isPathHidden = HIDDEN_EDIT_PAGE_ROUTES.some((route) => {
        return permalink.endsWith(route);
    });

    const canDisplayTagsRow = tags.length > 0;
    const canDisplayEditMetaRow = !!editUrl && !isPathHidden;
    const canDisplaySeeAlso = see_also && see_also.length > 0;
    // Hosted guide articles only: not the guides home page, and not external guides, whose stub has no body.
    const canDisplayGuideCta = pluginId === "guides" && metadata.id !== "home" && !external_url;

    if (!canDisplayTagsRow && !canDisplayEditMetaRow && !canDisplaySeeAlso && !canDisplayGuideCta) {
        return null;
    }

    return (
        <footer className={clsx(ThemeClassNames.docs.docFooter, "mt-4")}>
            {canDisplayEditMetaRow && (
                <EditMetaRow
                    className={clsx(ThemeClassNames.docs.docFooterEditMetaRow)}
                    editUrl={getEditUrlWithLanguage(editUrl, language, metadata.frontMatter.supported_languages)}
                    lastUpdatedAt={lastUpdatedAt}
                    lastUpdatedBy={lastUpdatedBy}
                />
            )}
            {/* The CTA closes the article, so it comes before the reference list rather than after it. */}
            {canDisplayGuideCta && <GuideCtaFooter className="mt-8 mb-6" />}
            {canDisplaySeeAlso && <SeeAlso items={see_also} className="mb-6" />}
        </footer>
    );
}
