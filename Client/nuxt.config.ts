// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  css: ["~/assets/css/main.css", "~/assets/css/fonts.css"],
  devtools: { enabled: true },
  modules: ["@nuxtjs/tailwindcss", "nuxt-svgo"],
  runtimeConfig: {
    public: {
      baseURL: process.env.BASE_URL
    }
  },
  ssr: false,
  svgo: {
    componentPrefix: 'i',
  },
});