<script setup lang="ts">
const cart = useCartStore()
</script>

<template>
  <div>
    <header>
      <NuxtLink to="/">EventHub</NuxtLink>
      <span>Warenkorb: {{ cart.totalCount }} ({{ cart.totalPrice }} €)</span>
    </header>
    <NuxtPage />
  </div>
</template>