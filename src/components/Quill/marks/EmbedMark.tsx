import React, { type SVGProps } from "react";

/**
 * The mark for Quill's own chat-widget channel: a speech bubble with an embed glyph, in the Quill coral.
 * There is no vendor here, so this one is drawn in-house.
 */
export default function EmbedMark(props: SVGProps<SVGSVGElement>) {
    return (
        <svg
            viewBox="0 0 24 24"
            fill="none"
            stroke="#ff775f"
            strokeWidth={2}
            strokeLinecap="round"
            strokeLinejoin="round"
            aria-hidden="true"
            {...props}
        >
            <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z" />
            <path d="m10 7.5-2.5 2.5 2.5 2.5" />
            <path d="m14 7.5 2.5 2.5-2.5 2.5" />
        </svg>
    );
}
