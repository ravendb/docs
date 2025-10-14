import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
  const title = props.content.metadata?.title;
  const isHomePage =
    title === "RavenDB Documentation" ||
    title === "RavenDB Cloud Documentation";

    return (
    <div className="wrapper row">
      <div className="col flex-1 min-w-0">
        <DocItem {...props} />
      </div>
      {!isHomePage && <div className="col col--3 lg:block"></div>}
    </div>
  );
}
