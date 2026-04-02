import React, { useState, useEffect, useRef } from "react";
import clsx from "clsx";

export interface LazyImageProps extends React.ImgHTMLAttributes<HTMLImageElement> {
    imgSrc?: string;
    minContentHeight?: number;
    isRounded?: boolean;
    aspectRatio?: string;
}

// @docusaurus/plugin-ideal-image transforms image imports into objects like
// { src: { src: "/img/file.hash.png" } } instead of plain URL strings.
// Extract the URL string from such objects.
function toUrl(value: unknown): string | undefined {
    if (typeof value === "string") {
        return value;
    }
    if (!value || typeof value !== "object") {
        return undefined;
    }
    const { src } = value as Record<string, unknown>;
    if (typeof src === "string") {
        return src;
    }
    if (src && typeof src === "object") {
        return (src as Record<string, unknown>).src as string;
    }
    return undefined;
}

export default function LazyImage({
    imgSrc,
    src,
    alt = "",
    className,
    style,
    minContentHeight = 100,
    isRounded = true,
    loading = "lazy",
    aspectRatio,
    ...props
}: LazyImageProps) {
    const isEager = loading === "eager";
    const [isLoaded, setIsLoaded] = useState(isEager);
    const imgRef = useRef<HTMLImageElement>(null);

    // Check if image is already loaded after hydration
    useEffect(() => {
        if (imgRef.current?.complete) {
            setIsLoaded(true);
        }
    }, []);

    const imageSrc = src || toUrl(imgSrc);

    if (!imageSrc) {
        return null;
    }

    return (
        <span
            className={clsx("relative overflow-hidden block w-full", className)}
            style={{
                ...style,
                aspectRatio: !isLoaded ? aspectRatio : undefined,
                minHeight: !isLoaded && !aspectRatio ? `${minContentHeight}px` : undefined,
            }}
        >
            {!isLoaded && (
                <span
                    className={clsx(
                        "absolute inset-0 skeleton z-10",
                        isRounded ? "rounded-[inherit]" : "!rounded-none"
                    )}
                    aria-hidden="true"
                />
            )}
            <img
                {...props}
                ref={imgRef}
                src={imageSrc}
                alt={alt}
                className={clsx(className, "transition-opacity duration-300", !isLoaded ? "opacity-0" : "opacity-100")}
                onLoad={() => setIsLoaded(true)}
                onError={() => setIsLoaded(true)}
                loading={loading}
            />
        </span>
    );
}
