import React, { cloneElement, type ReactElement, type ReactNode } from "react";
import clsx from "clsx";
import {
    useScrollPositionBlocker,
    useTabsContextValue,
    sanitizeTabsChildren,
    type TabValue,
    type TabItemProps,
} from "@docusaurus/theme-common/internal";
import useIsBrowser from "@docusaurus/useIsBrowser";
import type { Props } from "@theme/Tabs";
import styles from "./styles.module.css";

// TabItem elements enhanced with the `hidden` prop that Tabs injects
type TabItemElement = ReactElement<TabItemProps & { hidden?: boolean }>;

interface TabListProps {
    className?: string;
    block: boolean;
    selectedValue: string;
    selectValue: (value: string) => void;
    tabValues: readonly TabValue[];
}

interface TabContentProps {
    lazy: boolean;
    selectedValue: string;
    children: ReactNode;
}

function TabList({ className, block, selectedValue, selectValue, tabValues }: TabListProps) {
    const tabRefs: (HTMLLIElement | null)[] = [];
    const { blockElementScrollPositionUntilNextRender } = useScrollPositionBlocker();

    const handleTabChange = (
        event: React.FocusEvent<HTMLLIElement> | React.MouseEvent<HTMLLIElement> | React.KeyboardEvent<HTMLLIElement>
    ) => {
        const newTab = event.currentTarget;
        const newTabIndex = tabRefs.indexOf(newTab);
        const newTabValue = tabValues[newTabIndex]!.value;

        if (newTabValue !== selectedValue) {
            blockElementScrollPositionUntilNextRender(newTab);
            selectValue(newTabValue);
        }
    };

    const handleKeydown = (event: React.KeyboardEvent<HTMLLIElement>) => {
        let focusElement: HTMLLIElement | null = null;

        switch (event.key) {
            case "Enter": {
                handleTabChange(event);
                break;
            }
            case "ArrowRight": {
                const nextTab = tabRefs.indexOf(event.currentTarget) + 1;
                focusElement = tabRefs[nextTab] ?? tabRefs[0]!;
                break;
            }
            case "ArrowLeft": {
                const prevTab = tabRefs.indexOf(event.currentTarget) - 1;
                focusElement = tabRefs[prevTab] ?? tabRefs[tabRefs.length - 1]!;
                break;
            }
            default:
                break;
        }

        focusElement?.focus();
    };

    return (
        <ul
            role="tablist"
            aria-orientation="horizontal"
            className={clsx(
                "!my-0 !px-4 flex gap-2 bg-black/6 dark:bg-white/3 border-b border-black/10 dark:border-white/10",
                {
                    "tabs--block": block,
                },
                className
            )}
        >
            {tabValues.map(({ value, label, attributes }) => (
                <li
                    // TODO extract TabListItem
                    role="tab"
                    tabIndex={selectedValue === value ? 0 : -1}
                    aria-selected={selectedValue === value}
                    key={value}
                    ref={(tabControl) => {
                        tabRefs.push(tabControl);
                    }}
                    onKeyDown={handleKeydown}
                    onClick={handleTabChange}
                    {...attributes}
                    className={clsx(
                        "tabs__item",
                        "flex items-center !text-sm !rounded-none !border-b cursor-pointer !pb-2.5",
                        "border-b-transparent text-gray-500 hover:text-gray-600 hover:!bg-transparent",
                        "dark:text-gray-300 dark:hover:text-gray-200",
                        "!transition-all !ease-in-out !duration-300",
                        styles.tabItem,
                        attributes?.className as string,
                        {
                            "tabs__item--active !text-primary": selectedValue === value,
                        }
                    )}
                >
                    {label ?? value}
                </li>
            ))}
        </ul>
    );
}

function TabContent({ lazy, children, selectedValue }: TabContentProps) {
    const childTabs = (Array.isArray(children) ? children : [children]).filter(Boolean) as TabItemElement[];
    if (lazy) {
        const selectedTabItem = childTabs.find((tabItem) => tabItem.props.value === selectedValue);
        if (!selectedTabItem) {
            // fail-safe or fail-fast? not sure what's best here
            return null;
        }
        return cloneElement(selectedTabItem, {
            className: clsx(selectedTabItem.props.className),
        });
    }
    return (
        <div className="p-4 bg-pre-background">
            {childTabs.map((tabItem, i) =>
                cloneElement(tabItem, {
                    key: i,
                    hidden: tabItem.props.value !== selectedValue,
                })
            )}
        </div>
    );
}

function TabsComponent(props: Props): ReactNode {
    const tabs = useTabsContextValue(props);
    return (
        <div
            className={clsx(
                "tabs-container overflow-hidden rounded-lg border-black/10 dark:border-white/10 border mb-6",
                styles.tabList
            )}
        >
            <TabList
                className={props.className}
                block={tabs.block}
                selectedValue={tabs.selectedValue}
                selectValue={tabs.selectValue}
                tabValues={tabs.tabValues}
            />
            <TabContent lazy={tabs.lazy} selectedValue={tabs.selectedValue}>
                {props.children}
            </TabContent>
        </div>
    );
}

export default function Tabs(props: Props): ReactNode {
    const isBrowser = useIsBrowser();
    return (
        <TabsComponent
            // Remount tabs after hydration
            // Temporary fix for https://github.com/facebook/docusaurus/issues/5653
            key={String(isBrowser)}
            {...props}
        >
            {sanitizeTabsChildren(props.children)}
        </TabsComponent>
    );
}
