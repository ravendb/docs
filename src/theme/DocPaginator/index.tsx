import React, { type ReactNode } from "react";
import DocPaginator from "@theme-original/DocPaginator";
import type DocPaginatorType from "@theme/DocPaginator";
import type { WrapperProps } from "@docusaurus/types";
import { Icon } from "@site/src/components/Common/Icon";
import Link from "@docusaurus/Link";

type Props = WrapperProps<typeof DocPaginatorType>;

export default function DocPaginatorWrapper(props: Props): ReactNode {
    return (
        <>
            <div className="mt-[32px] text-left items-center">
                <Link
                    className="inline-flex items-center gap-2 text-sm"
                    to={"https://ravendb.net"}
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    <Icon icon="edit" size="xs" />
                    Edit on GitHub
                </Link>
            </div>
            <DocPaginator {...props} />
        </>
    );
}
