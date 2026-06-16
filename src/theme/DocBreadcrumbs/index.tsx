import React, { type ReactNode } from "react";
import DocBreadcrumbs from "@theme-original/DocBreadcrumbs";
import type DocBreadcrumbsType from "@theme/DocBreadcrumbs";
import type { WrapperProps } from "@docusaurus/types";
import { useLocation } from "@docusaurus/router";

type Props = WrapperProps<typeof DocBreadcrumbsType>;

// The /samples hub injects its own richer 2-item `CollectionPage` breadcrumb
// (RavenDB Documentation -> Samples) via SamplesHomePage. Docusaurus' default
// DocBreadcrumbs additionally emits a 1-item `BreadcrumbList` (just "Samples"),
// producing two conflicting BreadcrumbList structured-data blocks on the same page.
// Suppress the default on the hub only — every other doc page, including the sample
// detail pages, keeps its normal breadcrumb. Matches both trailing-slash forms.
export default function DocBreadcrumbsWrapper(props: Props): ReactNode {
    const { pathname } = useLocation();
    if (pathname === "/samples" || pathname === "/samples/") {
        return null;
    }
    return <DocBreadcrumbs {...props} />;
}
