import React, { type ReactNode } from "react";
import { listTagsByLetters, type TagLetterEntry } from "@docusaurus/theme-common";
import Tag from "../Tag";
import type { Props } from "@theme/TagsListByLetter";
import Heading from "@theme/Heading";

function TagLetterEntryItem({ letterEntry }: { letterEntry: TagLetterEntry }) {
    return (
        <article className="flex flex-col py-6 border-b border-black/5 dark:border-white/5 last:border-0">
            <Heading as="h2" id={letterEntry.letter}>
                {letterEntry.letter}
            </Heading>
            <div className="flex flex-wrap gap-2">
                {letterEntry.tags.map((tag) => (
                    <Tag key={tag.permalink} permalink={tag.permalink} count={tag.count}>
                        {tag.label}
                    </Tag>
                ))}
            </div>
        </article>
    );
}

export default function TagsListByLetter({ tags }: Props): ReactNode {
    const letterList = listTagsByLetters(tags);
    return (
        <section className="flex flex-col">
            {letterList.map((letterEntry) => (
                <TagLetterEntryItem key={letterEntry.letter} letterEntry={letterEntry} />
            ))}
        </section>
    );
}
