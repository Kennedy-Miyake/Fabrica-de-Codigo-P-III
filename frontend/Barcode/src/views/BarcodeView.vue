<script setup>
import { onMounted } from 'vue'
import initBarcodeScanner from '../assets/Services/Barcode.js'

onMounted(() => {
  initBarcodeScanner()
})
</script>

<template>
  <div class="flex items-center justify-center min-h-screen">
    <main class="font-sans min-h-[400px] w-full max-w-lg p-8 flex flex-col items-center bg-neutral-900 rounded-2xl shadow-2xl border border-neutral-700 relative">
      <h1 class="text-neutral-100 text-2xl font-semibold mb-5 text-center">📦 Leitor de Código de Barras</h1>

      <div id="camera" class="w-full max-w-[500px] h-[180px] mx-auto relative rounded-lg overflow-hidden mb-5 bg-black z-10">
        <div id="linha-guia" class="absolute top-1/2 left-0 w-full h-[2px] bg-red-600 opacity-60 z-10"></div>
      </div>

      <div id="resultado" class="flex items-center justify-center gap-2 text-green-400 text-lg font-bold px-5 py-3 bg-green-900/30 border border-green-700 rounded-md">
        <span class="loader animate-spin w-5 h-5 border-4 border-green-400 border-t-transparent rounded-full"></span>
        Lendo código de barras...
      </div>
    </main>
  </div>
</template>

<style scoped>
.animate-slide {
  animation: slide 1.5s linear infinite alternate;
}
</style>
