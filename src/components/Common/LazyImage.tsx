import React, { useState, useEffect, useRef } from "react";
import clsx from "clsx";
import ThemedImage, { Props as ThemedImageProps } from "@theme/ThemedImage";

export interface LazyImageProps extends React.ImgHTMLAttributes<HTMLImageElement> {
    imgSrc?: string | { light: string; dark: string };
    minContentHeight?: number;
}

// @docusaurus/plugin-ideal-image transforms image imports into objects like
// { src: { src: "/img/file.hash.png" } } instead of plain URL strings.
// Extract the URL string from such objects.
function toUrl(value: unknown): string | undefined {
    if (typeof value === "string") return value;
    if (!value || typeof value !== "object") return undefined;
    const { src } = value as Record<string, unknown>;
    if (typeof src === "string") return src;
    if (src && typeof src === "object") return (src as Record<string, unknown>).src as string;
    return undefined;
}

export default function LazyImage({
    imgSrc,
    src,
    alt = "",
    className,
    style,
    minContentHeight = 100,
    ...props
}: LazyImageProps) {
    const [isLoaded, setIsLoaded] = useState(false);
    const imgRef = useRef<HTMLImageElement>(null);

    // Check if image is already loaded after hydration
    useEffect(() => {
        if (imgRef.current?.complete) {
            setIsLoaded(true);
        }
    }, []);

    const sources = getSources({ imgSrc, src });

    return (
        <span
            className={clsx("relative overflow-hidden inline-block w-full", className)}
            style={{
                ...style,
                minHeight: !isLoaded ? `${minContentHeight + "px"}` : undefined,
            }}
        >
            {!isLoaded && <span className="absolute inset-0 skeleton rounded-[inherit] z-10" aria-hidden="true" />}
            <ThemedImage
                {...props}
                ref={imgRef}
                sources={sources}
                alt={alt}
                className={clsx(className, "transition-opacity duration-300", !isLoaded ? "opacity-0" : "opacity-100")}
                onLoad={() => setIsLoaded(true)}
                onError={() => setIsLoaded(true)}
                loading="lazy"
            />
        </span>
    );
}

function getSources({ imgSrc, src }: Pick<LazyImageProps, "imgSrc" | "src">): ThemedImageProps["sources"] {
    if (src) {
        return { light: src, dark: src };
    }

    if (imgSrc && typeof imgSrc === "object" && "light" in imgSrc && "dark" in imgSrc) {
        return imgSrc;
    }

    const resolved = toUrl(imgSrc);
    if (resolved) {
        return { light: resolved, dark: resolved };
    }

    return imgSrc;
}
