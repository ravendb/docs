import type { DocsLanguage } from "./LanguageStore";

export interface LanguageConfig {
    label: string;
    value: DocsLanguage;
    brand: string;
}

export const languageConfig: LanguageConfig[] = [
    { label: "C#", value: "csharp", brand: "#9179E4" },
    { label: "Java", value: "java", brand: "#f89820" },
    { label: "Python", value: "python", brand: "#fbcb24" },
    { label: "PHP", value: "php", brand: "#8993be" },
    { label: "Node.js", value: "nodejs", brand: "#3c873a" },
];

export function getLanguageConfig(languageKey: string): LanguageConfig | undefined {
    return languageConfig.find((lang) => lang.value === languageKey);
}

export function getLanguageBrandColor(languageKey: string): string | undefined {
    return getLanguageConfig(languageKey)?.brand;
}
