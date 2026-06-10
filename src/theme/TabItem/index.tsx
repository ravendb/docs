import React, { type ReactNode } from "react";
import type { Props } from "@theme/TabItem";

/**
 * Swizzled to be context-free: no useTabs() call, so bare <TabItem> (used as
 * a "code-panel" idiom throughout this docs site) renders without needing a
 * <Tabs> ancestor.  Visibility is driven by the `hidden` prop that the parent
 * <Tabs> component injects via cloneElement.
 */
export default function TabItem({ children, className, hidden }: Props & { hidden?: boolean }): ReactNode {
    return (
        <div role="tabpanel" hidden={hidden} className={className}>
            {children}
        </div>
    );
}
