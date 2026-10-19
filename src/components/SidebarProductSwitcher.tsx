import React, { useEffect, useId, useRef, useState } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import { Icon } from "@site/src/components/Common/Icon";
import { PRODUCT_BRAND_COLORS } from "@site/src/components/Common/productBrandColors";
import type { ProductNavItem } from "@site/src/typescript/pathUtils";

interface SidebarProductSwitcherProps {
    products: ProductNavItem[];
    currentProduct: ProductNavItem;
    onNavigate?: () => void;
}

export default function SidebarProductSwitcher({ products, currentProduct, onNavigate }: SidebarProductSwitcherProps) {
    const [open, setOpen] = useState(false);
    const wrapperRef = useRef<HTMLDivElement>(null);
    const buttonRef = useRef<HTMLButtonElement>(null);
    const listId = useId();

    useEffect(() => {
        if (!open) {
            return;
        }
        const handleClickOutside = (event: MouseEvent) => {
            if (!wrapperRef.current?.contains(event.target as Node)) {
                setOpen(false);
            }
        };
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, [open]);

    const handleKeyDown = (event: React.KeyboardEvent) => {
        if (event.key === "Escape" && open) {
            setOpen(false);
            buttonRef.current?.focus();
        }
    };

    return (
        <div ref={wrapperRef} onKeyDown={handleKeyDown} className="relative w-full">
            <span className="block text-xs text-ifm-menu mb-1">Product</span>
            <button
                ref={buttonRef}
                type="button"
                onClick={() => setOpen((isOpen) => !isOpen)}
                aria-expanded={open}
                aria-controls={listId}
                aria-label={`Product: ${currentProduct.label}`}
                className="w-full flex justify-between items-center rounded-md border border-black/10 dark:border-white/10 px-3 text-sm py-2 bg-transparent text-ifm-menu hover:bg-black/5 dark:hover:bg-white/5 cursor-pointer !transition-all"
            >
                <span className="flex items-center gap-2">
                    <Icon
                        icon={currentProduct.icon}
                        size="xs"
                        className={clsx("-translate-y-px", PRODUCT_BRAND_COLORS[currentProduct.pathType])}
                    />
                    {currentProduct.label}
                </span>
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    aria-hidden="true"
                >
                    <path d="m7 15 5 5 5-5"></path>
                    <path d="m7 9 5-5 5 5"></path>
                </svg>
            </button>
            <div
                id={listId}
                className={clsx(
                    "absolute inset-x-0 mt-1 z-50 p-1 text-sm rounded-md border border-black/10 dark:border-white/10 bg-ifm-background shadow-lg !transition-all duration-200 ease-out origin-top",
                    open ? "visible opacity-100 scale-100" : "invisible opacity-0 scale-95"
                )}
            >
                <ul className="!m-0 !p-0 !list-none flex flex-col gap-1">
                    {products.map((product) => {
                        const isCurrent = product.pathType === currentProduct.pathType;
                        return (
                            <li key={product.label} className="rounded-sm overflow-hidden">
                                <Link
                                    to={product.to}
                                    aria-current={isCurrent ? "true" : undefined}
                                    onClick={() => {
                                        setOpen(false);
                                        onNavigate?.();
                                    }}
                                    className="menu__link !items-start gap-2"
                                >
                                    <Icon
                                        icon={product.icon}
                                        size="xs"
                                        className={PRODUCT_BRAND_COLORS[product.pathType]}
                                    />
                                    <span className="flex flex-col gap-0.5">
                                        {product.label}
                                        <span className="text-xs text-black/55 dark:text-white/55">
                                            {product.description}
                                        </span>
                                    </span>
                                </Link>
                            </li>
                        );
                    })}
                </ul>
            </div>
        </div>
    );
}
