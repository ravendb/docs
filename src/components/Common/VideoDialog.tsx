import React, { useEffect, useId, useRef, type ReactNode } from "react";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { youTubeEmbedUrl, youTubeVideoId } from "@site/src/lib/youtube-url/lib/youtube-url";

export interface VideoDialogProps {
    url: string;
    title: string;
    isOpen: boolean;
    onClose: () => void;
}

const closeButton = clsx(
    "flex size-9 cursor-pointer items-center justify-center rounded-full",
    "border border-white/20 bg-white/10 text-white !transition-colors hover:bg-white/20",
    "focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-white"
);

/**
 * A YouTube video in a modal dialog, opened by a trigger of the caller's own. The player mounts when the dialog
 * opens and unmounts when it closes, so nothing loads from YouTube until someone asks for the video, and closing
 * the dialog stops playback. Esc, the close button and a click outside the video all close it.
 */
export default function VideoDialog({ url, title, isOpen, onClose }: VideoDialogProps): ReactNode {
    const dialogRef = useRef<HTMLDialogElement>(null);
    const titleId = useId();

    const id = youTubeVideoId(url);
    if (!id) {
        throw new Error(`<VideoDialog>: "${url}" is not a YouTube watch or youtu.be link, or a video ID.`);
    }

    // A <dialog> only becomes modal through showModal(), so the open state is mirrored onto it imperatively.
    useEffect(() => {
        const dialog = dialogRef.current;
        if (isOpen && !dialog?.open) {
            dialog?.showModal();
        } else if (!isOpen && dialog?.open) {
            dialog.close();
        }
    }, [isOpen]);

    function close() {
        dialogRef.current?.close();
    }

    return (
        <dialog
            ref={dialogRef}
            aria-labelledby={titleId}
            onClose={onClose}
            onClick={(event) => {
                if (event.target === event.currentTarget) {
                    close();
                }
            }}
            className={clsx(
                "m-auto w-[min(70rem,calc(100vw-2rem))] max-w-none overflow-visible bg-transparent p-0 text-white",
                "open:animate-dialog-in backdrop:bg-black/80 backdrop:backdrop-blur-sm"
            )}
        >
            <div>
                <div className="mb-3 flex items-center justify-between gap-4">
                    <p id={titleId} className="!mb-0 text-[0.9375rem] font-semibold">
                        {title}
                    </p>
                    <button type="button" onClick={close} aria-label="Close video" className={closeButton}>
                        <Icon icon="close" size="xs" />
                    </button>
                </div>
                <div className="relative aspect-video overflow-hidden rounded-2xl bg-black shadow-[0_30px_120px_-20px_rgb(0_0_0_/_0.6)]">
                    {isOpen && (
                        <iframe
                            src={youTubeEmbedUrl(id)}
                            title={title}
                            allow="autoplay; clipboard-write; encrypted-media; picture-in-picture"
                            referrerPolicy="strict-origin-when-cross-origin"
                            allowFullScreen
                            className="absolute inset-0 size-full"
                        />
                    )}
                </div>
            </div>
        </dialog>
    );
}
