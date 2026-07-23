import React from "react";
import clsx from "clsx";
import Button from "@site/src/components/Common/Button";
import { trackSampleOutboundClick, trackSampleRepoClick, withSamplesUtm } from "@site/src/components/Samples/analytics";

export interface ActionsCardProps {
    className?: string;
    githubUrl?: string;
    demoUrl?: string;
    sampleName?: string;
    techStack?: string;
}

export default function ActionsCard({ className, githubUrl, demoUrl, sampleName, techStack }: ActionsCardProps) {
    const githubHref = withSamplesUtm(githubUrl, sampleName);
    const demoHref = withSamplesUtm(demoUrl, sampleName);

    return (
        <div
            className={clsx(
                "flex flex-col gap-3",
                "p-3 rounded-xl",
                "bg-black/5 dark:bg-white/5",
                "border border-black/10 dark:border-white/10",
                className
            )}
        >
            <div className="flex flex-col gap-2">
                {githubHref && (
                    <Button
                        url={githubHref}
                        variant="secondary"
                        size="sm"
                        iconName="github"
                        className="w-full"
                        onClick={() =>
                            trackSampleRepoClick({
                                sample_name: sampleName || "",
                                tech_stack: techStack || "",
                                destination_url: githubHref,
                            })
                        }
                    >
                        Browse code
                    </Button>
                )}
                {demoHref && (
                    <Button
                        url={demoHref}
                        variant="outline"
                        size="sm"
                        iconName="newtab"
                        className="w-full"
                        onClick={() =>
                            trackSampleOutboundClick({
                                destination_url: demoHref,
                                link_text: "View demo",
                                sample_name: sampleName,
                            })
                        }
                    >
                        View demo
                    </Button>
                )}
            </div>
        </div>
    );
}
