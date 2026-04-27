import React from "react";
import clsx from "clsx";

interface ToggleOption<T extends string> {
    value: T;
    label: string;
}

interface ToggleProps<T extends string> {
    options: ToggleOption<T>[];
    value: T;
    onChange: (value: T) => void;
    className?: string;
}

export default function Toggle<T extends string>({ options, value, onChange, className }: ToggleProps<T>) {
    return (
        <div className={clsx("flex gap-1 p-1 rounded-full", "border border-black/10 dark:border-white/10", className)}>
            {options.map((option) => (
                <button
                    key={option.value}
                    onClick={() => onChange(option.value)}
                    className={clsx(
                        "flex-1 py-2 leading-none rounded-full text-xs font-semibold transition-colors cursor-pointer",
                        value === option.value
                            ? "bg-black/10 dark:bg-white/10 text-black dark:text-white"
                            : "text-black/60 dark:text-white/60 hover:text-black dark:hover:text-white"
                    )}
                >
                    {option.label}
                </button>
            ))}
        </div>
    );
}
