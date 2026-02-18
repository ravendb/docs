import React, { useState, useEffect, useRef } from "react";
import clsx from "clsx";
import ThemedImage, { Props as ThemedImageProps } from "@theme/ThemedImage";

export interface LazyImageProps extends React.ImgHTMLAttributes<HTMLImageElement> {
    imgSrc?: string | { light: string; dark: string };
}

export default function LazyImage({ imgSrc, src, alt = "", className, style, ...props }: LazyImageProps) {
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
                minHeight: !isLoaded ? "100px" : undefined,
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
        return {
            light: src,
            dark: src,
        };
    }

    if (typeof imgSrc === "string") {
        return {
            light: imgSrc,
            dark: imgSrc,
        };
    }

    return imgSrc;
}
