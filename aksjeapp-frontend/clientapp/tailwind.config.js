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
      'purple': '#3f3cbb',
      'midnight': '#121063',
      'metal': '#565584',
      'tahiti': '#3ab7bf',
      'silver': '#ecebff',
      'bubble-gum': '#ff77e9',
      'bermuda': '#78dcca',
      'card': '#ffffff',
      'navbar': '#ffffff',
      'background': "#ffffff",
      'stock-preview-line': "#020202",
      'gray': colors.gray,
      'black': colors.black,
      'red': colors.red,
      'green': colors.green,
      'blue': colors.blue,
    },
  },
  plugins: [],
}
