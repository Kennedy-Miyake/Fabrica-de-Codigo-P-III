
<script setup>
</script>

<template>
<section class="min-h-screen flex flex-col items-center justify-center gap-4 bg-transparent text-white px-6">
  <h1 class="text-4xl sm:text-5xl font-bold drop-shadow-lg">
    Bem-vindo ao BuyCode
  </h1>
  <p class="text-lg sm:text-xl text-center max-w-2xl leading-relaxed">
    Compare preços, leia códigos de barras e encontre a melhor oferta sem complicação.
  </p>
  <router-link to='/scanner' class="mt-8 inline-block rounded-2xl px-6 py-3 font-semibold bg-[#4facfe] hover:bg-[#8ce7fe] transition-colors shadow-md animated-border">
    Começar Agora
  </router-link>
</section>
</template>

<style scoped>
@keyframes pulse-border {
  0%, 100% {
    box-shadow: 0 0 0 0 rgba(79, 172, 254, 0.7);
  }
  50% {
    box-shadow: 0 0 12px 4px rgba(79, 172, 254, 0.6);
  }
}

.animated-border {
  animation: pulse-border 2s infinite ease-in-out;
}
</style>
