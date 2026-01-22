<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div class="rayito"></div>
</template>

<script setup></script>

<style scoped>
.rayito {
  animation: parpadeo 1000ms infinite;
  border: 4px solid rgba(0, 0, 0, 0);
  border-bottom: 22px solid #ffdf00;
  transform: rotateZ(-160deg);
  padding: 2px;
  width: 0;
  height: 0;
  margin: 0;
  overflow: visible;
  border-top: 0 solid;
  border-radius: 0;
  /* Controla la posicion del primer rayo lo cual afecta el segundo por ser after por padre*/
  transform-origin: 50% 42% 0;
}

.rayito:after {
  animation: parpadeoafter 1000ms infinite;
  content: '';
  top: -9px;
  left: -8px;
  border: 5px solid rgba(0, 0, 0, 0);
  border-bottom: 25px solid #ffdf00;
  transform: rotateZ(4deg);
  padding: 0;
  width: 0;
  height: 0;
  position: absolute;
  margin: 0;
  overflow: visible;
  border-top: 0 solid;
  border-radius: 0;
  transform-origin: 50% 50% 0;
}

@keyframes parpadeo {
  0% {
    border-bottom: 22px solid #ffdf00;
  }
  50% {
    border-bottom: 22px solid #9b903d;
  }
  100% {
    border-bottom: 22px solid #ffdf00;
  }
}

@keyframes parpadeoafter {
  0% {
    border-bottom: 25px solid #ffdf00;
  }
  50% {
    border-bottom: 25px solid #9b903d;
  }
  100% {
    border-bottom: 25px solid #ffdf00;
  }
}
</style>
