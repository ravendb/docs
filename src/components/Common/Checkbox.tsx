import React from "react";
import clsx from "clsx";
import { Icon } from "./Icon";

interface CheckboxProps {
    checked: boolean;
    onChange: () => void;
    label?: string;
    className?: string;
}

export default function Checkbox({ checked, onChange, label, className }: CheckboxProps) {
    return (
        <label className={clsx("flex items-center gap-2 cursor-pointer group", className)}>
            <div className="relative flex items-center justify-center">
                <input type="checkbox" checked={checked} onChange={onChange} className="sr-only" />
                <div
                    className={clsx(
                        "w-4 h-4 rounded flex items-center justify-center",
                        "border border-black/10 dark:border-white/10",
                        checked
                            ? "bg-black/10 dark:bg-white/10"
                            : "bg-[var(--ifm-background-color)] hover:bg-black/5 dark:hover:bg-white/5"
                    )}
                >
                    {checked && <Icon icon="check" size="2xs" className="text-black dark:text-white" />}
                </div>
            </div>
            {label && (
                <span className="text-sm text-black dark:text-white group-hover:opacity-80 transition-opacity flex-1">
                    {label}
                </span>
            )}
        </label>
    );
}
