/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors')
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    colors: {
      transparent: 'transparent',
      current: 'currentColor',
      'white': '#ffffff',
      'card': '#ffffff',
      'navbar': '#ffffff',
      'background': "#ffffff",
      'stock-preview-line': "#08216c",
      'stock-preview-text-1': "#000000",
      'stock-preview-text-2': "#ffffff",
      'text-display': "#000000",
      'gradient-start': "#22C55E",
      'gradient-end': "#1D4ED8",
      'gray': colors.gray,
      'black': colors.black,
      'red': colors.red,
      'green': colors.green,
      'blue': colors.blue,
      'cyan': colors.cyan,
      'amber': colors.amber,
      'pink': colors.pink,
      'purple': colors.purple,
      'yellow': colors.yellow,

    },
  },
  plugins: [
    require('tailwind-scrollbar')({nocompatible: true}),
  ],
}
