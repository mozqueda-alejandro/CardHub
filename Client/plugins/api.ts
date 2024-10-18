import { ofetch } from 'ofetch'
import { defineNuxtPlugin, useRuntimeConfig } from "nuxt/app";

// Allows client-side fetching configured with a global header URL
// https://stackoverflow.com/a/75605263/18790415
export default defineNuxtPlugin(nuxtApp => {
    const runtimeConfig = useRuntimeConfig();
    const instance = ofetch.create({
        baseURL: runtimeConfig.public.baseURL as string,
        headers: {
            Accept: 'application/json'
        }
    }) as typeof ofetch;

    return {
        provide: {
            api: instance
        }
    }
})
