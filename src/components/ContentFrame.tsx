import clsx from "clsx";
import React from "react";

export type ContentFrameProps = {
    children: React.ReactNode;
    className?: string;
    flush?: boolean;
};

export function ContentFrame(props: ContentFrameProps) {
    const { children, className, flush } = props;

    return <div className={clsx("content-frame", flush ? "" : "my-4", className)}>{children}</div>;
}

export default ContentFrame;
