import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";
import DocsTopbar from "@site/src/components/DocsTopbar";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
    const title = props.content.metadata?.title;
    const source = props.content.metadata?.source as string | undefined;

    const isDocsOrVersioned =
        source?.startsWith("@site/docs/") ||
        source?.startsWith("@site/versioned_docs/") ||
        source?.startsWith("docs/") ||
        source?.startsWith("versioned_docs/");
    const fileName = source?.split("/").pop();
    const isHomepage = fileName === "home.mdx";
    const isExcluded = isHomepage || fileName === "whats-new.mdx";

    const supportedLanguages =
        (props.content as any)?.supportedLanguages ||
        (props.content as any)?.frontMatter?.supportedLanguages ||
        (props.content as any)?.exports?.supportedLanguages;

    const showTopbar = Boolean(isDocsOrVersioned && !isExcluded);

    return (
        <>
            {showTopbar && (
                <DocsTopbar
                    title={title}
                    supportedLanguages={supportedLanguages}
                />
            )}
            <div className="wrapper row">
                <div className="col flex-1 min-w-0">
                    <DocItem {...props} />
                </div>
                {!isHomepage && <div className="col col--3 lg:block"></div>}
            </div>
        </>
    );
}
