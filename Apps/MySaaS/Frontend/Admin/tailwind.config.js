const defaultTheme = require("tailwindcss/defaultTheme");

/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx,css,md,mdx,html,json,scss}"],
    darkMode: "class",
    theme: {
        extend: {
            colors: {
                "google-blue": "#3367d6",
            },
            fontFamily: {
                mono: ["Chivo Mono", ...defaultTheme.fontFamily.mono],
                sans: ["Inter", ...defaultTheme.fontFamily.sans],
                tight: ["Inter Tight", ...defaultTheme.fontFamily.sans],
            },
        },
    },
    plugins: [require("@tailwindcss/forms"), require("@tailwindcss/line-clamp"), require("@tailwindcss/aspect-ratio")],
};
