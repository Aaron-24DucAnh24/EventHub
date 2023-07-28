/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  important: '#root',
  theme: {
    extend: {},
    colors: {
      'main': '#5669FF',
      'picking': '#88050c',
      'picked': '#ccc',
      'vip': '#e50914',
      'sweetbox': '#ee7a7a',
    }
  },
  plugins: [],
  corePlugins: {
    preflight: false // <== disable this!
  },
}