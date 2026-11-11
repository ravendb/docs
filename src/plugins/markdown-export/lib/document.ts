import { LANGUAGE_LABELS } from "./urls.js";

export interface Variant {
    language: string | null;
    url: string;
}

export interface DocumentParts {
    title: string;
    description?: string;
    url: string;
    version: string | null;
    language: string | null;
    variants: Variant[];
    agentSkill: string;
    body: string;
    seeAlso: { title: string; url: string }[];
}

function yamlString(value: string): string {
    return JSON.stringify(value);
}

export function languageLabel(language: string): string {
    return LANGUAGE_LABELS[language] ?? language;
}

/** Carries the skill line too: Markdown readers never see the HTML hint. */
export function buildDocument(parts: DocumentParts): string {
    const fm = ["---", `title: ${yamlString(parts.title)}`];
    if (parts.description) {
        fm.push(`description: ${yamlString(parts.description)}`);
    }
    fm.push(`url: ${yamlString(parts.url)}`);
    if (parts.version) {
        fm.push(`version: ${yamlString(parts.version)}`);
    }
    if (parts.language) {
        fm.push(`language: ${yamlString(parts.language)}`);
    }
    fm.push("---");

    const notes: string[] = [];
    if (parts.language) {
        const others = parts.variants.filter((v) => v.language && v.language !== parts.language);
        let line = `Client language: ${languageLabel(parts.language)}.`;
        if (others.length) {
            line += ` Also available for ${others.map((v) => `[${languageLabel(String(v.language))}](${v.url})`).join(", ")}.`;
        }
        notes.push(line);
    }
    notes.push(parts.agentSkill);

    const sections = [fm.join("\n"), `# ${parts.title}`, notes.map((n) => `> ${n}`).join("\n>\n")];
    if (parts.body.trim()) {
        sections.push(parts.body.trim());
    }
    if (parts.seeAlso.length) {
        sections.push(["## See also", "", ...parts.seeAlso.map((s) => `- [${s.title}](${s.url})`)].join("\n"));
    }
    return `${sections.join("\n\n")}\n`;
}
