import React, { type CSSProperties, type ReactNode } from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";

export interface QuillFlowNode {
    icon: IconName;
    title: string;
    description: string;
    url?: string;
}

const flowCard = clsx(
    "flex h-full w-full flex-col gap-3 rounded-xl p-4",
    "border border-black/10 bg-black/[0.03]",
    "dark:border-white/10 dark:bg-white/[0.04]"
);

const flowCardLink = clsx(
    "group !text-inherit !no-underline !transition-all",
    "hover:border-black/25 hover:bg-black/[0.06] hover:!text-inherit hover:!no-underline",
    "dark:hover:border-white/25 dark:hover:bg-white/[0.08]"
);

/**
 * The data path through Quill, read left to right as four stages: source database, mirroring,
 * agent, channels. Each stage is its own card so the four read as a sequence rather than a feature
 * grid, and an arrow sits in the gutter between cards — pointing right on wide screens, rotated to
 * point down once the cards stack. The arrows carry the sequence on their own; the stages are
 * deliberately not numbered. Nothing on the home page enumerates the setup steps: the road from
 * sign-up to a live chat is listed in one place only, Getting Started > Overview, so the home page
 * cannot contradict it. Stages optionally link to their explanation on the Overview page.
 *
 * On load the cards fade and rise in turn (--flow-delay, 90ms apart) so the path is drawn left to
 * right. The section sits just under the hero and is in view on load, so this is a plain CSS
 * animation rather than a scroll observer — no hydration flash, and it degrades correctly under
 * prefers-reduced-motion. See --animate-quill-flow-in in src/css/custom.css.
 */
export function QuillFlow({ nodes }: { nodes: QuillFlowNode[] }): ReactNode {
    return (
        <ol className="!m-0 !p-0 !pb-4 !list-none flex flex-col gap-7 md:flex-row md:gap-8">
            {nodes.map((node, index) => {
                const body = (
                    <>
                        <span
                            className={clsx(
                                "flex h-10 w-10 shrink-0 items-center justify-center rounded-xl",
                                "bg-primary/15 text-primary-darker dark:text-primary",
                                "transition-colors group-hover:bg-primary/30"
                            )}
                        >
                            <Icon icon={node.icon} size="sm" />
                        </span>
                        <span className="flex flex-col gap-1">
                            <span className="font-semibold leading-5">{node.title}</span>
                            <span className="text-sm leading-5 text-[color:var(--ifm-color-emphasis-700)]">
                                {node.description}
                            </span>
                        </span>
                    </>
                );
                return (
                    <li
                        key={node.title}
                        className="!m-0 relative flex md:flex-1 animate-quill-flow-in"
                        style={{ "--flow-delay": `${index * 90}ms` } as CSSProperties}
                    >
                        {index > 0 && (
                            <span
                                aria-hidden="true"
                                className={clsx(
                                    "absolute left-1/2 -top-3.5 -translate-x-1/2 -translate-y-1/2 rotate-90",
                                    "md:-left-4 md:top-1/2 md:rotate-0",
                                    // emphasis-600 inverts between themes; emphasis-500 is the same
                                    // faint grey in both and disappears against the light background.
                                    "text-[color:var(--ifm-color-emphasis-300)]"
                                )}
                            >
                                <Icon icon="arrow-thin-right" size="sm" />
                            </span>
                        )}
                        {node.url ? (
                            <Link to={node.url} className={clsx(flowCard, flowCardLink)}>
                                {body}
                            </Link>
                        ) : (
                            <div className={flowCard}>{body}</div>
                        )}
                    </li>
                );
            })}
        </ol>
    );
}
