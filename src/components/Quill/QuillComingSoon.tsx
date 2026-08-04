import type { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import styles from "./QuillComingSoon.module.css";

export default function QuillComingSoon(): ReactNode {
    const latestVersion = useLatestVersion("default");

    return (
        <section className="relative flex items-center justify-center overflow-hidden rounded-2xl px-6 py-16 min-h-[calc(100vh-var(--ifm-navbar-height)-4rem)]">
            <div className={`${styles.grid} absolute inset-0 opacity-70`} aria-hidden="true" />
            <div
                className={`${styles.glow} pointer-events-none absolute top-[30%] left-1/2 h-[340px] w-[min(680px,90%)] -translate-x-1/2 -translate-y-1/2 opacity-[0.12] blur-xl`}
                aria-hidden="true"
            />

            <div className="relative z-10 flex max-w-3xl flex-col items-center gap-4 text-center">
                <span className="inline-flex items-center gap-4 rounded-full border border-[var(--ifm-color-emphasis-200)] bg-[var(--ifm-color-primary-contrast-background)] px-3.5 py-1.5 text-[0.8125rem] font-semibold tracking-wide uppercase text-[var(--ifm-color-primary)]">
                    <span
                        className={`${styles.pulseDot} h-[7px] w-[7px] rounded-full bg-[var(--ifm-color-primary)]`}
                        aria-hidden="true"
                    />
                    Coming soon
                </span>
                <div className="flex flex-col items-center gap-4">
                    <Heading as="h1">Quill documentation is on the way</Heading>

                    <p
                        style={{ margin: 0 }}
                        className="max-w-[34rem] text-md leading-relaxed text-[var(--ifm-color-emphasis-700)]"
                    >
                        We're busy writing the docs for Quill. In the meantime, explore the rest of the RavenDB
                        documentation or reach out if you have questions.
                    </p>
                </div>

                <div className="flex flex-wrap justify-center gap-3">
                    <Link to={`/${latestVersion.label}`} className="button button--primary">
                        Browse RavenDB docs
                    </Link>
                </div>
            </div>
        </section>
    );
}
