try {
    require("graceful-fs").gracefulify(require("fs"));
} catch (err) {
    console.warn(`[patch-fs] graceful-fs unavailable; EMFILE mitigation skipped: ${err.message}`);
}
