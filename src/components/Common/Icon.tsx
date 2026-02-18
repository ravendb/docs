import React from "react";
import { IconName } from "../../typescript/iconName";
import clsx from "clsx";

export type IconSize = "xs" | "sm" | "md" | "lg" | "xl";

export interface IconProps {
    icon: IconName;
    size?: IconSize;
    className?: string;
}

export function Icon({ icon, size = "sm", className }: IconProps) {
    const iconBase64 = require(`@site/static/icons/${icon}.svg`).default;
    const sizeClass = getSizeClass(size);

    return (
        <div
            dangerouslySetInnerHTML={{
                __html: getSvg(iconBase64, sizeClass),
            }}
            className={clsx(className)}
        />
    );
}

function getSvg(base64String: string, sizeClass: string): string {
    const svgContent = atob(base64String.split("base64,")[1]);
    const sanitizedSvg = svgContent.replace(/fill="[^"]*"/g, 'fill="currentColor"');
    const svgWithClasses = sanitizedSvg.replace(/<svg([^>]*)>/, `<svg$1 class="${sizeClass} ">`);
    return svgWithClasses;
}

function getSizeClass(size?: IconSize): `w-${number} h-${number}` {
    switch (size) {
        case "xs":
            return "w-4 h-4";
        case "sm":
            return "w-6 h-6";
        case "md":
            return "w-8 h-8";
        case "lg":
            return "w-10 h-10";
        case "xl":
            return "w-12 h-12";
        default:
            return "w-6 h-6";
    }
}
