import React, { type CSSProperties, type ReactNode } from "react";
import clsx from "clsx";
import styles from "./IntegrationBlobField.module.css";
import { CHANNELS } from "./channels";

const ROW_GLOW = 0.35;

/**
 * The narrow-screen stand-in for the blob field: one centred row of the same channel chips at 40px, with
 * no blur and no drift. Below `md` the card is full-bleed and there are no gutters left to float anything
 * in, so this is where the channels show.
 */
export default function ChannelRow({ className }: { className?: string }): ReactNode {
    return (
        <ul
            aria-label={`Channels: ${CHANNELS.map((channel) => channel.name).join(", ")}`}
            className={clsx("!mt-4 !p-0 !list-none flex flex-wrap justify-center gap-3 pt-8", className)}
        >
            {CHANNELS.map((channel) => (
                <li key={channel.id} className="!m-0">
                    <span
                        role="img"
                        aria-label={channel.name}
                        className={styles.rowChip}
                        style={{ "--tint": channel.tint, "--glow": ROW_GLOW } as CSSProperties}
                    >
                        <span className={styles.glow} />
                        <span className={styles.glass} />
                        <channel.Icon className={styles.rowMark} />
                    </span>
                </li>
            ))}
        </ul>
    );
}
