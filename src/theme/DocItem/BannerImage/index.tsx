import React from "react";
import clsx from "clsx";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import LazyImage from "@site/src/components/Common/LazyImage";

export interface BannerImageProps {
    imgSrc?: string | { light: string; dark: string };
    imgAlt?: string;
    imgIcon?: IconName;
    className?: string;
}

export default function BannerImage({
    imgSrc: propImgSrc,
    imgAlt: propImgAlt,
    imgIcon: propImgIcon,
    className,
}: BannerImageProps = {}) {
    const { frontMatter } = useDoc();

    const imgSrc = propImgSrc ?? frontMatter.image;
    const imgAlt = propImgAlt ?? frontMatter.title ?? "";
    const imgIcon = propImgIcon ?? frontMatter.icon;

    if (!imgSrc && !imgIcon) {
        return null;
    }

    const hasImage = Boolean(imgSrc);

    return (
        <div
            className={clsx(
                "flex items-center justify-center",
                "rounded-xl overflow-hidden",
                "mb-4 relative",
                "aspect-[79/24]",
                !hasImage && "bg-gradient-to-b from-[#204879] to-[#0F1425] to-[70%]",
                className
            )}
        >
            {hasImage ? (
                <LazyImage
                    imgSrc={imgSrc}
                    alt={imgAlt}
                    className={clsx(
                        "pointer-events-none",
                        "w-full h-full object-cover object-center",
                        "!transition-transform origin-bottom"
                    )}
                />
            ) : (
                <Icon
                    icon={imgIcon ?? "default"}
                    size="2xl"
                    className="filter brightness-0 invert !transition-transform"
                />
            )}
        </div>
    );
}
