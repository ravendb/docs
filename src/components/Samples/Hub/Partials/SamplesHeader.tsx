import React from "react";
import Heading from "@theme/Heading";
import clsx from "clsx";
import SamplesDecoration from "./SamplesDecoration";

export default function SamplesHeader() {
    return (
        <div
            className={clsx(
                "flex flex-col gap-4",
                "p-12 mb-8",
                "overflow-hidden relative rounded-xl",
                "border border-black/10 dark:border-white/10",
                "bg-[radial-gradient(130.79%_106.31%_at_63.92%_100%,var(--color-primary-ifm-color-primary-lightest,#86BAF2)_0%,var(--color-primary-ifm-color-primary-darkest,#2382E7)_100%)] bg-no-repeat"
            )}
        >
            <div className="z-1">
                <Heading as="h1" className="!mb-0">
                    Explore Samples
                </Heading>
                <p className="!mb-0">Production-ready code samples, architecture patterns, and starter kits.</p>
            </div>
            <SamplesDecoration />
        </div>
    );
}
