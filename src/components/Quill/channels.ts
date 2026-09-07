import type { FC, SVGProps } from "react";
import DiscordMark from "./marks/DiscordMark";
import EmbedMark from "./marks/EmbedMark";
import SlackMark from "./marks/SlackMark";
import TelegramMark from "./marks/TelegramMark";

export interface Channel {
    id: string;
    /** Read into the mobile row's aria-labels, never rendered as text. */
    name: string;
    /** Drives the glow behind the chip only; the mark keeps its own brand fill. */
    tint: string;
    /** The vendor's official mark as an inline SVG, `viewBox="0 0 24 24"`, sized by CSS. */
    Icon: FC<SVGProps<SVGSVGElement>>;
}

/**
 * The shipped channels, in the order they take blob slots (see blobPositions.ts): the first two land on
 * the crisp slots, so keep the channels that matter most at the top. Adding a channel is one entry here.
 */
export const CHANNELS: Channel[] = [
    { id: "telegram", name: "Telegram", tint: "#3AA8DE", Icon: TelegramMark },
    { id: "slack", name: "Slack", tint: "#A86EC4", Icon: SlackMark },
    { id: "discord", name: "Discord", tint: "#6B78F0", Icon: DiscordMark },
    { id: "embed", name: "Chat widget", tint: "#C98A7E", Icon: EmbedMark },
];
