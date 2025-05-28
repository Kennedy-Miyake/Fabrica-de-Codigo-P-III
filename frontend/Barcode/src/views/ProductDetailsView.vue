<template>
  <div class="bg-gray-500 p-6 max-w-xl mx-auto mt-[100px]">
    <div v-if="loading">Carregando...</div>
    <div v-else-if="error" class="text-red-500">{{ error }}</div>
    <div v-else class="flex flex-col items-center gap-4 space-y-4">
      <h3 class="text-2xl font-bold text-black">{{ product.name }}</h3>
      <p class="text-sm text-black">{{ product.imageUrl }}</p>
      <div class="flex flex-col items-center">
        <h3 class="text-2xl font-bold text-black bg-red-500">Descrição</h3>
        <p class="text-sm text-black bg-white">{{ product.description }}</p>
      </div>
    </div>
  </div>
  <BuyAndCart />
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getProductByBarcode } from '../assets/services/products.js'
import { getProductCompaniesByBarcode } from '../assets/services/productCompanies.js';
import BuyAndCart from '../components/BuyAndCartComponent.vue'

const route = useRoute()
const barcode = route.params.barcode

const product = ref(null)

const productCompanies = ref()

const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    const { data } = await getProductByBarcode(barcode)
    product.value = data
  } catch(error) {
    console.error(error)
    error.value = 'Não foi possível carregar o produto.'
  } finally {
    loading.value = false
  }
});
</script>