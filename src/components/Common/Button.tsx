import * as React from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children: React.ReactNode;
  url?: string;
  className?: string;
  variant?: "default" | "outline" | "ghost" | "destructive";
  size?: "sm" | "md" | "lg";
}

const variantClasses: Record<NonNullable<ButtonProps["variant"]>, string> = {
  default: "bg-primary !text-white dark:!text-black hover:bg-primary-darker",
  outline:
    "border !text-black border-black/25 !text-foreground hover:bg-black/5 dark:!text-white dark:border-white/25 dark:hover:bg-white/5",
  ghost: "hover:bg-muted !text-foreground",
  destructive: "bg-red-500 !text-white hover:bg-red-600",
};

const sizeClasses: Record<NonNullable<ButtonProps["size"]>, string> = {
  sm: "h-8 px-3 text-xs",
  md: "h-10 px-4 text-sm",
  lg: "h-12 px-6 text-base",
};

export default function Button({
  children,
  url,
  className = "",
  variant = "default",
  size = "md",
  ...props
}: ButtonProps) {
  const baseClasses = clsx(
    "inline-flex items-center justify-center rounded-md font-medium",
    "!no-underline !transition-colors !transition-duration-300",
    "disabled:opacity-50 disabled:pointer-events-none",
    variantClasses[variant],
    sizeClasses[size],
    className,
  );

  if (url) {
    return (
      <Link to={url} className={baseClasses}>
        {children}
      </Link>
    );
  }

  return (
    <button className={baseClasses} {...props}>
      {children}
    </button>
  );
}
