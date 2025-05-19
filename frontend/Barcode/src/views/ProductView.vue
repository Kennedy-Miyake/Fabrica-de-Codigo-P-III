<script setup>
import ProductComponent from '../components/ProductComponent.vue'
import initBarcodeScanner from '../assets/Services/Barcode.js'

</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold text-gray-800 mb-8 text-center">
        Detalhes do Produto
      </h1>
      <ProductComponent />
    </div>
  </div>
</template>