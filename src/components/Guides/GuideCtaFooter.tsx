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
    /** Sets `--cta-accent` for both themes; the icon tile and the hover state read it. */
    accent: string;
}

/* Cloud borrows the green of the Cloud docs theme. */
const ctas: GuideCta[] = [
    {
        title: "Join the community on Discord",
        description: "Ask questions and talk with the RavenDB team.",
        url: "https://discord.com/invite/ravendb",
        icon: "discord",
        accent: "[--cta-accent:#4752c4] dark:[--cta-accent:#8c95ff]",
    },
    {
        title: "Get a free developer license",
        description: "Fully featured, free for development and testing.",
        url: "https://ravendb.net/dev",
        icon: "key",
        accent: "[--cta-accent:#b45309] dark:[--cta-accent:#fbbf24]",
    },
    {
        title: "Start a free cloud database",
        description: "A managed instance, with nothing to install.",
        url: "https://ravendb.net/cloud",
        icon: "cloud",
        accent: "[--cta-accent:#417c5a] dark:[--cta-accent:#63ffa6]",
    },
    {
        title: "Explore more guides",
        description: "Step-by-step walkthroughs of RavenDB features.",
        url: "/guides",
        icon: "guides",
        accent: "[--cta-accent:var(--ifm-color-primary)] dark:[--cta-accent:var(--ifm-color-primary-lightest)]",
    },
];

function CtaCard({ title, description, url, icon, accent }: GuideCta): ReactNode {
    const isExternal = !isInternalUrl(url);

    return (
        <li
            className={clsx(
                "group relative flex items-center gap-3 rounded-xl p-3.5",
                "bg-ifm-background dark:bg-white/5",
                "transition-[background-color,box-shadow] duration-200",
                "hover:ring-1 hover:ring-[color:var(--cta-accent)]/40 dark:hover:bg-white/[0.08]",
                "has-[a:focus-visible]:outline-2 has-[a:focus-visible]:outline-offset-2",
                "has-[a:focus-visible]:outline-[color:var(--cta-accent)]",
                accent
            )}
        >
            <div
                aria-hidden="true"
                className={clsx(
                    "flex size-9 shrink-0 items-center justify-center rounded-lg",
                    "bg-[color:var(--cta-accent)]/12 text-[color:var(--cta-accent)]"
                )}
            >
                <Icon icon={icon} size="xs" />
            </div>
            <div className="flex min-w-0 flex-1 flex-col gap-0.5">
                <h3 className="!m-0 !text-sm !font-semibold !leading-snug">
                    {/* The ::after stretches the link over the whole card, so the card is one target that is
                        announced by its title alone. */}
                    <Link
                        {...(isExternal ? { href: url } : { to: url })}
                        className={clsx(
                            "!text-inherit !no-underline hover:!no-underline focus-visible:outline-none",
                            "after:absolute after:inset-0 after:rounded-xl after:content-['']"
                        )}
                    >
                        {title}
                        {isExternal && <span className="sr-only"> (opens in a new tab)</span>}
                    </Link>
                </h3>
                <p className="!m-0 text-xs leading-relaxed text-[color:var(--ifm-color-emphasis-700)]">{description}</p>
            </div>
            <Icon
                icon={isExternal ? "newtab" : "arrow-thin-right"}
                size="2xs"
                className={clsx(
                    "shrink-0 self-start text-[color:var(--ifm-color-emphasis-500)]",
                    "transition-colors duration-200 group-hover:text-[color:var(--cta-accent)]"
                )}
            />
        </li>
    );
}

/**
 * Closing call to action for guide articles: the community, the two free ways to run RavenDB, and the
 * rest of the guides. DocItem/Footer renders it right after every hosted guide, so articles end on their own
 * takeaways and leave the generic invitations to this block. The outer frame is the only border; the
 * cards are filled tiles, so the block doesn't read as boxes inside boxes.
 */
export default function GuideCtaFooter({ className }: { className?: string }): ReactNode {
    return (
        <section
            aria-labelledby="guide-cta-footer-heading"
            className={clsx(
                "relative isolate overflow-hidden rounded-2xl border p-5 md:p-6",
                "border-black/10 dark:border-white/10",
                "bg-ifm-background-surface",
                "bg-[radial-gradient(70%_90%_at_100%_0%,color-mix(in_oklab,var(--ifm-color-primary)_16%,transparent)_0%,transparent_70%)]",
                className
            )}
        >
            {/* The brand mark from the navbar logo, bleeding off the corner. */}
            <div
                aria-hidden="true"
                className={clsx(
                    "pointer-events-none absolute -top-8 -right-8 -z-10 rotate-12",
                    "text-[color:var(--ifm-color-primary)] opacity-[0.06] dark:opacity-[0.08]",
                    "[&_svg]:size-48"
                )}
            >
                <Icon icon="ravendb-mark" />
            </div>
            <div className="mb-4 flex flex-col items-start gap-1.5">
                <span className="text-xs font-semibold uppercase tracking-widest text-primary-darker dark:text-primary-lightest">
                    Next steps
                </span>
                <h2 id="guide-cta-footer-heading" className="!m-0 !text-lg !leading-tight tracking-tight">
                    Keep building with RavenDB
                </h2>
                <p className="!m-0 max-w-[60ch] text-sm text-[color:var(--ifm-color-emphasis-700)]">
                    Try what you just learned on your own data, and bring your questions to the people who build
                    RavenDB.
                </p>
            </div>
            <ul className="!m-0 grid list-none grid-cols-1 gap-2.5 !p-0 sm:grid-cols-2">
                {ctas.map((cta) => (
                    <CtaCard key={cta.url} {...cta} />
                ))}
            </ul>
        </section>
    );
}
