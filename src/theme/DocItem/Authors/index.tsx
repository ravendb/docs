import React from "react";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import authorsData from "@site/docs/authors.json";
import { Icon } from "@site/src/components/Common/Icon";
import LazyImage from "@site/src/components/Common/LazyImage";

function normalizeSocialLink(platform, handleOrUrl) {
    const isAbsoluteUrl = handleOrUrl.startsWith("http://") || handleOrUrl.startsWith("https://");
    if (isAbsoluteUrl) {
        return handleOrUrl;
    }
    switch (platform) {
        case "x":
            return `https://x.com/${handleOrUrl}`;
        case "github":
            return `https://github.com/${handleOrUrl}`;
        case "linkedin":
            return `https://www.linkedin.com/in/${handleOrUrl}/`;
        case "stackoverflow":
            return `https://stackoverflow.com/users/${handleOrUrl}`;
        case "email":
            return `mailto:${handleOrUrl}`;
        default:
            return handleOrUrl;
    }
}

const socialIconMap = {
    x: "x",
    github: "github",
    linkedin: "linkedin",
    email: "envelope",
};

export default function DocItemAuthors() {
    const { frontMatter } = useDoc();
    const { publishedAt, author: authorKey } = frontMatter;

    if (!authorKey && !publishedAt) {
        return null;
    }

    let author = null;

    if (authorKey) {
        const authorInfo = authorsData[authorKey];
        if (!authorInfo) {
            // eslint-disable-next-line no-console
            console.warn(`No author data found for key '${authorKey}' in authors.json`);
        } else {
            author = {
                name: authorInfo.name,
                title: authorInfo.title,
                url: authorInfo.url,
                imageURL: authorInfo.image_url,
                socials: authorInfo.socials,
            };
        }
    }

    if (!author && !publishedAt) {
        return null;
    }

    return (
        <div className="docAuthors margin-bottom--md">
            {author && (
                <div className="docAuthor">
                    {author.imageURL && (
                        <LazyImage
                            src={author.imageURL}
                            alt={author.name}
                            className="docAuthorImg"
                            minContentHeight={32}
                        />
                    )}
                    <div>
                        <div className="docAuthorName">
                            {author.url ? (
                                <a href={author.url} target="_blank" rel="noopener noreferrer">
                                    {author.name}
                                </a>
                            ) : (
                                author.name
                            )}
                        </div>
                        {author.title && <div className="docAuthorTitle">{author.title}</div>}
                        {author.socials && (
                            <div className="docAuthorSocials">
                                {Object.entries(author.socials).map(([platform, handleOrUrl]) => {
                                    const normalizedUrl = normalizeSocialLink(platform, handleOrUrl);
                                    return (
                                        <a
                                            key={platform}
                                            href={normalizedUrl}
                                            target="_blank"
                                            rel="noopener noreferrer"
                                            className="docAuthorSocialLink"
                                        >
                                            <Icon icon={socialIconMap[platform]} size="xs" />
                                        </a>
                                    );
                                })}
                            </div>
                        )}
                    </div>
                </div>
            )}
            {publishedAt && (
                <div className="docPublishedDate">
                    Published on{" "}
                    {new Date(publishedAt).toLocaleDateString("en-US", {
                        year: "numeric",
                        month: "long",
                        day: "numeric",
                    })}
                </div>
            )}
        </div>
    );
}
