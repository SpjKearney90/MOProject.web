/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./wwwroot/**/*.{html,js}",
        "./Pages/**/*.{html,js}",
    ],
    theme: {
        extend: {
            fontFamily: {
                casual: ['Roboto Mono', 'monospace'],
                primary: ['Playfair Display', 'cursive'],
            },
        },
    },
    plugins: [],
};
