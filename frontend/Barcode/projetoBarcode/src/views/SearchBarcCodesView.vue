<script setup>
import BarcodeScanner from '../components/BarcodeScannerComponent.vue'
</script>

<template>
  <main class="container mx-auto px-4 py-6">
    <h1 class="text-3xl font-bold text-center mb-6">
      Leitor de Código de Barras
    </h1>

    <BarcodeScanner />
  </main>
</template>
