<script setup lang="ts">
definePageMeta({
  layout: "gradient"
});

import { GameType } from "~/types/enums";

const baseStore = useBaseStore();
const { tryJoinRoom } = baseStore;


const pin = ref<string>();
const isValidPin = computed(() => {
  const digitRegex = /^\d+$/;
  return pin.value?.length === 1 && digitRegex.test(pin.value);
});

async function enterPin() {
  console.log("attempting WS-PL");
  await tryJoinRoom(GameType.Pl);
}

async function navigateToLibrary() {
  console.log("attempting WS-GB");
  await tryJoinRoom(GameType.Une);
}

// #region Styles
function getSuitTransform(translateX: number, translateY: number, rotateDeg: number) {
  return {
    transform: `translate(${ translateX }vh, ${ translateY }vh) rotate(${ rotateDeg }deg)`
  }
}

// #endregion

</script>

<template>
  <i-exp-diamond class="suit red top-0 left-0 opacity-[6%]" :style="getSuitTransform(10, -20, -38)"
                 :fontControlled="false" :filled="false"/>
  <i-exp-spade class="suit black top-0 right-0 opacity-[16%]" :style="getSuitTransform(10, -12, -144)"
               :fontControlled="false" :filled="false"/>
  <i-exp-club class="suit black bottom-0 left-0 opacity-[30%]" :style="getSuitTransform(-10, 10, -36)"
              :fontControlled="false" :filled="false"/>
  <i-exp-heart class="suit red bottom-0 right-0 opacity-[6%]" :style="getSuitTransform(18, 48, 0)"
               :fontControlled="false" :filled="false"/>

  <div class="h-screen w-screen flex justify-center justify-items-center">
    <div class="grid grid-rows-[50%_25%_25%] w-[20%] h-full">
      <div class="flex flex-col items-center justify-end gap-4">
        <i-logo-combination class="w-64 h-64" :fontControlled="false" filled/>
      </div>
      <div class="flex flex-col justify-center gap-4 mb-8">
        <InputNumber v-model="pin" placeholder="Game Pin" :maxLength="6"/>
        <PushableButton @click="enterPin" :disabled="false">Enter</PushableButton>
      </div>
      <div class="flex flex-col w-auto">
        <PushableButton @click="" severity="secondary">Start a game</PushableButton>
      </div>
    </div>
  </div>

</template>

<style scoped>
.glass {
  background-color: #141414;
}

.red {
  fill: #e6354e;
}

.black {
  fill: #09090b;
}

.suit {
  position: absolute;
  height: 76%;
}
</style>