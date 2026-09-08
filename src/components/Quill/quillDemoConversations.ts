/**
 * The scripted exchanges the start page's widget mockup plays back, in random order. Each is a short
 * conversation an agent could hold from mirrored SQL data. Turns alternate user and assistant, and an
 * assistant turn uses only the markdown subset the mockup renders: paragraphs separated by a blank line,
 * `- ` list items, `| |` tables with a `---` separator row, **bold**, and `code`.
 */
export interface QuillDemoTurn {
    role: "user" | "assistant";
    content: string;
}

export type QuillDemoConversation = QuillDemoTurn[];

export const QUILL_DEMO_CONVERSATIONS: QuillDemoConversation[] = [
    [
        { role: "user", content: "How much does Chai cost?" },
        {
            role: "assistant",
            content:
                "Chai is **$18.00** per unit, sold as 10 boxes × 20 bags. It's supplied by Exotic Liquids, and there are **39 units** in stock right now.",
        },
        { role: "user", content: "Any cheaper beverages?" },
        {
            role: "assistant",
            content: `Three beverages come in under $18:

| Product | Price | In stock |
| --- | --- | --- |
| Guaraná Fantástica | $4.50 | 20 |
| Rhönbräu Klosterbier | $7.75 | 125 |
| Outback Lager | $15.00 | 15 |

Guaraná Fantástica is the best seller of the three.`,
        },
    ],
    [
        { role: "user", content: "Has order 10248 shipped yet?" },
        {
            role: "assistant",
            content: `Yes. Order **10248** shipped on July 16 with Federal Shipping and is on its way to Reims.

It contains three items:
- Queso Cabrales × 12
- Singaporean Hokkien Fried Mee × 10
- Mozzarella di Giovanni × 5

The freight charge was $32.38.`,
        },
    ],
    [
        { role: "user", content: "Which of my invoices are still unpaid?" },
        {
            role: "assistant",
            content: `Two invoices are open on your account:

| Invoice | Due | Amount |
| --- | --- | --- |
| INV-2041 | Sep 12 | $1,240.00 |
| INV-2057 | Sep 30 | $386.50 |

INV-2041 is due in five days. Both can be paid from the Billing page.`,
        },
    ],
    [
        { role: "user", content: "How many API calls did we make this month?" },
        {
            role: "assistant",
            content: `Your workspace has made **184,302 API calls** in September so far, about 61% of the 300,000 included in your plan.

The busiest day was **September 3** with 14,918 calls. At the current pace you'll end the month near 250,000, comfortably within the limit.`,
        },
    ],
    [
        { role: "user", content: "How many vacation days do I have left?" },
        {
            role: "assistant",
            content:
                "You have **9 days** left for 2026. You've used 16 of 25, and 3 more are already approved for October 6–8.",
        },
        { role: "user", content: "Do unused days carry over?" },
        {
            role: "assistant",
            content:
                "Partly. Up to **5 days** carry over into the next year and must be used by March 31. Anything beyond that is forfeited.",
        },
    ],
];
