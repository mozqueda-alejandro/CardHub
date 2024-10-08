<script setup lang="ts">
definePageMeta({
  layout: "gradient"
});

const name = ref<number>();
const pin = ref<number>();

function getSuitTransform(translateX: number, translateY: number, rotateDeg: number) {
  return {
    transform: `translate(${translateX}vh, ${translateY}vh) rotate(${rotateDeg}deg)`,
  }
}

</script>

<template>
  <i-diamond class="suit top-0 left-0 opacity-20" :style="getSuitTransform(25, -20, -28)" :fontControlled="false" filled/>
  <i-spade class="suit top-0 right-0 opacity-20" :style="getSuitTransform(10, -12, -144)" :fontControlled="false" filled/>
  <i-club class="suit bottom-0 left-0 opacity-30" :style="getSuitTransform(-10, 10, -36)" :fontControlled="false" filled/>
  <i-heart class="suit bottom-0 right-0 opacity-20" :style="getSuitTransform(-2, 38, 0)" :fontControlled="false" filled/>

  <div class="h-screen w-screen flex justify-center justify-items-center">
    <div class="grid grid-rows-[50%_25%_25%] w-[300px] h-full">
      <div class="flex flex-col items-center justify-end gap-4">
        <i-logo-combination class="w-64 h-64" :fontControlled="false" filled/>
      </div>
      <div class="flex flex-col justify-center gap-4 mb-8">
        <InputNumber v-model="pin" placeholder="Game Pin" :maxLength="6"/>
        <PushableButton>Enter</PushableButton>
      </div>
      <div class="flex flex-col w-auto">
        <PushableButton severity="secondary">Start a game</PushableButton>
      </div>
    </div>
  </div>

</template>

<style scoped>
body {
  background-color: gray;
}

.suit {
  position: absolute;
  height: 70%;
}
</style>