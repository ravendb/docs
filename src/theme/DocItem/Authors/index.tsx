import React from "react";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import authorsData from "@site/docs/authors.json";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";

type Platform = "x" | "github" | "linkedin" | "email";

type Author = {
    name: string;
    title: string | null;
    url: string | null;
    imageURL: string;
    socials: Record<string, string>;
};

function getAuthorData(authorKey: string): Author | null {
    const authorInfo = authorsData[authorKey];
    if (!authorInfo) {
        // eslint-disable-next-line no-console
        console.warn(`No author data found for key '${authorKey}' in authors.json`);
        return null;
    }
    return {
        name: authorInfo.name,
        title: authorInfo.title,
        url: authorInfo.url,
        imageURL: authorInfo.image_url,
        socials: authorInfo.socials,
    };
}

function isPlatform(value: string): value is Platform {
    return ["x", "github", "linkedin", "email"].includes(value);
}

function normalizeSocialLink(platform: Platform, handleOrUrl: string): string {
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
        case "email":
            return `mailto:${handleOrUrl}`;
        default:
            return handleOrUrl;
    }
}

const socialIconMap: Record<Platform, IconName> = {
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

    const author = authorKey ? getAuthorData(authorKey) : null;

    if (!author && !publishedAt) {
        return null;
    }

    return (
        <div className="docAuthors margin-bottom--md">
            {author && (
                <div className="docAuthor">
                    {author.imageURL && (
                        <img src={author.imageURL} alt={author.name} className="docAuthorImg" loading="eager" />
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
                                    if (!isPlatform(platform)) {
                                        return null;
                                    }
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
