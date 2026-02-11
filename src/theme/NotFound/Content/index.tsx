import React, { type ReactNode } from "react";
import clsx from "clsx";
import Translate from "@docusaurus/Translate";
import type { Props } from "@theme/NotFound/Content";
import Heading from "@theme/Heading";
import CardWithIcon from "@site/src/components/Common/CardWithIcon";
import { useActiveDocContext, useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import { useLocation } from "@docusaurus/router";

export default function NotFoundContent({ className }: Props): ReactNode {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const versionLabel = activeVersion?.label ?? latestVersion.label;
    const { pathname } = useLocation();
    const isCloudPath = pathname.includes("/cloud");
    return (
        <main className={clsx("container margin-vert--xl", className)}>
            <div className="row">
                <div className="col">
                    <svg xmlns="http://www.w3.org/2000/svg" width="240" height="98" viewBox="0 0 240 98" fill="none">
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M119.539 0.795471C127.327 0.795471 134.024 2.67461 139.63 6.43219C145.266 10.1898 149.6 15.644 152.63 22.7955C155.66 29.9167 157.16 38.5531 157.13 48.7047C157.13 58.9166 155.615 67.6439 152.585 74.8863C149.585 82.1286 145.282 87.6589 139.676 91.4771C134.07 95.2953 127.357 97.2047 119.539 97.2047C111.721 97.1743 104.994 95.2502 99.3574 91.4322C93.7514 87.614 89.4326 82.0837 86.4023 74.8414C83.4024 67.599 81.9179 58.8864 81.9482 48.7047C81.9482 38.5531 83.4482 29.9011 86.4482 22.7496C89.4785 15.5984 93.7966 10.1594 99.4023 6.43219C105.039 2.67462 111.751 0.795476 119.539 0.795471ZM119.539 16.9322C114.236 16.9322 109.993 19.5833 106.812 24.8863C103.63 30.1893 102.023 38.1289 101.993 48.7047C101.993 55.8561 102.706 61.826 104.13 66.6138C105.584 71.3713 107.63 74.9475 110.267 77.3414C112.903 79.7048 115.994 80.8863 119.539 80.8863C124.872 80.8863 129.13 78.2047 132.312 72.8414C135.493 67.4777 137.069 59.4319 137.039 48.7047C137.039 41.644 136.312 35.7649 134.857 31.0679C133.433 26.3711 131.403 22.8407 128.767 20.4771C126.161 18.1135 123.085 16.9322 119.539 16.9322Z"
                            fill="url(#paint0_linear_592_3318)"
                        />
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M63.5 63.023H75.0459V78.7955H63.5V95.1588H44.6816V78.7955H0V63.2955L38.8633 2.06793H63.5V63.023ZM19.8184 62.2955V63.023H45.0459V23.523H44.3184L19.8184 62.2955Z"
                            fill="url(#paint0_linear_592_3318)"
                        />
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M228.135 2.06793V63.023H239.681V78.7955H228.135V95.1588H209.316V78.7955H164.635V63.2955L203.499 2.06793H228.135ZM208.953 23.523L184.453 62.2955V63.023H209.681V23.523H208.953Z"
                            fill="url(#paint0_linear_592_3318)"
                        />
                        <defs>
                            <linearGradient
                                id="paint0_linear_592_3318"
                                x1="517.932"
                                y1="-32.4123"
                                x2="517.932"
                                y2="94.7306"
                                gradientUnits="userSpaceOnUse"
                            >
                                <stop stop-color="var(--ifm-color-primary)" />
                                <stop offset="1" stop-color="var(--ifm-color-primary-dark)" stop-opacity="0" />
                            </linearGradient>
                        </defs>
                    </svg>
                    <Heading as="h1" className="hero__title !mt-4">
                        <Translate id="theme.NotFound.title" description="The title of the 404 page">
                            Page Not Found
                        </Translate>
                    </Heading>
                    <p>
                        The page you’re looking for doesn’t exist or may have been moved.
                        <br />
                        Double-check the URL, or head back to the homepage to find what you need.
                    </p>
                    <div className="grid grid-cols-2 gap-3">
                        <CardWithIcon
                            title="Homepage"
                            description="Feeling lost? Start at the very beginning"
                            url={isCloudPath ? "/cloud" : `/${versionLabel}`}
                            icon="home"
                        />
                        <CardWithIcon
                            title="Search the docs"
                            description="There's probably an article for your issue already"
                            url="/search"
                            icon="search"
                        />
                    </div>
                </div>
            </div>
        </main>
    );
}
