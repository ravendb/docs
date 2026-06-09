import React, { useCallback, useEffect, useLayoutEffect, useRef, useState } from "react";
import clsx from "clsx";
import Tag from "@site/src/theme/Tag";

const useIsomorphicLayoutEffect = typeof window !== "undefined" ? useLayoutEffect : useEffect;

export interface OverflowTagItem {
    label: string;
    key?: string;
    permalink?: string;
    category?: string;
}

interface OverflowTagRowProps {
    tags: OverflowTagItem[];
    onTagClick?: (e: React.MouseEvent, tag: OverflowTagItem) => void;
    isTagSelected?: (tag: OverflowTagItem) => boolean;
    className?: string;
}

const GAP_PX = 4;

const tagKey = (tag: OverflowTagItem) => tag.key ?? tag.permalink ?? tag.label;

export default function OverflowTagRow({ tags, onTagClick, isTagSelected, className }: OverflowTagRowProps) {
    const measureRef = useRef<HTMLDivElement>(null);
    const [visibleCount, setVisibleCount] = useState(tags.length);
    const [expanded, setExpanded] = useState(false);

    const recompute = useCallback(() => {
        const el = measureRef.current;
        if (!el) {
            return;
        }

        const containerWidth = el.clientWidth;
        if (containerWidth === 0) {
            return;
        }

        const nodes = Array.from(el.children) as HTMLElement[];
        const pill = nodes[nodes.length - 1];
        const tagNodes = nodes.slice(0, tags.length);
        const widths = tagNodes.map((node) => node.offsetWidth);

        const totalWithGaps = widths.reduce((sum, w, i) => sum + w + (i > 0 ? GAP_PX : 0), 0);

        if (totalWithGaps <= containerWidth) {
            setVisibleCount(tags.length);
            return;
        }

        const available = containerWidth - pill.offsetWidth - GAP_PX;
        let used = 0;
        let count = 0;
        for (let i = 0; i < widths.length; i++) {
            const next = widths[i] + (count > 0 ? GAP_PX : 0);
            if (used + next <= available) {
                used += next;
                count += 1;
            } else {
                break;
            }
        }

        setVisibleCount(Math.max(count, 1));
    }, [tags.length]);

    useIsomorphicLayoutEffect(() => {
        const el = measureRef.current;
        if (!el) {
            return undefined;
        }

        let cancelled = false;
        const run = () => {
            if (!cancelled) {
                recompute();
            }
        };

        // The mirror's box width tracks the container, but its contents reflow
        // when fonts finish loading after hydration — and that reflow doesn't
        // resize the mirror, so recompute now, next frame, and once fonts settle.
        const observer = new ResizeObserver(run);
        observer.observe(el);
        run();
        const raf = requestAnimationFrame(run);
        if (typeof document !== "undefined" && document.fonts?.ready) {
            document.fonts.ready.then(run).catch(() => {});
        }

        return () => {
            cancelled = true;
            cancelAnimationFrame(raf);
            observer.disconnect();
        };
    }, [recompute, tags]);

    useEffect(() => {
        setExpanded(false);
    }, [tags]);

    const renderTag = (tag: OverflowTagItem) => (
        <Tag
            key={tagKey(tag)}
            size="xs"
            permalink={tag.permalink}
            onClick={onTagClick ? (e) => onTagClick(e, tag) : undefined}
            className={clsx(
                "shrink-0 whitespace-nowrap pointer-events-auto",
                onTagClick && "cursor-pointer",
                isTagSelected && !isTagSelected(tag) && "opacity-50"
            )}
        >
            {tag.label}
        </Tag>
    );

    const hiddenCount = tags.length - visibleCount;
    const visibleTags = expanded ? tags : tags.slice(0, visibleCount);
    const hiddenTags = tags.slice(visibleCount);

    return (
        <div className={clsx("relative", className)}>
            {/* Off-flow mirror used only for measurement: every tag + a worst-case pill. */}
            <div
                ref={measureRef}
                aria-hidden="true"
                className="invisible pointer-events-none absolute inset-x-0 top-0 flex flex-nowrap gap-1 overflow-hidden"
            >
                {tags.map((tag) => (
                    <Tag key={tagKey(tag)} size="xs" className="shrink-0 whitespace-nowrap">
                        {tag.label}
                    </Tag>
                ))}
                <Tag size="xs" className="shrink-0 whitespace-nowrap">
                    +{tags.length} more
                </Tag>
            </div>

            <div className={clsx("flex gap-1", expanded ? "flex-wrap" : "flex-nowrap overflow-hidden")}>
                {visibleTags.map(renderTag)}

                {!expanded && hiddenCount > 0 && (
                    <button
                        type="button"
                        title={hiddenTags.map((t) => t.label).join(", ")}
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            setExpanded(true);
                        }}
                        className={clsx(
                            "relative z-10 inline-flex items-center shrink-0 select-none cursor-pointer rounded-full border pointer-events-auto",
                            "h-5 px-2 text-[11px] font-medium leading-4 whitespace-nowrap",
                            "bg-black/5 dark:bg-white/5 border-black/10 dark:border-white/10",
                            "text-black/60 dark:text-white/60",
                            "hover:bg-black/10 dark:hover:bg-white/10 hover:!no-underline"
                        )}
                    >
                        +{hiddenCount} more
                    </button>
                )}
            </div>
        </div>
    );
}
