import * as React from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { IconName } from "@site/src/typescript/iconName";
import { Icon } from "./Icon";
import isInternalUrl from "@docusaurus/isInternalUrl";

export type ButtonVariant = "default" | "outline" | "ghost" | "destructive" | "secondary";

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    children: React.ReactNode;
    url?: string;
    className?: string;
    variant?: ButtonVariant;
    size?: "xs" | "sm";
    iconName?: IconName;
}

const variantClasses: Record<ButtonVariant, string> = {
    default: "bg-primary !text-white dark:!text-black hover:bg-primary-darker",
    secondary:
        "bg-black/10 dark:bg-white/10 !text-black dark:!text-white hover:bg-black/20 dark:hover:bg-white/20 border border-black/10 dark:border-white/10",
    outline:
        "border !text-black border-black/25 !text-foreground hover:bg-black/5 dark:!text-white dark:border-white/25 dark:hover:bg-white/5",
    ghost: "hover:bg-muted !text-foreground",
    destructive: "bg-red-500 !text-white hover:bg-red-600",
};

const sizeClasses: Record<NonNullable<ButtonProps["size"]>, string> = {
    xs: "py-2 px-3 text-xs",
    sm: "py-2 px-3 text-sm",
};

export default function Button({
    children,
    url,
    className = "",
    variant = "secondary",
    size = "sm",
    iconName,
    ...props
}: ButtonProps) {
    const baseClasses = clsx(
        "cursor-pointer inline-flex items-center justify-center rounded-md font-medium leading-none",
        "!no-underline !transition-all",
        "disabled:opacity-50 disabled:pointer-events-none",
        variantClasses[variant],
        sizeClasses[size],
        className
    );

    if (url) {
        const isExternal = !isInternalUrl(url);
        return (
            <Link {...(isExternal ? { href: url } : { to: url })} className={baseClasses}>
                {children} {iconName && <Icon icon={iconName} className="ms-1" size="xs" />}
            </Link>
        );
    }

    return (
        <button className={baseClasses} {...props}>
            {children} {iconName && <Icon icon={iconName} className="ms-1" size="xs" />}
        </button>
    );
}
