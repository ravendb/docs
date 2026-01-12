// This file has to be in JavaScript, otherwise @docusaurus/plugin-content-docs doesn't work properly
export default {
    guides: [
        {
            type: 'doc',
            id: 'home',
            label: 'Home',
            className: 'hidden' // <-- This makes the default item invisible, while still rendering the sidebar
        },
        {
            type: 'doc',
            id: 'example-guide',
            label: 'Example Guide'
        },
        {
            type: 'doc',
            id: 'other-guide',
            label: 'Other Guide'
        },
    ],
};
