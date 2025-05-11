<script setup>
import { onMounted } from 'vue'
import initBarcodeScanner from '../assets/Services/Barcode.js'

onMounted(() => {
  initBarcodeScanner()
})
</script>

<template>
  <main class="font-sans bg-gray-100 min-h-screen p-5 flex flex-col items-center">
    <h1 class="text-gray-800 text-2xl font-semibold mb-5">📦 Leitor de Código de Barras</h1>

    <div id="camera" class="w-full max-w-[640px] h-[200px] mx-auto relative rounded-lg overflow-hidden mb-5">
      <div id="linha-guia" class="absolute top-1/2 left-0 w-full h-[2px] bg-red-600 opacity-60 z-10"></div>
    </div>

    <div id="resultado" class="flex items-center justify-center gap-2 text-green-800 text-lg font-bold px-5 py-3 bg-green-100 border border-green-200 rounded-md">
      <span class="loader animate-spin w-5 h-5 border-4 border-green-400 border-t-transparent rounded-full"></span>
      Lendo código de barras...
    </div>
  </main>
</template>

<style scoped>
.animate-slide {
  animation: slide 1.5s linear infinite alternate;
}
</style>
