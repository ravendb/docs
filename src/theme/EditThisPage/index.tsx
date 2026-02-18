import React, { type ReactNode } from "react";
import type { Props } from "@theme/EditThisPage";
import { Icon } from "@site/src/components/Common/Icon";
import Link from "@docusaurus/Link";

export default function EditThisPage({ editUrl }: Props): ReactNode {
    return (
        <div className="me-auto">
            <Link className="inline-flex items-center gap-2 text-sm leading-none" to={editUrl}>
                <Icon icon="edit" size="xs" />
                Edit on GitHub
            </Link>
        </div>
    );
}
