import React, { ReactNode, useEffect, useRef, useState } from "react";
import { Icon } from "@site/src/components/Common/Icon";
import { youTubeEmbedUrl, youTubePosterUrl, youTubeVideoId } from "@site/src/lib/youtube-url/lib/youtube-url";

export interface VideoProps {
    url: string;
    title: string;
    caption?: ReactNode;
}

export default function Video({ url, title, caption }: VideoProps) {
    const [isPlaying, setIsPlaying] = useState(false);
    const iframeRef = useRef<HTMLIFrameElement>(null);

    useEffect(() => {
        if (isPlaying) {
            iframeRef.current?.focus();
        }
    }, [isPlaying]);

    const id = youTubeVideoId(url);
    if (!id) {
        throw new Error(`<Video>: "${url}" is not a YouTube watch or youtu.be link, or a video ID.`);
    }

    return (
        <figure className="mb-[var(--ifm-leading)]" data-no-lightbox>
            <div className="relative aspect-video overflow-hidden rounded-2xl border border-black/10 dark:border-white/10">
                {isPlaying ? (
                    <iframe
                        ref={iframeRef}
                        src={youTubeEmbedUrl(id)}
                        title={title}
                        allow="autoplay; clipboard-write; encrypted-media; picture-in-picture"
                        referrerPolicy="strict-origin-when-cross-origin"
                        allowFullScreen
                        className="absolute inset-0 size-full"
                    />
                ) : (
                    <button
                        type="button"
                        onClick={() => setIsPlaying(true)}
                        aria-label={`Play video: ${title}`}
                        className="group absolute inset-0 cursor-pointer rounded-2xl focus-visible:outline-4 focus-visible:-outline-offset-4 focus-visible:outline-primary"
                    >
                        <img
                            src={youTubePosterUrl(id)}
                            alt={title}
                            loading="lazy"
                            className="size-full object-cover !transition-transform group-hover:scale-105"
                        />
                        <span className="absolute inset-0 m-auto flex size-16 items-center justify-center rounded-full bg-primary text-white dark:text-black">
                            <Icon icon="play" size="sm" />
                        </span>
                    </button>
                )}
            </div>
            {caption && <figcaption className="mt-2 text-sm text-black/60 dark:text-white/60">{caption}</figcaption>}
        </figure>
    );
}
