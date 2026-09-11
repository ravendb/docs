import clsx from "clsx";
import React from "react";

export type PanelProps = {
    children: React.ReactNode;
    className?: string;
    flush?: boolean;
    /** Set by the remark-panel-headings plugin: the heading arrives as the first child. */
    headingInContent?: boolean;
};

function splitHeadingFromBody(children: React.ReactNode): [React.ReactNode, React.ReactNode] {
    const [first, ...rest] = React.Children.toArray(children);
    return React.isValidElement(first) ? [first, rest] : [null, children];
}

export function Panel(props: PanelProps) {
    const { children, className, flush, headingInContent } = props;
    const [heading, body]: [React.ReactNode, React.ReactNode] = headingInContent
        ? splitHeadingFromBody(children)
        : [null, children];

    return (
        <section className={clsx("panel", flush ? "" : "my-4", className)}>
            {heading}
            <div className="panel__body">{body}</div>
        </section>
    );
}

export default Panel;
