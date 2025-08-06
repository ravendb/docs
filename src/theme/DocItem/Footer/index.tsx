import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import EditThisPage from "@site/src/theme/EditThisPage";

export default function DocItemFooter(): ReactNode {
  const { metadata } = useDoc();
  const { editUrl, lastUpdatedAt, lastUpdatedBy, tags } = metadata;

  const canDisplayTagsRow = tags.length > 0;
  const canDisplayEditMetaRow = !!(editUrl || lastUpdatedAt || lastUpdatedBy);

  const canDisplayFooter = canDisplayTagsRow || canDisplayEditMetaRow;

  if (!canDisplayFooter) {
    return null;
  }

  return (
    <footer
      className={clsx(
        ThemeClassNames.docs.docFooter,
        "docusaurus-mt-lg flex justify-end",
      )}
    >
      <EditThisPage editUrl={editUrl} />
    </footer>
  );
}
