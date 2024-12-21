<script setup lang="ts">
import { gamesSet, getUrl } from "~/utils/game";
import type {GameType} from "~/types/enums";

definePageMeta({
  layout: "gradient"
});

let store = useBaseStore();
const { tryJoinRoom } = store;

function openGameDetails(type: GameType) {
  // navigateTo("/games/" + url);
  tryJoinRoom(type);
}

</script>

<template>
  <template v-for="(game, type) in gamesSet" :key="type">
    <PushableButton @click="openGameDetails(type)">
      {{ game.name }}
    </PushableButton>
  </template>
</template>

<style scoped>

</style>