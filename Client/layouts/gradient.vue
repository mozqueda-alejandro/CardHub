<template>
  <div id="background">
    <slot />
  </div>
</template>

<style scoped>
#background {
  height: 100%;
  width: 100%;
  background: linear-gradient(20deg, #040404 0%, #232323 100%);
  position: fixed;
  top: 0;
  left: 0;
}
</style>