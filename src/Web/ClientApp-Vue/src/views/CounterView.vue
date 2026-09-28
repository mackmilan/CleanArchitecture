<script setup lang="ts">
import { ref } from 'vue';

const count = ref(0);
</script>

<template>
  <section>
    <h1>Counter</h1>
    <p>This is a simple example of a Vue component written in TypeScript.</p>
    <p aria-live="polite">Current count: <strong>{{ count }}</strong></p>
    <button type="button" @click="count++">Increment</button>
  </section>
</template>
