import React, { useEffect } from "react";
import { motion, AnimatePresence, PanInfo } from "framer-motion";
import clsx from "clsx";
import { Icon } from "./Icon";

interface DrawerProps {
    open: boolean;
    onClose: () => void;
    children: React.ReactNode;
    title?: string;
    headerAction?: React.ReactNode;
}

export default function Drawer({ open, onClose, children, title, headerAction }: DrawerProps) {
    useEffect(() => {
        if (open) {
            document.body.style.overflow = "hidden";
        } else {
            document.body.style.overflow = "";
        }

        return () => {
            document.body.style.overflow = "";
        };
    }, [open]);

    useEffect(() => {
        const handleEscape = (e: KeyboardEvent) => {
            if (e.key === "Escape" && open) {
                onClose();
            }
        };

        document.addEventListener("keydown", handleEscape);
        return () => document.removeEventListener("keydown", handleEscape);
    }, [open, onClose]);

    // eslint-disable-next-line no-undef
    const handleDragEnd = (_: MouseEvent | TouchEvent | PointerEvent, info: PanInfo) => {
        const shouldClose = info.velocity.y > 500 || info.offset.y > 150;
        if (shouldClose) {
            onClose();
        }
    };

    return (
        <AnimatePresence mode="wait">
            {open && (
                <>
                    <motion.div
                        key="backdrop"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                        transition={{ duration: 0.2 }}
                        onClick={onClose}
                        className="fixed inset-0 bg-black/50 backdrop-blur-sm z-[9998]"
                    />
                    <motion.div
                        key="drawer"
                        initial={{ y: "100%" }}
                        animate={{ y: 0 }}
                        exit={{ y: "100%" }}
                        transition={{ type: "spring", damping: 30, stiffness: 300 }}
                        drag="y"
                        dragConstraints={{ top: 0 }}
                        dragElastic={{ top: 0, bottom: 0.5 }}
                        onDragEnd={handleDragEnd}
                        className={clsx(
                            "fixed bottom-0 left-0 right-0 z-[9999]",
                            "max-h-[85vh] rounded-t-2xl",
                            "bg-ifm-background",
                            "border-t border-x border-black/10 dark:border-white/10",
                            "flex flex-col",
                            "shadow-[0_-10px_30px_rgba(0,0,0,0.1)] dark:shadow-[0_-10px_30px_rgba(0,0,0,0.3)]"
                        )}
                    >
                        <div className="flex justify-center py-3 shrink-0 cursor-grab active:cursor-grabbing">
                            <div className="w-12 h-1 rounded-full bg-black/20 dark:bg-white/20" />
                        </div>

                        {title && (
                            <div className="flex items-center justify-between px-6 pb-4 shrink-0">
                                <h2 className="text-xl font-semibold text-black dark:text-white !mb-0">{title}</h2>
                                <div className="flex items-center gap-2">
                                    {headerAction}
                                    <button
                                        onClick={onClose}
                                        className="p-2 -mr-2 rounded-lg hover:bg-black/5 dark:hover:bg-white/5 transition-colors"
                                        aria-label="Close drawer"
                                    >
                                        <Icon icon="close" size="xs" />
                                    </button>
                                </div>
                            </div>
                        )}

                        <div
                            className={clsx(
                                "overflow-y-auto flex-1 px-6 pb-6",
                                "scrollbar-thin scrollbar-thumb-black/10 dark:scrollbar-thumb-white/10 scrollbar-track-transparent"
                            )}
                        >
                            {children}
                        </div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    );
}
