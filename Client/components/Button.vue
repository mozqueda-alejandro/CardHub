<script setup lang="ts">
import { defineProps, computed } from "vue";

const props = defineProps({
  label: String,
  severity: {
    type: String,
    default: "primary",
    validator: (value: string) => ["primary", "secondary"].includes(value)
  }
});

const buttonClasses = computed(() => {
  return {
    "bg-[#d60e26] text-white": props.severity === "primary",
    "bg-zinc-700 text-black": props.severity === "secondary",
  };
});
</script>

<template>
  <button class="ch-height ch-radius" :class="buttonClasses">
    <slot>{{ props.label }}</slot>
  </button>
</template>

<style scoped>
button {
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover:not([disabled]) {
  opacity: 0.9;
}
</style>