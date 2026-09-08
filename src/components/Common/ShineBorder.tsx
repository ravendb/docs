import React, { type CSSProperties, type HTMLAttributes, type ReactNode } from "react";
import clsx from "clsx";

export interface ShineBorderProps extends HTMLAttributes<HTMLDivElement> {
    /** Width of the animated ring, in pixels. */
    borderWidth?: number;
    /** One lap of the highlight, in seconds. */
    duration?: number;
    /** The color, or colors, that sweep along the ring. Any CSS color works, `var(...)` included. */
    shineColor?: string | string[];
}

/**
 * An animated border highlight, after Magic UI's ShineBorder (MIT). Place it as the last child of a
 * `relative` parent whose radius it inherits: two masks composited with `exclude` cut the gradient down
 * to a ring `borderWidth` wide along the parent's edge, and the gradient's position loops so a highlight
 * travels around the frame. Stays still under reduced motion. Decorative and inert to the pointer.
 */
export default function ShineBorder({
    borderWidth = 0.5,
    duration = 30,
    shineColor = "#ff775f",
    className,
    style,
    ...props
}: ShineBorderProps): ReactNode {
    const colors = Array.isArray(shineColor) ? shineColor.join(",") : shineColor;
    return (
        <div
            aria-hidden="true"
            style={
                {
                    "--shine-border-width": `${borderWidth}px`,
                    "--shine-duration": `${duration}s`,
                    backgroundImage: `radial-gradient(transparent, transparent, ${colors}, transparent, transparent)`,
                    backgroundSize: "300% 300%",
                    mask: "linear-gradient(#fff 0 0) content-box, linear-gradient(#fff 0 0)",
                    WebkitMask: "linear-gradient(#fff 0 0) content-box, linear-gradient(#fff 0 0)",
                    WebkitMaskComposite: "xor",
                    maskComposite: "exclude",
                    padding: "var(--shine-border-width)",
                    ...style,
                } as CSSProperties
            }
            className={clsx(
                "pointer-events-none absolute inset-0 size-full rounded-[inherit] will-change-[background-position]",
                "motion-safe:animate-shine",
                className
            )}
            {...props}
        />
    );
}
