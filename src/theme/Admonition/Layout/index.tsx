import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";

import type { Props as BaseProps } from "@theme/Admonition/Layout";

interface Props extends BaseProps {
  id?: string;
  href?: string;
}

function AdmonitionContainer({
  type,
  className,
  children,
}: Pick<Props, "type" | "className"> & { children: ReactNode }) {
  return (
    <div
      className={clsx(
        ThemeClassNames.common.admonition,
        ThemeClassNames.common.admonitionType(type),
        className,
      )}
    >
      {children}
    </div>
  );
}

function AdmonitionHeading({
  title,
  id,
  href,
}: Pick<Props, "title" | "id" | "href">) {
  return (
    <div>
      <a id={id} href={href}>
        {title}
      </a>
    </div>
  );
}

function AdmonitionContent({ children }: Pick<Props, "children">) {
  return children ? <div>{children}</div> : null;
}

export default function AdmonitionLayout(props: Props): ReactNode {
  const { type, title, children, className, id, href } = props;
  return (
    <AdmonitionContainer type={type} className={className}>
      {title ? <AdmonitionHeading title={title} id={id} href={href} /> : null}
      <AdmonitionContent>{children}</AdmonitionContent>
    </AdmonitionContainer>
  );
}
