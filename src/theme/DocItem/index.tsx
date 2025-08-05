import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";
import { LanguageProvider } from "../../components/LanguageContext";
import tocStyles from "../../components/EmptyToc.module.css";
import clsx from "clsx";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
  const title = props.content.metadata?.title;
  const isHomePage =
    title === "RavenDB Documentation" ||
    title === "RavenDB Cloud Documentation";

  return (
    <div className="wrapper row">
      <LanguageProvider>
        <div className="col">
          <DocItem {...props} />
        </div>
      </LanguageProvider>
      {!isHomePage && (
        <div className={clsx(tocStyles.emptyToc, "col col--3 lg:block")}></div>
      )}
    </div>
  );
}
