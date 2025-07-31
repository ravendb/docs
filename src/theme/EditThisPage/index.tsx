import React, { type ReactNode } from "react";
import Translate from "@docusaurus/Translate";
import Link from "@docusaurus/Link";
import IconEdit from "@theme/Icon/Edit";
import type { Props } from "@theme/EditThisPage";
import Button from "@site/src/components/Common/Button";

export default function EditThisPage({ editUrl }: Props): ReactNode {
  return (
    <Button url={editUrl} variant="outline" size="sm">
      🐱 Edit on GitHub
    </Button>
  );
}
