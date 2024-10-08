<template>
  <input
      :placeholder="isFocused ? '' : props.placeholder"
      @focus="isFocused = true"
      @blur="isFocused = false"
      v-model="model"
      @input="handleInput"
      class="ch-height ch-radius w-full border-zinc-700 placeholder-[#777777]"
  />
</template>

<script setup>
import { defineProps } from 'vue';

const model = defineModel();
const props = defineProps({
  placeholder: String,
  maxLength: Number
});

const handleInput = (e) => {
  let value = e.target.value.replace(/[^0-9]/g, '');

  if (props.maxLength && value.length > props.maxLength) {
    value = value.slice(0, props.maxLength)
  }

  model.value = value;
};

const isFocused = ref(false);

</script>

<style scoped>
input {
  background-color: #27272a;
  border-width: 2px;
  transition: background-color 0.2s ease, color 0.2s ease, border-color 0.2s ease, border-width 0.2s ease;
  text-align: center;
}

input:focus {
  outline: none;
  border-color: #f8f8f8;
  border-width: 3px;
}

::placeholder {
  text-align: center;
}

</style>
