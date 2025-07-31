import React, { type ReactNode } from "react";
import DocItem from "@theme-original/DocItem";
import type DocItemType from "@theme/DocItem";
import type { WrapperProps } from "@docusaurus/types";
import { LanguageProvider } from "../../components/language-context";

type Props = WrapperProps<typeof DocItemType>;

export default function DocItemWrapper(props: Props): ReactNode {
  return (
    <LanguageProvider>
      <DocItem {...props} />
    </LanguageProvider>
  );
}
