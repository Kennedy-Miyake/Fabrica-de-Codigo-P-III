<template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getProductByBarcode } from '../assets/services/products.js'

const route = useRoute()
const barcode = route.params.barcode

const product = ref(null)
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const { data } = await getProductByBarcode(barcode)
    product.value = data
    console.log(product.value)
  } catch(error) {
    console.error(error)
    error.value = 'Não foi possível carregar o produto.'
  } finally {
    loading.value = false
  }
});
</script>