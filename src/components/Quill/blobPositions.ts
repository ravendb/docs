export type Layout = "centered" | "split";
export type Layer = "near" | "mid" | "far";
export type Slot = [dx: number, dy: number, layer: Layer];

/** The unscaled blob box; `LAYERS[layer].scale` shrinks it. */
export const BLOB_SIZE_PX = 64;

/**
 * How much of a near or mid blob must stay clear of the card at every point of its drift. The field
 * deliberately hugs the panel — the front blobs half-tuck behind its edge, and that overlap is the depth
 * cue — so the rule is not a stand-off but a floor: a chip may hide behind the card, never disappear
 * behind it.
 */
export const MIN_VISIBLE_SLIVER_PX = 12;

/** Half the blob box plus a little breathing room: the closest a blob's centre may sit to a field edge. */
export const BLOB_MARGIN_PX = 40;

/**
 * How far the field pushes away from the card. 1 puts a slot's nominal offset exactly one card
 * half-dimension out; below 1 pulls the whole field in toward the panel.
 */
export const SPREAD = 0.85;

/**
 * Slot positions as offsets from the *card*, `[dx, dy, layer]`, in multiples of the card's half-width and
 * half-height (so `[-1.42, -0.06]` is 1.42 half-widths left of the card's centre, level with it). They are
 * scaled by `SPREAD` and clamped to the field box at `BLOB_MARGIN_PX`.
 *
 * Card-relative, not field-relative, is the point: the hero is fluid and the card's size and position move
 * with it, so percentages of the hero could not hold the same composition at every width — the blobs would
 * drift into the card at narrow widths and fly out to the corners at wide ones. Anchored to the card, the
 * field hugs the panel and the arrangement survives any frame.
 *
 * Order matters: crisp layers come first, so with four channels shipped none of them is blurred into
 * illegibility. Beyond the first two, slots alternate sides to keep the field balanced at any count.
 */
export const SLOTS: Record<Layout, Slot[]> = {
    split: [
        [1.32, -0.05, "near"],
        [-1.32, 0.34, "near"],
        [-1.24, -0.62, "near"],
        [1.24, 0.75, "near"],
        [-1.28, 0.55, "far"],
        [1.36, -0.22, "far"],
        [-1.02, -1.05, "far"],
        [1.24, 0.88, "mid"],
        [-1.4, -0.1, "far"],
        [0.1, -1.22, "far"],
        [-0.15, 1.24, "far"],
        [1.44, -1.0, "far"],
    ],
    centered: [
        [1.32, -0.05, "near"],
        [-1.32, 0.34, "near"],
        [-1.24, -0.62, "near"],
        [1.24, 0.75, "near"],
        [1.3, -0.92, "far"],
        [-1.32, 0.8, "far"],
        [-1.7, 0.34, "mid"],
        [1.68, -0.46, "mid"],
        [-1.28, -0.98, "far"],
        [1.28, 0.98, "far"],
        [-1.9, -0.58, "far"],
        [1.92, 0.52, "far"],
    ],
};

export const LAYERS = {
    near: { scale: 1, blur: 0, opacity: 1, glow: 0.35 },
    mid: { scale: 0.84, blur: 0.75, opacity: 0.7, glow: 0.25 },
    far: { scale: 0.66, blur: 1.5, opacity: 0.44, glow: 0.15 },
} as const;
