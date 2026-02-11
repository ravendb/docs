import React from "react";
import Heading from "@theme/Heading";
import clsx from "clsx";

export default function GuidesHeader() {
    return (
        <div
            className={clsx(
                "flex flex-col gap-4 items-center justify-center text-center",
                "p-4 py-12 mb-8",
                "overflow-hidden relative rounded-xl",
                "border border-black/10 dark:border-white/10",
                "bg-[radial-gradient(50%_100%_at_0%_100%,var(--color-primary)_0%,var(--color-ifm-background-surface)_100%)] bg-no-repeat"
            )}
        >
            <Heading as="h1" className="!mb-0">
                Master RavenDB
                <br />
                with step-by-step guides
            </Heading>
            <p className="!mb-0">Comprehensive guides to help you master RavenDB concepts and features</p>
        </div>
    );
}
