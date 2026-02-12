import React from "react";
import LazyImage from "./LazyImage";

export interface CardImageProps {
    imgSrc: string | { light: string; dark: string };
    imgAlt?: string;
    className?: string;
}

export default function CardImage({
    imgSrc,
    imgAlt = "",
    className = "",
}: CardImageProps) {
    return <LazyImage imgSrc={imgSrc} alt={imgAlt} className={className} />;
}
