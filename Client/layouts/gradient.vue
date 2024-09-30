<template>
  <div id="background">
    <slot />
  </div>
</template>

<style scoped>
#background {
  height: 100%;
  width: 100%;
  background: linear-gradient(20deg, #000000 0%, #313134 100%);
  position: fixed;
  top: 0;
  left: 0;
}
</style>