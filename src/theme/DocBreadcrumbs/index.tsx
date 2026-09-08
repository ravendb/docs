import React, { type ReactNode } from "react";
import DocBreadcrumbs from "@theme-original/DocBreadcrumbs";
import type DocBreadcrumbsType from "@theme/DocBreadcrumbs";
import type { WrapperProps } from "@docusaurus/types";
import { useLocation } from "@docusaurus/router";

type Props = WrapperProps<typeof DocBreadcrumbsType>;

// Pages that render without the default breadcrumb, matched in both trailing-slash forms:
// - /samples: the hub injects its own richer 2-item `CollectionPage` breadcrumb
//   (RavenDB Documentation -> Samples) via SamplesHomePage. Docusaurus' default DocBreadcrumbs
//   would additionally emit a 1-item `BreadcrumbList` (just "Samples"), producing two
//   conflicting BreadcrumbList structured-data blocks on the same page.
// - /quill: the start page is the root of its section, so a one-item "Quill" trail only
//   repeats the sidebar's "Home" entry.
// Every other doc page, including the sample detail pages, keeps its normal breadcrumb.
const HIDDEN_ON = new Set(["/samples", "/quill"]);

export default function DocBreadcrumbsWrapper(props: Props): ReactNode {
    const { pathname } = useLocation();
    const normalized = pathname.length > 1 ? pathname.replace(/\/+$/, "") : pathname;
    if (HIDDEN_ON.has(normalized)) {
        return null;
    }
    return <DocBreadcrumbs {...props} />;
}
