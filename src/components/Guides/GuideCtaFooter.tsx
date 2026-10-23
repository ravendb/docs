import React, { type ReactNode } from "react";
import Link from "@docusaurus/Link";
import isInternalUrl from "@docusaurus/isInternalUrl";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import type { IconName } from "@site/src/typescript/iconName";

interface GuideCta {
    title: string;
    description: string;
    url: string;
    icon: IconName;
    /** Sets `--cta-accent` for both themes; the icon tile reads it. */
    accent: string;
}

/*
 * The accents share one OKLCH lightness and chroma per theme and differ only in hue, so the tiles vary without
 * any of them outweighing the rest. Cloud keeps the green of the Cloud docs theme, Discord its blurple.
 */
const ctas: GuideCta[] = [
    {
        title: "Join the community on Discord",
        description: "Ask questions and talk with the RavenDB team.",
        url: "https://discord.com/invite/ravendb",
        icon: "discord",
        accent: "[--cta-accent:oklch(0.56_0.13_280)] dark:[--cta-accent:oklch(0.8_0.12_280)]",
    },
    {
        title: "Get a free developer license",
        description: "Fully featured, free for development and testing.",
        url: "https://ravendb.net/dev",
        icon: "key",
        accent: "[--cta-accent:oklch(0.56_0.13_70)] dark:[--cta-accent:oklch(0.8_0.12_70)]",
    },
    {
        title: "Start a free cloud database",
        description: "A managed instance, with nothing to install.",
        url: "https://ravendb.net/cloud",
        icon: "cloud",
        accent: "[--cta-accent:oklch(0.56_0.13_155)] dark:[--cta-accent:oklch(0.8_0.12_155)]",
    },
    {
        title: "Explore more guides",
        description: "Step-by-step walkthroughs of RavenDB features.",
        url: "/guides",
        icon: "guides",
        accent: "[--cta-accent:oklch(0.56_0.13_245)] dark:[--cta-accent:oklch(0.8_0.12_245)]",
    },
];

function CtaCell({ title, description, url, icon, accent }: GuideCta): ReactNode {
    const isExternal = !isInternalUrl(url);

    return (
        <li
            className={clsx(
                "group relative flex items-start gap-4 p-5",
                "bg-clip-padding transition-[background-color] duration-200 hover:bg-ifm-background-surface",
                "border-black/10 dark:border-white/10",
                "@max-xl:not-last:border-b @xl:odd:border-r @xl:nth-[-n+2]:border-b",
                "has-[a:focus-visible]:outline-2 has-[a:focus-visible]:-outline-offset-2",
                "has-[a:focus-visible]:outline-primary",
                accent
            )}
        >
            <div
                aria-hidden="true"
                className={clsx(
                    "flex size-10 shrink-0 items-center justify-center rounded-lg [&_svg]:size-5",
                    "bg-[color:var(--cta-accent)]/12 text-[color:var(--cta-accent)]",
                    "transition-[background-color] duration-200 group-hover:bg-[color:var(--cta-accent)]/20"
                )}
            >
                <Icon icon={icon} />
            </div>
            <div className="flex min-w-0 flex-col">
                <h3 className="!m-0 !text-sm !font-semibold !leading-5">
                    {/* The ::after stretches the link over the whole cell, so the cell is one target that is
                        announced by its title alone. */}
                    <Link
                        {...(isExternal ? { href: url } : { to: url })}
                        className={clsx(
                            "!text-inherit !no-underline hover:!no-underline focus-visible:outline-none",
                            "after:absolute after:inset-0 after:content-['']"
                        )}
                    >
                        {title}
                        {isExternal && <span className="sr-only"> (opens in a new tab)</span>}
                    </Link>
                </h3>
                <p className="!m-0 text-[13px]/5 text-[color:var(--ifm-color-emphasis-700)]">{description}</p>
            </div>
        </li>
    );
}

/**
 * Closing call to action for guide articles: the two free ways to run RavenDB, the community, and the rest of
 * the guides. DocItem/Footer renders it right after every hosted guide, so articles end on their own takeaways and
 * leave the generic invitations to this block. The heading sits on the page like the See also heading below it,
 * and the links share one bordered grid split by hairlines rather than cards inside a box.
 */
export default function GuideCtaFooter({ className }: { className?: string }): ReactNode {
    return (
        <section
            aria-labelledby="guide-cta-footer-heading"
            className={clsx("@container flex w-full flex-col items-start gap-4", className)}
        >
            <div className="flex flex-col items-start gap-1">
                <span className="text-xs font-semibold uppercase tracking-wider text-[color:var(--ifm-color-emphasis-600)]">
                    Next steps
                </span>
                <h2 id="guide-cta-footer-heading" className="!m-0 !text-xl !leading-tight">
                    Keep building with RavenDB
                </h2>
                <p className="!m-0 max-w-[60ch] text-sm text-[color:var(--ifm-color-emphasis-700)]">
                    Try what you just learned on your own data, and bring your questions to the people who build
                    RavenDB.
                </p>
            </div>
            <ul
                className={clsx(
                    "!m-0 grid w-full list-none overflow-hidden rounded-xl !p-0 @xl:grid-cols-2",
                    "border border-black/10 dark:border-white/10"
                )}
            >
                {ctas.map((cta) => (
                    <CtaCell key={cta.url} {...cta} />
                ))}
            </ul>
        </section>
    );
}
