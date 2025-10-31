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

  function slugify(value: string): string {
    return value
      .toLowerCase()
      .trim()
      .replace(/[`~!@#$%^&*()_+=[\]{}|;:'",.<>/?]+/g, "")
      .replace(/\s+/g, "-")
      .replace(/-+/g, "-");
  }

  const id = slugify(heading);
  const headingTag = (`h${headingLevel}` as any);

  return (
    <section className={clsx("panel", flush ? "" : "my-4", className)}>
      <Heading as={headingTag} id={id} className="panel__heading">{heading}</Heading>
      <div className="panel__body">
        {children}
      </div>
    </section>
  );
}

export default Panel;
