import React, { useEffect, useLayoutEffect, useRef, useState, type ReactNode } from "react";
import clsx from "clsx";
import styles from "./QuillWidgetDemo.module.css";
import { QUILL_DEMO_CONVERSATIONS, type QuillDemoConversation } from "./quillDemoConversations";

// The widget's DEFAULT_THEME copy (packages/widget/src/widget-theme.ts in Raven.Quill.Web).
const HEADER_TITLE = "AI Assistant";
const HEADER_SUBTITLE = "Ask me anything";
const GREETING_TITLE = "How can I help?";
const GREETING_BODY = "Ask a question and I'll do my best to answer it.";
const INPUT_PLACEHOLDER = "Ask a question...";

const TIMING_MS = {
    /** Before the first keystroke, and again after a reset, so the empty widget is seen for a moment. */
    settle: 900,
    typeCharMin: 30,
    typeCharMax: 70,
    beforeSend: 450,
    thinking: 1100,
    streamTick: 28,
    afterReply: 3400,
    afterConversation: 4500,
    resetFade: 400,
} as const;

const STREAM_CHUNK_CHARS = { min: 2, max: 5 } as const;

interface DemoMessage {
    id: number;
    role: "user" | "assistant";
    content: string;
}

interface DemoState {
    messages: DemoMessage[];
    streamingId: number | null;
    draft: string;
    isTyping: boolean;
    isResetting: boolean;
}

const EMPTY_STATE: DemoState = { messages: [], streamingId: null, draft: "", isTyping: false, isResetting: false };

function completedState(conversation: QuillDemoConversation): DemoState {
    return {
        ...EMPTY_STATE,
        messages: conversation.map((turn, index) => ({ id: index, role: turn.role, content: turn.content })),
    };
}

function shuffle<T>(items: readonly T[]): T[] {
    const copy = [...items];
    for (let i = copy.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [copy[i], copy[j]] = [copy[j], copy[i]];
    }
    return copy;
}

function randomBetween(min: number, max: number): number {
    return min + Math.random() * (max - min);
}

const sleep = (ms: number) => new Promise<void>((resolve) => window.setTimeout(resolve, ms));

// --- Markdown subset -------------------------------------------------------------------------------------

/** What `remend` does for the real widget: a half-streamed buffer gets its dangling emphasis or code span
 *  closed for this render only, and a marker with nothing after it yet is hidden rather than shown raw. */
function closeDangling(text: string): string {
    let out = text;
    if (out.endsWith("*") && !out.endsWith("**")) {
        out = out.slice(0, -1);
    }
    if ((out.match(/\*\*/g) ?? []).length % 2 === 1) {
        out = out.endsWith("**") ? out.slice(0, -2) : `${out}**`;
    }
    if ((out.match(/`/g) ?? []).length % 2 === 1) {
        out = out.endsWith("`") ? out.slice(0, -1) : `${out}\``;
    }
    return out;
}

function renderInline(text: string): ReactNode[] {
    return text
        .split(/(\*\*[^*]+\*\*|`[^`]+`)/g)
        .filter((part) => part.length > 0)
        .map((part, index) => {
            if (part.startsWith("**") && part.endsWith("**")) {
                return <strong key={index}>{part.slice(2, -2)}</strong>;
            }
            if (part.startsWith("`") && part.endsWith("`")) {
                return <code key={index}>{part.slice(1, -1)}</code>;
            }
            return part;
        });
}

function tableCells(line: string): string[] {
    return line
        .replace(/^\|/, "")
        .replace(/\|$/, "")
        .split("|")
        .map((cell) => cell.trim());
}

function isSeparatorRow(cells: string[]): boolean {
    return cells.every((cell) => /^:?-*:?$/.test(cell));
}

function MarkdownTable({ lines }: { lines: string[] }): ReactNode {
    const [headerLine, ...bodyLines] = lines;
    const header = tableCells(headerLine);
    const rows = bodyLines.map(tableCells).filter((cells) => !isSeparatorRow(cells));
    return (
        <div className={styles.tableWrap}>
            <table>
                <thead>
                    <tr>
                        {header.map((cell, index) => (
                            <th key={index}>{renderInline(cell)}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {rows.map((cells, rowIndex) => (
                        <tr key={rowIndex}>
                            {cells.map((cell, cellIndex) => (
                                <td key={cellIndex}>{renderInline(cell)}</td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

type LineKind = "text" | "list" | "table";

function kindOf(line: string): LineKind {
    if (line.startsWith("- ")) {
        return "list";
    }
    if (line.startsWith("|")) {
        return "table";
    }
    return "text";
}

/** Groups consecutive lines of one kind, so a list or table can follow a sentence without a blank line
 *  between them, as CommonMark allows and the scripted answers do. */
function groupLines(lines: string[]): { kind: LineKind; lines: string[] }[] {
    const groups: { kind: LineKind; lines: string[] }[] = [];
    for (const line of lines) {
        const kind = kindOf(line);
        const last = groups[groups.length - 1];
        if (last && last.kind === kind) {
            last.lines.push(line);
        } else {
            groups.push({ kind, lines: [line] });
        }
    }
    return groups;
}

function renderMarkdown(markdown: string, isStreaming: boolean): ReactNode {
    const text = isStreaming ? closeDangling(markdown) : markdown;
    return text.split(/\n\n+/).flatMap((block, blockIndex) =>
        groupLines(block.split("\n").filter((line) => line.length > 0)).map((group, groupIndex) => {
            const key = `${blockIndex}-${groupIndex}`;
            if (group.kind === "list") {
                return (
                    <ul key={key}>
                        {group.lines.map((line, lineIndex) => (
                            <li key={lineIndex}>{renderInline(line.slice(2))}</li>
                        ))}
                    </ul>
                );
            }
            if (group.kind === "table") {
                return <MarkdownTable key={key} lines={group.lines} />;
            }
            return <p key={key}>{renderInline(group.lines.join(" "))}</p>;
        })
    );
}

// --- Pieces of the widget ----------------------------------------------------------------------------------

function ArrowUpIcon(): ReactNode {
    return (
        <svg
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth={2}
            strokeLinecap="round"
            strokeLinejoin="round"
            aria-hidden="true"
            className={styles.icon}
        >
            <path d="M12 19V5" />
            <path d="m5 12 7-7 7 7" />
        </svg>
    );
}

function StopIcon(): ReactNode {
    return (
        <svg viewBox="0 0 24 24" fill="currentColor" aria-hidden="true" className={styles.icon}>
            <rect x="7" y="7" width="10" height="10" rx="1.5" />
        </svg>
    );
}

function ThinkingIndicator(): ReactNode {
    return (
        <div className={styles.thinking}>
            <span aria-hidden="true" className={styles.dots}>
                {["0ms", "160ms", "320ms"].map((delay) => (
                    <span key={delay} style={{ animationDelay: delay }} className={styles.dot} />
                ))}
            </span>
            Thinking
        </div>
    );
}

function AssistantMessage({ content, isStreaming }: { content: string; isStreaming: boolean }): ReactNode {
    if (content.length === 0) {
        return isStreaming ? <ThinkingIndicator /> : null;
    }
    return <div className={styles.assistant}>{renderMarkdown(content, isStreaming)}</div>;
}

// --- The mockup ---------------------------------------------------------------------------------------------

/**
 * A live, non-interactive re-creation of the Quill chat widget for the docs start page. It types a
 * question into the composer, shows the thinking dots, streams the answer in, and after a pause moves on
 * to another scripted conversation in random order. It pauses while scrolled out of view or in a hidden
 * tab, and with reduced motion it shows one finished conversation instead of animating.
 */
export default function QuillWidgetDemo(): ReactNode {
    const [state, setState] = useState<DemoState>(EMPTY_STATE);
    const rootRef = useRef<HTMLDivElement>(null);
    const feedRef = useRef<HTMLDivElement>(null);

    // Grows with every appended chunk, which is what tells the feed to follow the new content.
    const scrollSignal = state.messages.reduce(
        (total, message) => total + message.content.length,
        state.messages.length
    );

    useLayoutEffect(() => {
        const feed = feedRef.current;
        if (feed) {
            feed.scrollTop = feed.scrollHeight;
        }
    }, [scrollSignal]);

    useEffect(() => {
        if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
            setState(completedState(QUILL_DEMO_CONVERSATIONS[0]));
            return undefined;
        }

        let isCancelled = false;
        let isInView = true;
        const observer = new IntersectionObserver(([entry]) => {
            isInView = entry.isIntersecting;
        });
        if (rootRef.current) {
            observer.observe(rootRef.current);
        }

        /** Waits, then holds while nobody can see the widget. Resolves false once the component is gone. */
        const step = async (ms: number): Promise<boolean> => {
            await sleep(ms);
            while (!isCancelled && (document.hidden || !isInView)) {
                await sleep(250);
            }
            return !isCancelled;
        };

        const playConversation = async (conversation: QuillDemoConversation): Promise<boolean> => {
            let nextId = 0;
            for (let i = 0; i + 1 < conversation.length; i += 2) {
                const question = conversation[i].content;
                const answer = conversation[i + 1].content;

                setState((current) => ({ ...current, isTyping: true }));
                for (let length = 1; length <= question.length; length++) {
                    setState((current) => ({ ...current, draft: question.slice(0, length) }));
                    if (!(await step(randomBetween(TIMING_MS.typeCharMin, TIMING_MS.typeCharMax)))) {
                        return false;
                    }
                }
                if (!(await step(TIMING_MS.beforeSend))) {
                    return false;
                }

                const userId = nextId++;
                const assistantId = nextId++;
                setState((current) => ({
                    ...current,
                    draft: "",
                    isTyping: false,
                    streamingId: assistantId,
                    messages: [
                        ...current.messages,
                        { id: userId, role: "user", content: question },
                        { id: assistantId, role: "assistant", content: "" },
                    ],
                }));
                if (!(await step(TIMING_MS.thinking))) {
                    return false;
                }

                let shown = 0;
                while (shown < answer.length) {
                    shown = Math.min(
                        answer.length,
                        shown + Math.round(randomBetween(STREAM_CHUNK_CHARS.min, STREAM_CHUNK_CHARS.max))
                    );
                    const content = answer.slice(0, shown);
                    setState((current) => ({
                        ...current,
                        messages: current.messages.map((message) =>
                            message.id === assistantId ? { ...message, content } : message
                        ),
                    }));
                    if (!(await step(TIMING_MS.streamTick))) {
                        return false;
                    }
                }
                setState((current) => ({ ...current, streamingId: null }));
                if (!(await step(TIMING_MS.afterReply))) {
                    return false;
                }
            }
            return true;
        };

        const play = async () => {
            if (!(await step(TIMING_MS.settle))) {
                return;
            }
            let queue: QuillDemoConversation[] = [];
            let previous: QuillDemoConversation | null = null;
            while (!isCancelled) {
                if (queue.length === 0) {
                    queue = shuffle(QUILL_DEMO_CONVERSATIONS);
                    // A fresh shuffle may start with the conversation that just ended; push it to the back.
                    if (queue[0] === previous && queue.length > 1) {
                        queue.push(queue.shift()!);
                    }
                }
                const conversation = queue.shift()!;
                if (!(await playConversation(conversation))) {
                    return;
                }
                previous = conversation;

                if (!(await step(TIMING_MS.afterConversation))) {
                    return;
                }
                setState((current) => ({ ...current, isResetting: true }));
                if (!(await step(TIMING_MS.resetFade))) {
                    return;
                }
                setState(EMPTY_STATE);
                if (!(await step(TIMING_MS.settle))) {
                    return;
                }
            }
        };

        void play();
        return () => {
            isCancelled = true;
            observer.disconnect();
        };
    }, []);

    const hasTranscript = state.messages.length > 0;
    const isStreaming = state.streamingId !== null;
    const isSendEnabled = isStreaming || state.draft.length > 0;

    return (
        <div
            ref={rootRef}
            role="img"
            aria-label="Animated preview of the Quill chat widget answering a user's questions from mirrored data"
            className={styles.root}
        >
            <div className={styles.header}>
                <span className={styles.headerText}>
                    <span className={styles.headerTitle}>{HEADER_TITLE}</span>
                    <span className={styles.headerSubtitle}>{HEADER_SUBTITLE}</span>
                </span>
            </div>

            <div className={styles.feed}>
                <div ref={feedRef} className={clsx(styles.feedScroll, state.isResetting && styles.feedHidden)}>
                    {hasTranscript ? (
                        <div className={styles.transcript}>
                            {state.messages.map((message) =>
                                message.role === "user" ? (
                                    <div key={message.id} className={styles.userRow}>
                                        <div className={styles.userBubble}>{message.content}</div>
                                    </div>
                                ) : (
                                    <AssistantMessage
                                        key={message.id}
                                        content={message.content}
                                        isStreaming={message.id === state.streamingId}
                                    />
                                )
                            )}
                        </div>
                    ) : (
                        <div className={styles.greeting}>
                            <div>
                                <p className={styles.greetingTitle}>{GREETING_TITLE}</p>
                                <p className={styles.greetingBody}>{GREETING_BODY}</p>
                            </div>
                        </div>
                    )}
                </div>
            </div>

            <div className={styles.composer}>
                <div className={clsx(styles.composerBox, state.isTyping && styles.composerBoxActive)}>
                    <div className={styles.input}>
                        {state.draft.length > 0 ? (
                            state.draft
                        ) : (
                            <span className={styles.placeholder}>{INPUT_PLACEHOLDER}</span>
                        )}
                        {state.isTyping && <span className={styles.caret} />}
                    </div>
                    <div className={clsx(styles.send, !isSendEnabled && styles.sendDisabled)}>
                        {isStreaming ? <StopIcon /> : <ArrowUpIcon />}
                    </div>
                </div>
            </div>
        </div>
    );
}
