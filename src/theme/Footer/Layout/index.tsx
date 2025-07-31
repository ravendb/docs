import React from "react";
import Link from "@docusaurus/Link";
import { useThemeConfig } from "@docusaurus/theme-common";
import type { Props } from "@theme/Footer/Layout";

interface CustomFooterLinkItem {
  label: string;
  href: string;
  context?: string;
  icon?: string;
}

interface CustomFooterSection {
  items: CustomFooterLinkItem[];
}

function TopLinks({ items }: { items: CustomFooterLinkItem[] }) {
  return (
    <div className="space-y-1">
      {items.map((item, index) => (
        <span key={index} className="text-sm flex flex-wrap items-start gap-1">
          {item.context && <span>{item.context}</span>}
          <Link to={item.href}>{item.label}</Link>
        </span>
      ))}
    </div>
  );
}

function SocialIcons({ items }: { items: CustomFooterLinkItem[] }) {
  return (
    <div className="flex gap-2">
      {items.map((item, index) => (
        <Link
          key={index}
          href={item.href}
          className="w-8 h-8 flex items-center justify-center card !rounded-lg border border-black/10 dark:border-white/10 bg-muted/40 p-4 hover:border-black/20 dark:hover:border-white/20 transition-colors hover:!no-underline"
          aria-label={item.label}
        >
          {item.icon === "github" && <span>🐱</span>}
          {item.icon === "youtube" && <span>▶️</span>}
          {item.icon === "discord" && <span>💬</span>}
        </Link>
      ))}
    </div>
  );
}

export default function FooterLayout({ copyright }: Props) {
  const { footer } = useThemeConfig();
  const links = footer.links as CustomFooterSection[];

  const topItems = links[0].items;
  const footnoteItems = links[1].items;
  const socialItems = links[2].items;

  const footnoteSection = [{ label: copyright as string }, ...footnoteItems];

  return (
    <footer className="footer">
      <div className="container">
        <TopLinks items={topItems} />
        <hr className="!mt-6 !mb-5 !bg-black/10 dark:!bg-white/10" />
        <div className="flex justify-between items-center gap-4 flex-wrap">
          <div className="flex flex-wrap gap-x-2 text-sm">
            {footnoteSection.map((item, index) => (
              <React.Fragment key={index}>
                {"href" in item ? (
                  <Link to={item.href}>{item.label}</Link>
                ) : (
                  <>{item.label}</>
                )}
                {index !== footnoteSection.length - 1 && (
                  <span className="opacity-50">·</span>
                )}
              </React.Fragment>
            ))}
          </div>
          <SocialIcons items={socialItems} />
        </div>
      </div>
    </footer>
  );
}
