import React, { type ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";

export interface QuillFlowNode {
    icon: IconName;
    title: string;
    description: string;
    url?: string;
}

export interface QuillStep {
    title: string;
    description: ReactNode;
    url: string;
    linkLabel?: string;
}

const nodeBody = "flex items-start gap-3 md:flex-col md:gap-3";

/**
 * The data path through Quill, read left to right: source database, mirroring, agent, channels.
 * Stacks vertically on small screens. Nodes optionally link to their explanation on the Overview page;
 * linked nodes get a tinted hover surface and a slightly brighter tile so they read as clickable.
 */
export function QuillFlow({ nodes }: { nodes: QuillFlowNode[] }): ReactNode {
    return (
        <ol
            className={clsx(
                "!m-0 !p-4 md:!p-5 !list-none",
                "grid grid-cols-1 gap-y-4 md:grid-cols-4 md:gap-x-6",
                "rounded-2xl border border-black/10 bg-black/5 dark:border-white/10 dark:bg-white/5"
            )}
        >
            {nodes.map((node) => {
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
                        <span className="flex flex-col gap-0.5">
                            <span className="font-semibold leading-5">{node.title}</span>
                            <span className="text-sm leading-5 text-[color:var(--ifm-color-emphasis-700)]">
                                {node.description}
                            </span>
                        </span>
                    </>
                );
                return (
                    <li key={node.title} className="!m-0">
                        {node.url ? (
                            <Link
                                to={node.url}
                                className={clsx(
                                    nodeBody,
                                    "group -m-2 rounded-xl p-2 !transition-all",
                                    "!text-inherit !no-underline hover:!text-inherit hover:!no-underline",
                                    "hover:bg-black/5 dark:hover:bg-white/5"
                                )}
                            >
                                {body}
                            </Link>
                        ) : (
                            <div className={nodeBody}>{body}</div>
                        )}
                    </li>
                );
            })}
        </ol>
    );
}

/**
 * Numbered setup steps, each a card linking to its Getting Started page.
 */
export function QuillSteps({ steps }: { steps: QuillStep[] }): ReactNode {
    return (
        <ol className="!m-0 !p-0 !list-none grid grid-cols-1 gap-4 md:grid-cols-3">
            {steps.map((step, index) => (
                <li key={step.title} className="!m-0 flex">
                    <Link
                        to={step.url}
                        className={clsx(
                            "group flex h-full w-full flex-col gap-3 rounded-2xl p-5",
                            "border border-black/10 bg-black/5 !text-inherit",
                            "hover:border-black/20 hover:bg-black/10 hover:!text-inherit",
                            "dark:border-white/10 dark:bg-white/5 dark:hover:border-white/20 dark:hover:bg-white/10",
                            "!no-underline hover:!no-underline !transition-all"
                        )}
                    >
                        <span
                            className={clsx(
                                "flex h-8 w-8 items-center justify-center rounded-full",
                                "bg-primary text-sm font-bold text-white dark:text-black"
                            )}
                        >
                            {index + 1}
                        </span>
                        <Heading as="h3" className="!mb-0 !text-base !font-bold !leading-5">
                            {step.title}
                        </Heading>
                        <p className="!mb-0 text-sm leading-5 text-[color:var(--ifm-color-emphasis-700)]">
                            {step.description}
                        </p>
                        <span className="mt-auto inline-flex items-center gap-1 pt-1 text-sm font-medium text-primary-darker dark:text-primary">
                            {step.linkLabel ?? "Open the guide"}
                            <Icon
                                icon="arrow-thin-right"
                                size="xs"
                                className="!transition-transform group-hover:translate-x-0.5"
                            />
                        </span>
                    </Link>
                </li>
            ))}
        </ol>
    );
}
