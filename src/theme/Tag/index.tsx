import React, { type ReactNode } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import type { Props as DocusaurusTagProps } from "@theme/Tag";

export interface Props
    extends Partial<DocusaurusTagProps>,
        // eslint-disable-next-line no-undef
        React.HTMLAttributes<HTMLSpanElement> {
    children?: React.ReactNode;
    to?: string;
    size?: "xs" | "default";
    className?: string;
}

export default function Tag({
    permalink,
    count,
    description,
    children,
    size = "default",
    className,
    ...props
}: Props): ReactNode {
    const isXs = size === "xs";

    const baseClasses = clsx(
        "inline-flex items-center select-none border",
        "bg-black/5 dark:bg-white/5 border-black/10 dark:border-white/10",
        "hover:bg-black/10 dark:hover:bg-white/10 hover:!no-underline",
        isXs
            ? ["font-medium rounded-full", "text-[11px] leading-4 h-5 px-2"]
            : [
                  "gap-2 px-2 py-1.5 rounded-xl",
                  "text-sm font-semibold leading-[normal]",
                  "!transition-colors",
              ],
    );

    const tagContent = (
        <>
            <span>{children}</span>
            {count !== undefined && (
                <span
                    className={clsx(
                        "flex items-center justify-center rounded-full font-bold leading-none",
                        "w-4 h-4 text-[9px]",
                        "bg-black/10",
                        "dark:bg-white/20 dark:text-white",
                    )}
                >
                    {count}
                </span>
            )}
        </>
    );

    if (permalink) {
        return (
            <Link
                href={permalink}
                rel="tag"
                title={description}
                className={clsx(
                    baseClasses,
                    "!text-inherit relative z-10",
                    className,
                )}
                {...props}
            >
                {tagContent}
            </Link>
        );
    }

    return (
        <span className={clsx(baseClasses, className)} {...props}>
            {tagContent}
        </span>
    );
}
