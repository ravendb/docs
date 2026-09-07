import React, { useCallback, useEffect, useRef, useState, type CSSProperties, type ReactNode } from "react";
import clsx from "clsx";
import styles from "./IntegrationBlobField.module.css";
import ChannelRow from "./ChannelRow";
import { BLOB_MARGIN_PX, LAYERS, MIN_VISIBLE_SLIVER_PX, SLOTS, SPREAD, type Layout } from "./blobPositions";
import { CHANNELS } from "./channels";

interface IntegrationBlobFieldProps {
    /** The hero content; it sits above the field. */
    children: ReactNode;
    /**
     * `split` (default): copy left, card right. Because a split hero stacks on narrower screens, this
     * renders the split slots from `xl` up and the centered slots below it. `centered`: the card alone,
     * centred, at every width.
     */
    layout?: Layout;
    className?: string;
}

const isDevelopment = process.env.NODE_ENV !== "production";

/** Per-blob motion, derived from the index so no two blobs share a cycle. Negative delays start each one
 *  mid-cycle, so nothing syncs up on load. */
function driftStyle(index: number, tint: string): CSSProperties {
    const magnitude = 8 + (index % 3) * 5;
    return {
        "--dur": `${(7.5 + (index % 5) * 1.3).toFixed(1)}s`,
        "--del": `${(-index * 0.9).toFixed(1)}s`,
        "--dx": `${index % 2 ? magnitude : -magnitude}px`,
        "--dy": `${index % 3 ? -Math.round(magnitude * 0.7) : Math.round(magnitude * 0.8)}px`,
        "--tint": tint,
    } as CSSProperties;
}

let hasWarnedAboutSlotCount = false;

function BlobLayer({ layout, className }: { layout: Layout; className?: string }): ReactNode {
    const slots = SLOTS[layout];
    if (isDevelopment && CHANNELS.length > slots.length && !hasWarnedAboutSlotCount) {
        hasWarnedAboutSlotCount = true;
        console.warn(
            `[IntegrationBlobField] ${CHANNELS.length} channels but only ${slots.length} slots; positions repeat from the first slot.`
        );
    }

    return (
        <div className={clsx(styles.box, className)}>
            {CHANNELS.map((channel, index) => {
                const [dx, dy, layer] = slots[index % slots.length];
                const { scale, blur, opacity, glow } = LAYERS[layer];
                return (
                    <div
                        key={channel.id}
                        data-blob-dx={dx}
                        data-blob-dy={dy}
                        className={clsx(
                            styles.drift,
                            "motion-safe:animate-blob-drift",
                            layer === "far" && "hidden lg:block"
                        )}
                        style={driftStyle(index, channel.tint)}
                    >
                        <div
                            data-blob-layer={layer}
                            data-blob-channel={channel.id}
                            className={styles.chip}
                            style={
                                {
                                    transform: `scale(${scale})`,
                                    filter: blur > 0 ? `blur(${blur}px)` : undefined,
                                    opacity,
                                    "--glow": glow,
                                } as CSSProperties
                            }
                        >
                            <span className={styles.glow} />
                            <span className={styles.glass} />
                            <channel.Icon className={styles.mark} />
                        </div>
                    </div>
                );
            })}
        </div>
    );
}

/**
 * Anchors every blob to the card rather than to the hero box: each slot's `[dx, dy]` is read as a multiple
 * of the card's half-width and half-height, scaled by `SPREAD`, then clamped so no blob centre comes within
 * `BLOB_MARGIN_PX` of a field edge. Runs after layout and on every change of the hero's or the card's size,
 * so the field re-hugs the panel as the hero reflows. Returns whether a first measurement has landed, so
 * the field can stay hidden until the blobs are somewhere real.
 */
function useCardAnchoredPlacement(rootRef: React.RefObject<HTMLElement | null>): boolean {
    const [placed, setPlaced] = useState(false);

    const place = useCallback(() => {
        const root = rootRef.current;
        const card = root?.querySelector("[data-blob-keep-clear]");
        if (!root || !card) {
            return;
        }
        const field = root.getBoundingClientRect();
        const box = card.getBoundingClientRect();
        if (field.width === 0 || box.width === 0) {
            return;
        }
        const centerX = box.left - field.left + box.width / 2;
        const centerY = box.top - field.top + box.height / 2;

        root.querySelectorAll<HTMLElement>("[data-blob-dx]").forEach((blob) => {
            const dx = Number.parseFloat(blob.dataset.blobDx ?? "0");
            const dy = Number.parseFloat(blob.dataset.blobDy ?? "0");
            const x = centerX + dx * (box.width / 2) * SPREAD;
            const y = centerY + dy * (box.height / 2) * SPREAD;
            blob.style.left = `${clamp(x, BLOB_MARGIN_PX, field.width - BLOB_MARGIN_PX)}px`;
            blob.style.top = `${clamp(y, BLOB_MARGIN_PX, field.height - BLOB_MARGIN_PX)}px`;
        });
        setPlaced(true);
    }, [rootRef]);

    useEffect(() => {
        const root = rootRef.current;
        const card = root?.querySelector("[data-blob-keep-clear]");
        if (!root || !card) {
            return undefined;
        }

        // Coalesced with a timer, not requestAnimationFrame: a hero that mounts in a background tab gets
        // no animation frames, and blobs that never place would stay invisible until the tab was focused.
        let timer: ReturnType<typeof setTimeout> | undefined;
        const schedule = () => {
            clearTimeout(timer);
            timer = setTimeout(place, 0);
        };
        place();

        // ResizeObserver catches the reflows a viewport listener misses (the card growing with its own
        // content, a sidebar opening); the viewport listener covers the reverse, since observer callbacks
        // are delivered with a frame and a page that is not rendering gets none.
        const observer = new ResizeObserver(schedule);
        observer.observe(root);
        observer.observe(card);
        window.addEventListener("resize", schedule);
        // Fonts land after first paint and change the card's height, so remeasure once they do.
        document.fonts?.ready.then(schedule).catch(() => undefined);

        return () => {
            clearTimeout(timer);
            observer.disconnect();
            window.removeEventListener("resize", schedule);
        };
    }, [place, rootRef]);

    return placed;
}

function clamp(value: number, low: number, high: number): number {
    if (high < low) {
        return (low + high) / 2;
    }
    return value < low ? low : value > high ? high : value;
}

/** Development-only check of the visibility rule: every near and mid blob must keep at least
 *  MIN_VISIBLE_SLIVER_PX of itself outside the card marked `data-blob-keep-clear`, at every point of its
 *  drift. Half-tucking behind the card's edge is the depth cue and is fine; vanishing behind it entirely
 *  is a slot that has drifted out of the composition. Re-runs on resize. */
function useBlobVisibilityAssertion(rootRef: React.RefObject<HTMLElement | null>) {
    useEffect(() => {
        if (!isDevelopment) {
            return undefined;
        }
        const root = rootRef.current;
        if (!root) {
            return undefined;
        }

        const check = () => {
            const card = root.querySelector("[data-blob-keep-clear]");
            if (!card) {
                return;
            }
            const c = card.getBoundingClientRect();
            root.querySelectorAll<HTMLElement>('[data-blob-layer="near"], [data-blob-layer="mid"]').forEach((chip) => {
                const wrapper = chip.parentElement;
                if (!wrapper || chip.offsetParent === null) {
                    return;
                }
                const wrapperStyle = getComputedStyle(wrapper);
                const dx = Math.abs(Number.parseFloat(wrapperStyle.getPropertyValue("--dx"))) || 0;
                const dy = Math.abs(Number.parseFloat(wrapperStyle.getPropertyValue("--dy"))) || 0;
                const b = chip.getBoundingClientRect();
                // The worst case over the drift cycle: the chip pushed as far behind the card as it goes.
                const sliver = Math.max(
                    c.left - (b.left - dx),
                    b.right + dx - c.right,
                    c.top - (b.top - dy),
                    b.bottom + dy - c.bottom
                );
                if (sliver < MIN_VISIBLE_SLIVER_PX) {
                    console.warn(
                        `[IntegrationBlobField] ${chip.dataset.blobChannel} (${chip.dataset.blobLayer}) shows only ${Math.round(Math.max(0, sliver))}px past the card at its deepest drift (want ${MIN_VISIBLE_SLIVER_PX}px) at ${window.innerWidth}px wide.`
                    );
                }
            });
        };

        let frame = 0;
        const onResize = () => {
            cancelAnimationFrame(frame);
            frame = requestAnimationFrame(check);
        };
        check();
        window.addEventListener("resize", onResize);
        return () => {
            cancelAnimationFrame(frame);
            window.removeEventListener("resize", onResize);
        };
    }, [rootRef]);
}

/**
 * A decorative field of circular channel blobs behind the hero content, spread over three depth layers:
 * two crisp in front, the rest progressively blurred and faded back, so the hero reads as "many channels"
 * without asking the viewer to identify each one. Every blob is placed relative to the card marked
 * `data-blob-keep-clear` (see blobPositions.ts), so the field hugs the panel at any hero width. Background
 * art only: nothing here is interactive or exposed to assistive technology. The field paints no background
 * of its own. Below `md` it is replaced by a centred row of the same chips under the content.
 */
export default function IntegrationBlobField({
    children,
    layout = "split",
    className,
}: IntegrationBlobFieldProps): ReactNode {
    const rootRef = useRef<HTMLElement>(null);
    const placed = useCardAnchoredPlacement(rootRef);
    useBlobVisibilityAssertion(rootRef);

    return (
        <section ref={rootRef} className={clsx("relative isolate overflow-hidden", className)}>
            <div
                aria-hidden="true"
                className={clsx(
                    "absolute inset-0 z-0 hidden transition-opacity duration-500 md:block",
                    placed ? "opacity-100" : "opacity-0"
                )}
            >
                {layout === "split" ? (
                    <>
                        <BlobLayer layout="split" className="hidden xl:block" />
                        <BlobLayer layout="centered" className="xl:hidden" />
                    </>
                ) : (
                    <BlobLayer layout="centered" />
                )}
            </div>
            <div className="relative z-10">{children}</div>
            <ChannelRow className="md:hidden" />
        </section>
    );
}
