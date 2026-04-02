import React, { ReactNode } from "react";
import clsx from "clsx";
import { motion } from "motion/react";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import useBoolean from "@site/src/hooks/useBoolean";

export interface FeatureAccordionProps {
    className?: string;
    title: string;
    description: string;
    icon?: IconName;
    children?: ReactNode;
    defaultExpanded?: boolean;
}

export default function FeatureAccordion({
    className,
    title,
    description,
    icon = "link",
    children,
    defaultExpanded = false,
}: FeatureAccordionProps) {
    const { value: isExpanded, toggle } = useBoolean(defaultExpanded);

    return (
        <div
            className={clsx(
                "flex flex-col",
                "p-3 rounded-xl !mb-4",
                "bg-black/5 dark:bg-white/5",
                "border border-black/10 dark:border-white/10",
                className
            )}
        >
            <button
                onClick={toggle}
                className={clsx(
                    "flex items-center gap-3 w-full",
                    "text-left cursor-pointer",
                    "bg-transparent border-none p-0"
                )}
            >
                <Icon icon={icon} size="sm" className="text-black dark:text-white shrink-0 mt-0.5" />
                <div className="flex-1 min-w-0">
                    <p className="text-base font-semibold leading-6 text-black dark:text-white !mb-0 break-words">
                        {title}
                    </p>
                    <p className="text-sm leading-5 text-black/60 dark:text-white/60 !mb-0 break-words">
                        {description}
                    </p>
                </div>
                <motion.div
                    className="flex items-center justify-center shrink-0 size-[15px] mt-1"
                    animate={{ rotate: isExpanded ? -180 : 0 }}
                    transition={{ duration: 0.2, ease: "easeInOut" }}
                >
                    <Icon icon="chevron-down" size="xs" className="text-black dark:text-white" />
                </motion.div>
            </button>
            {children && (
                <div
                    className={clsx(
                        "overflow-hidden transition-all duration-200 ease-in-out",
                        isExpanded ? "grid grid-rows-[1fr] opacity-100" : "grid grid-rows-[0fr] opacity-0"
                    )}
                >
                    <div className="min-h-0">
                        <div className="border-t border-black/10 dark:border-white/10 mt-3 pt-3 [&>*:last-child]:!mb-0">
                            {children}
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
