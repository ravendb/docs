import React, { useState } from "react";
import clsx from "clsx";
import ThemedImage, { Props as ThemedImageProps } from "@theme/ThemedImage";

export interface LazyImageProps
    extends React.ImgHTMLAttributes<HTMLImageElement> {
    imgSrc?: string | { light: string; dark: string };
}

export default function LazyImage({
    imgSrc,
    src,
    alt = "",
    className,
    style,
    ...props
}: LazyImageProps) {
    const [isLoaded, setIsLoaded] = useState(false);

    const handleLoaded = () => {
        setIsLoaded(true);
    };

    const sources = getSources({ imgSrc, src });

    return (
        <span
            className={clsx(
                "relative overflow-hidden inline-block w-full",
                className,
            )}
            style={{
                ...style,
                minHeight: !isLoaded ? "100px" : undefined,
            }}
        >
            {!isLoaded && (
                <span
                    className="absolute inset-0 skeleton rounded-[inherit] z-10"
                    aria-hidden="true"
                />
            )}
            <ThemedImage
                {...props}
                sources={sources}
                alt={alt}
                className={clsx(
                    className,
                    "transition-opacity duration-300",
                    !isLoaded ? "opacity-0" : "opacity-100",
                )}
                onLoad={handleLoaded}
                onError={handleLoaded}
                loading="lazy"
            />
        </span>
    );
}

function getSources({
    imgSrc,
    src,
}: Pick<LazyImageProps, "imgSrc" | "src">): ThemedImageProps["sources"] {
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
