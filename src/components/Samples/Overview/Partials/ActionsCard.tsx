import React from "react";
import clsx from "clsx";
import Button from "@site/src/components/Common/Button";

export interface ActionsCardProps {
    className?: string;
    githubUser?: string;
    githubRepo?: string;
    demoUrl?: string;
}

export default function ActionsCard({ className, githubUser, githubRepo, demoUrl }: ActionsCardProps) {
    const githubUrl = githubUser && githubRepo ? `https://github.com/${githubUser}/${githubRepo}` : undefined;

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
                {githubUrl && (
                    <Button url={githubUrl} variant="secondary" size="sm" iconName="github" className="w-full">
                        Browse code
                    </Button>
                )}
                {demoUrl && (
                    <Button url={demoUrl} variant="outline" size="sm" iconName="newtab" className="w-full">
                        View demo
                    </Button>
                )}
            </div>
        </div>
    );
}
