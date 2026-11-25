import React, { type ReactNode } from "react";
import clsx from "clsx";
import { useDateTimeFormat } from "@docusaurus/theme-common/internal";
import { useBlogPost } from "@docusaurus/plugin-content-blog/client";
import type { Props } from "@theme/BlogPostItem/Header/Info";

import styles from "./styles.module.css";

export default function BlogPostItemHeaderInfo({ className }: Props): ReactNode {
    const { metadata } = useBlogPost();
    const { date } = metadata;
    const dateTimeFormat = useDateTimeFormat({
        day: "numeric",
        month: "long",
        year: "numeric",
        timeZone: "UTC",
    });
    return (
        <div className={clsx(styles.info, className)}>
            <time dateTime={date}>{dateTimeFormat.format(new Date(date))}</time>
        </div>
    );
}
