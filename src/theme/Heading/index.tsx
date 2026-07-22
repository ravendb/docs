import React, { useCallback, useEffect, useRef, useState, type ReactNode } from "react";
import clsx from "clsx";
import { translate } from "@docusaurus/Translate";
import { useAnchorTargetClassName } from "@docusaurus/theme-common";
import useBrokenLinks from "@docusaurus/useBrokenLinks";
import type { Props } from "@theme/Heading";

// navigator.clipboard needs a secure context (HTTPS/localhost); fall back otherwise.
async function copyToClipboard(text: string): Promise<void> {
    if (typeof navigator !== "undefined" && navigator.clipboard) {
        return navigator.clipboard.writeText(text);
    }
    const { default: copy } = await import("copy-text-to-clipboard");
    copy(text);
}

function getAnchorUrl(id: string): string {
    const { origin, pathname, search } = window.location;
    return `${origin}${pathname}${search}#${id}`;
}

export default function Heading({ as: As, id, ...props }: Props): ReactNode {
    const brokenLinks = useBrokenLinks();
    const anchorTargetClassName = useAnchorTargetClassName(id);
    const [copied, setCopied] = useState(false);
    const copyTimeout = useRef<number | undefined>(undefined);

    const copyLink = useCallback(
        (event: React.MouseEvent<HTMLAnchorElement>) => {
            if (!id) {
                return;
            }
            // Update the hash (no new history entry) and copy the full link.
            event.preventDefault();
            window.history.replaceState(null, "", `#${id}`);
            copyToClipboard(getAnchorUrl(id)).then(() => {
                setCopied(true);
                copyTimeout.current = window.setTimeout(() => setCopied(false), 1500);
            });
        },
        [id]
    );

    useEffect(() => () => window.clearTimeout(copyTimeout.current), []);

    // H1 has no anchor link (not in the TOC).
    if (As === "h1" || !id) {
        return <As {...props} id={undefined} />;
    }

    brokenLinks.collectAnchor(id);

    const anchorTitle = translate(
        {
            id: "theme.common.headingLinkTitle",
            message: "Direct link to {heading}",
            description: "Title for link to heading",
        },
        {
            heading: typeof props.children === "string" ? props.children : id,
        }
    );
    const copiedTitle = translate({
        id: "theme.common.headingLinkCopied",
        message: "Link copied!",
        description: "Feedback shown after copying a heading link to the clipboard",
    });

    return (
        <As {...props} className={clsx("anchor", anchorTargetClassName, props.className)} id={id}>
            {props.children}
            <a
                className="hash-link relative"
                href={`#${id}`}
                onClick={copyLink}
                aria-label={anchorTitle}
                title={copied ? copiedTitle : anchorTitle}
                translate="no"
            >
                &#8203;
                <span
                    className={clsx(
                        "pointer-events-none absolute bottom-full left-1/2 z-10 mb-2 -translate-x-1/2",
                        "whitespace-nowrap rounded-md border px-2.5 py-1 text-xs font-medium shadow-lg backdrop-blur",
                        "border-black/10 bg-white/90 text-gray-700",
                        "dark:border-white/10 dark:bg-[#1b1b1d]/90 dark:text-gray-200",
                        "transition-opacity duration-300",
                        copied ? "opacity-100" : "opacity-0"
                    )}
                    aria-hidden="true"
                >
                    {copiedTitle}
                </span>
            </a>
        </As>
    );
}
