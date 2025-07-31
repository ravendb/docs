import type { Plugin } from '@docusaurus/types';

const tailwindPlugin: Plugin = function tailwindPlugin(_context, _options) {
    return {
        name: 'tailwind-plugin',
        configurePostCss(postcssOptions) {
            postcssOptions.plugins = [
                require('@tailwindcss/postcss'),
                require('autoprefixer'),
            ];
            return postcssOptions;
        },
    };
};

export default tailwindPlugin;