<script setup lang="ts">
// Attribution
// https://www.joshwcomeau.com/animation/3d-button/

const props = defineProps({
  label: String,
  severity: {
    type: String,
    default: "primary",
    validator: (value: string) => ["primary", "secondary"].includes(value)
  }
});

const edgeStyles = computed(() => {
  if (props.severity === "primary") {
    return {
      background: `linear-gradient(
        to left,
        hsl(340deg 100% 16%) 0%,
        hsl(340deg 100% 32%) 8%,
        hsl(340deg 100% 32%) 92%,
        hsl(340deg 100% 16%) 100%
      )`
    };
  } else if (props.severity === "secondary") {
    return {
      background: `linear-gradient(
        to left,
        #0f0f0f 0%,
        #161616 8%,
        #161616 92%,
        #0f0f0f 100%
      )`
    };
  }
});

const frontStyles = computed(() => {
  if (props.severity === "primary") {
    return {
      background: "#f0003c"
    };
  } else if (props.severity === "secondary") {
    return {
      background: "#202020"
    };
  }
});
</script>

<template>
  <button class="pushable">
    <span class="shadow"></span>
    <span class="edge" :style="edgeStyles"></span>
    <span class="front ch-height" :style="frontStyles">
      <span v-if="props.label" class="ch-font">{{ props.label }}</span>
      <span v-else>
        <slot class="ch-font"/>
      </span>
    </span>
  </button>
</template>

<style scoped>
.pushable {
  position: relative;
  border: none;
  background: transparent;
  padding: 0;
  cursor: pointer;
  outline-offset: 4px;
  transition: filter 250ms;
}

.shadow {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border-radius: 12px;
  background: hsl(0deg 0% 0% / 0.25);
  will-change: transform;
  transform: translateY(2px);
  transition: transform 600ms cubic-bezier(0.3, 0.7, 0.4, 1);
}

.edge {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border-radius: 12px;
}

.front {
  display: block;
  position: relative;
  padding: 12px 42px;
  border-radius: 12px;
  font-size: 1.25rem;
  will-change: transform;
  transform: translateY(-4px);
  transition: transform 600ms cubic-bezier(0.3, 0.7, 0.4, 1);
}

.front * {
  color: #f0f0f0;
}

.front:hover * {
  color: white;
}

.front:active * {
  color: white;
}

.pushable:hover {
  filter: brightness(110%);
}

.pushable:hover .front {
  transform: translateY(-6px);
  transition: transform 250ms cubic-bezier(0.3, 0.7, 0.4, 1.5);
}

.pushable:active .front {
  transform: translateY(-2px);
  transition: transform 34ms;
}

.pushable:hover .shadow {
  transform: translateY(4px);
  transition: transform 250ms cubic-bezier(0.3, 0.7, 0.4, 1.5);
}

.pushable:active .shadow {
  transform: translateY(1px);
  transition: transform 34ms;
}

.pushable:focus:not(:focus-visible) {
  outline: none;
}

.pushable:focus {
  border-radius: 12px;
}
</style>