import * as React from "react";
import clsx from "clsx";
import { Icon } from "./Icon";
import { IconName } from "@site/src/typescript/iconName";

// eslint-disable-next-line no-undef
export interface BadgeProps extends React.HTMLAttributes<HTMLSpanElement> {
    children: React.ReactNode;
    className?: string;
    variant?: "default" | "secondary" | "outline" | "success" | "warning" | "destructive";
    size?: "sm" | "md";
    iconName?: IconName;
    iconPosition?: "left" | "right";
}

const variantClasses: Record<NonNullable<BadgeProps["variant"]>, string> = {
    default: "bg-primary-light/80 backdrop-blur-xs text-white border border-primary/10 dark:text-black",
    secondary:
        "bg-gray-200/70 backdrop-blur-xs text-black dark:bg-secondary/70 dark:text-black border border-gray-300/10 dark:border-secondary/10",
    outline: "bg-stone-100/30 backdrop-blur-xs text-black border border-black/70",
    success: "bg-green-300/80 backdrop-blur-xs text-green-950 border border-green-400/10",
    warning: "bg-orange-300/80 backdrop-blur-xs text-orange-950 border border-orange-400/10",
    destructive: "bg-red-300/80 backdrop-blur-xs text-red-950 border border-red-400/10",
};

const sizeClasses: Record<NonNullable<BadgeProps["size"]>, string> = {
    sm: "text-[11px] leading-4 h-5 px-2",
    md: "text-xs leading-5 h-6 px-2.5",
};

export default function Badge({
    children,
    className = "",
    variant = "secondary",
    size = "sm",
    iconName,
    iconPosition = "left",
    ...props
}: BadgeProps) {
    const baseClasses = clsx(
        "inline-flex items-center gap-1 select-none",
        "font-medium",
        "rounded-full",
        variantClasses[variant],
        sizeClasses[size],
        className
    );

    const iconElement = iconName ? <Icon icon={iconName} size="xs" className="shrink-0" /> : null;

    return (
        <span className={baseClasses} {...props}>
            {iconElement && iconPosition === "left" && iconElement}
            <span>{children}</span>
            {iconElement && iconPosition === "right" && iconElement}
        </span>
    );
}
