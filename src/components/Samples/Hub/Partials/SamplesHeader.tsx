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
                    Explore RavenDB Sample Apps
                </Heading>
                <p className="!mb-0">
                    Production-ready, fully runnable applications built with RavenDB. Each one pairs a working codebase
                    with a documented architecture and a step-by-step setup guide. Clone a repository to explore
                    real-world patterns like vector search, AI integration, and ETL, or use one as a starting point for
                    your own project.
                </p>
            </div>
            <SamplesDecoration />
        </div>
    );
}
