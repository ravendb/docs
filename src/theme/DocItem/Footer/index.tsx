import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import TagsListInline from "@theme/TagsListInline";
import EditMetaRow from "@theme/EditMetaRow";
import { HIDDEN_EDIT_PAGE_ROUTES } from "@site/src/typescript/hiddenEditPageRoutes";

export default function DocItemFooter(): ReactNode {
    const { metadata } = useDoc();
    const { editUrl, lastUpdatedAt, lastUpdatedBy, tags, permalink } = metadata;

    const isPathHidden = HIDDEN_EDIT_PAGE_ROUTES.some((route) => {
        return permalink.endsWith(route);
    });

    const canDisplayTagsRow = tags.length > 0;
    const canDisplayEditMetaRow = !!editUrl && !isPathHidden;

    if (!canDisplayTagsRow && !canDisplayEditMetaRow) {
        return null;
    }

    return (
        <footer className={clsx(ThemeClassNames.docs.docFooter, "mt-6")}>
            {canDisplayTagsRow && (
                <div
                    className={clsx(
                        "row",
                        ThemeClassNames.docs.docFooterTagsRow,
                    )}
                >
                    <div className="col">
                        <TagsListInline tags={tags} />
                    </div>
                </div>
            )}
            {canDisplayEditMetaRow && (
                <EditMetaRow
                    className={clsx(ThemeClassNames.docs.docFooterEditMetaRow)}
                    editUrl={editUrl}
                    lastUpdatedAt={lastUpdatedAt}
                    lastUpdatedBy={lastUpdatedBy}
                />
            )}
        </footer>
    );
}
