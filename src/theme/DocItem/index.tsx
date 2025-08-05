import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";
import { LanguageProvider } from "../../components/LanguageContext";
import wrapperStyles from "./DocItemWrapper.module.css";
import tocStyles from "../../components/EmptyToc.module.css";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
    return (
        <div className={wrapperStyles.wrapper}>
            <LanguageProvider>
                <div className={wrapperStyles.content}>
                    <DocItem {...props} />
                </div>
            </LanguageProvider>
            <div className={tocStyles.emptyToc}></div>
        </div>
    );
}