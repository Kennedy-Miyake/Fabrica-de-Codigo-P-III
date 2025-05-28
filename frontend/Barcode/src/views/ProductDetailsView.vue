<template>
  <!-- Informação do Produto -->
  <section class="bg-gray-500 p-6 max-w-xl mx-auto mt-[100px]">
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
  </section>

  <!-- Informação das Empresas que Vendem o Produto Acima -->
  <section class="bg-red-500 mt-6 max-w-1xl w-full mx-auto">
    <h1 class="text-2xl font-semibold text-black max-w-md mx-auto">Empresas que vendem o produto</h1>
    <div class="flex bg-blue-600 h-[400px] justify-center items-center">
      <CompanyCardComponent
        v-for="c in productCompanies"
        :company="c"
      />
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getProductByBarcode } from '../assets/services/products.js'
import { getProductCompaniesByBarcode } from "../assets/services/productCompanies.js";
import CompanyCardComponent from "../components/CompanyCardComponent.vue";

const route = useRoute()
const barcode = route.params.barcode

const product = ref(null)

const productCompanies = ref(null)

const loading = ref(true)
const error = ref('')

const fetchProduct = async() => {
  try {
    const { data } = await getProductByBarcode(barcode)
    product.value = data
  } catch (error) {
    console.error(error)
    error.value = 'Não foi possível carregar o produto.'
  } finally {
    loading.value = false
  }
}

const fetchProductCompanies = async() => {
  try {
    const { data } = await getProductCompaniesByBarcode(barcode)
    productCompanies.value = data
    console.log(productCompanies.value, 'Empresas que vendem o produto')
  } catch (error) {
    console.error(error)
    error.value = 'Não foi possível carregar as empresas'
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  fetchProduct()
  fetchProductCompanies()
});
</script>