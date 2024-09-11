/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{html,cshtml}",
        "./wwwroot/**/*.{html,cshtml}"
    ],
    theme: {
        extend: {
            fontFamily: {
                'MyFont': ["Playwrite CU", 'cursive'],
                'casual': ["Roboto Mono", 'monospace']
            },
            screens: {
                'sm': '640px',
                'md': '768px',
                'lg': '1024px',
                'xl': '1280px',
                '2xl': '1536px'
            }
        }
    },
    plugins: [],
};
