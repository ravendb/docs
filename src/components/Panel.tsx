import clsx from "clsx";
import React from "react";
import Heading from "@theme/Heading";

export type PanelProps = {
  children: React.ReactNode;
  className?: string;
  flush?: boolean;
  heading: string;
  headingLevel?: 1 | 2 | 3 | 4 | 5 | 6;
};

export function Panel(props: PanelProps) {
  const { children, className, flush, heading, headingLevel = 2 } = props;
  const headingTag = (`h${headingLevel}` as any);

  return (
    <section className={clsx("panel", flush ? "" : "my-4", className)}>
      <Heading as={headingTag} className="panel__heading">{heading}</Heading>
      <div className="panel__body">
        {children}
      </div>
    </section>
  );
}

export default Panel;
