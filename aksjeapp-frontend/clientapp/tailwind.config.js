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
      'midnight': '#121063',
      'metal': '#565584',
      'tahiti': '#3ab7bf',
      'silver': '#ecebff',
      'bubble-gum': '#ff77e9',
      'bermuda': '#78dcca',
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

    },
  },
  plugins: [
    require('tailwind-scrollbar')({nocompatible: true}),
  ],
}
