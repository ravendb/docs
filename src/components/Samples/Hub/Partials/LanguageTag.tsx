import React from "react";
import Tag from "@site/src/theme/Tag";
import { getLanguageConfig } from "../../../languageConfig";

export interface LanguageTagProps {
    languageKey: string;
    className?: string;
    onClick?: (e: React.MouseEvent) => void;
}

export default function LanguageTag({ languageKey, className, onClick }: LanguageTagProps) {
    const config = getLanguageConfig(languageKey);

    if (!config) {
        return (
            <Tag size="xs" className={className} onClick={onClick}>
                {languageKey}
            </Tag>
        );
    }

    return (
        <Tag
            size="xs"
            className={className}
            onClick={onClick}
            style={{
                backgroundColor: `light-dark(#e6e6e6, #313133)`,
                color: config.brand,
                borderColor: config.brand,
            }}
        >
            {config.label}
        </Tag>
    );
}
