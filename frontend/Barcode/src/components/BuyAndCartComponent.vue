<script setup lang="ts">
import CompanyCard from '../components/CompanyCardComponent.vue'
</script>

<template>
  <section class="bg-red-500 mt-6 max-w-1xl w-full mx-auto">
    <h1 class="text-2xl font-semibold text-black max-w-md mx-auto">Empresas que vendem o produto</h1>
    <div class="flex bg-blue-600 h-[400px] justify-center items-center">
      <CompanyCard />
    </div>
  </section>
</template>

<style scoped>

</style>