import React, { useState, type ReactNode } from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import VideoDialog from "@site/src/components/Common/VideoDialog";

export interface QuillWalkthroughProps {
    url: string;
    title: string;
    lead: ReactNode;
    poster: string;
    readInstead: { label: string; url: string };
}

const card = clsx(
    "relative isolate mb-4 grid grid-cols-1 overflow-hidden rounded-2xl text-white",
    "xl:grid-cols-[minmax(0,0.85fr)_minmax(0,1.15fr)]",
    // The text sits on the dark end of both gradients, where white keeps WCAG AA contrast.
    "bg-[linear-gradient(110deg,#a83826_0%,#b63d29_40%,#d9563f_70%,#ff8a73_100%)]",
    "dark:bg-[linear-gradient(135deg,#261210_0%,#5c2116_38%,#b8442f_72%,#ff775f_100%)]"
);

/**
 * The start page's walkthrough video as a card: the lead and a quiet link to the written path sit on a coral
 * gradient, with a dashboard screenshot tucked into the bottom-right corner as the poster. The poster is a local
 * image, so nothing loads from YouTube until someone presses play, and the video opens in a VideoDialog.
 * remark-video-embeds checks `url` the way it checks a <Video>'s.
 */
export default function QuillWalkthrough({ url, title, lead, poster, readInstead }: QuillWalkthroughProps): ReactNode {
    const [isVideoOpen, setIsVideoOpen] = useState(false);

    return (
        <div className={card} data-no-lightbox>
            <div className="flex flex-col gap-5 p-7 xl:justify-between xl:px-10 xl:py-9">
                <p className="!mb-0 max-w-[30ch] text-xl leading-snug font-medium tracking-tight text-balance">
                    {lead}
                </p>
                <Link
                    to={readInstead.url}
                    className={clsx(
                        "inline-flex w-fit items-center gap-1 rounded-sm text-sm",
                        "!text-white/85 !no-underline hover:!text-white hover:!underline",
                        "dark:!text-white/60 dark:hover:!text-white",
                        "focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-white"
                    )}
                >
                    Prefer to read? {readInstead.label}
                    <Icon icon="arrow-thin-right" size="2xs" />
                </Link>
            </div>

            <button
                type="button"
                onClick={() => setIsVideoOpen(true)}
                aria-haspopup="dialog"
                aria-label={`Play video: ${title}`}
                className={clsx(
                    "group relative ml-7 aspect-video cursor-pointer self-end overflow-hidden rounded-tl-xl bg-black",
                    "shadow-[-30px_-30px_80px_-30px_rgb(0_0_0_/_0.5)] outline outline-1 -outline-offset-1 outline-white/15",
                    "focus-visible:outline-4 focus-visible:-outline-offset-4 focus-visible:outline-white",
                    "xl:mt-11 xl:ml-0"
                )}
            >
                <img
                    src={poster}
                    alt=""
                    loading="lazy"
                    decoding="async"
                    className="size-full object-cover object-left-top"
                />
                <span className="absolute inset-0 m-auto flex size-16 items-center justify-center rounded-full bg-primary text-white shadow-[0_8px_30px_rgb(0_0_0_/_0.35)] !transition-colors group-hover:bg-primary-dark dark:text-black dark:group-hover:bg-primary-light">
                    <Icon icon="play" size="sm" />
                </span>
            </button>

            <VideoDialog url={url} title={title} isOpen={isVideoOpen} onClose={() => setIsVideoOpen(false)} />
        </div>
    );
}
