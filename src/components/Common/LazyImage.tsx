import React, { useState } from "react";
import clsx from "clsx";
import ThemedImage from "@theme/ThemedImage";

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
    const resolveSrc = (s: any) => {
        if (typeof s === "object" && s !== null && "default" in s) {
            return s.default;
        }
        return s;
    };
    const finalSrc = imgSrc || src;
    const isThemedImage =
        typeof finalSrc === "object" &&
        finalSrc !== null &&
        !("default" in finalSrc) &&
        ("light" in finalSrc || "dark" in finalSrc);
    const handleLoad = () => {
        setIsLoaded(true);
    };

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
            {isThemedImage ? (
                <ThemedImage
                    {...props}
                    sources={{
                        light: resolveSrc(
                            (finalSrc as { light: string; dark: string }).light,
                        ),
                        dark: resolveSrc(
                            (finalSrc as { light: string; dark: string }).dark,
                        ),
                    }}
                    alt={alt}
                    className={clsx(
                        className,
                        "transition-opacity duration-300",
                        !isLoaded ? "opacity-0" : "opacity-100",
                    )}
                    onLoad={handleLoad}
                    loading="lazy"
                />
            ) : (
                <img
                    {...props}
                    src={resolveSrc(finalSrc)}
                    alt={alt}
                    className={clsx(
                        className,
                        "transition-opacity duration-300",
                        !isLoaded ? "opacity-0" : "opacity-100",
                    )}
                    onLoad={handleLoad}
                    loading="lazy"
                />
            )}
        </span>
    );
}
