import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";
import DocsTopbar from "@site/src/components/DocsTopbar";
import { CustomDocFrontMatter } from "@site/src/typescript/docMetadata";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
    const title = props.content.metadata?.title;
    const source = props.content.metadata?.source as string | undefined;
    const frontMatter = props.content.frontMatter as CustomDocFrontMatter;

    const isDocsOrVersioned =
        source?.startsWith("@site/docs/") ||
        source?.startsWith("@site/versioned_docs/") ||
        source?.startsWith("docs/") ||
        source?.startsWith("versioned_docs/");
    const fileName = source?.split("/").pop();
    const isHomepage = fileName === "home.mdx";
    const isExcluded = isHomepage || fileName === "whats-new.mdx";

    const supportedLanguages = frontMatter.supported_languages;

    const showTopbar = Boolean(isDocsOrVersioned && !isExcluded);

    return (
        <>
            {showTopbar && <DocsTopbar title={title} supportedLanguages={supportedLanguages} />}
            <div className="wrapper row">
                <div className="col flex-1 min-w-0">
                    <DocItem {...props} />
                </div>
            </div>
        </>
    );
}
