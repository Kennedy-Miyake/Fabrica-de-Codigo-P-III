<template>
  <!-- Informação do Produto -->
  <section class="bg-white/90 shadow-xl backdrop-blur-md rounded-2xl p-8 max-w-xl mx-auto mt-32 border border-neutral-200">
    <div v-if="loading" class="text-center text-neutral-500 text-lg py-10">Carregando...</div>
    <div v-else-if="error" class="text-red-500 text-center font-semibold py-6">{{ error }}</div>
    <div v-else class="flex flex-col items-center gap-6">
      <h3 class="text-3xl font-bold text-neutral-800 tracking-tight mb-2">{{ product.name }}</h3>
      <!-- Imagem do Produto -->
      <p class="text-center text-neutral-500 text-lg py-4 break-all">{{ product.imageUrl }}</p>
      <!--descriçao-->
      <div class="w-full flex flex-col items-center">
        <h4 class="text-lg font-semibold text-neutral-700 mb-1">Descrição</h4>
        <p class="text-base text-neutral-900 bg-neutral-100 rounded-lg px-4 py-2 text-center w-full border border-neutral-200">{{ product.description }}</p>
      </div>
    </div>
  </section>

  <!-- Informação das Empresas que Vendem o Produto Acima -->
  <section class="bg-gradient-to-r from-blue-600 via-blue-500 to-blue-400 mt-10 max-w-3xl w-full mx-auto rounded-2xl shadow-xl border border-blue-200 p-8">
    <h2 class="text-2xl font-bold text-white text-center mb-8 drop-shadow">Empresas que vendem o produto</h2>
    <div class="flex flex-row justify-center gap-6 min-h-[180px]">
      <CompanyCard
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
import CompanyCard from "../components/CompanyCardComponent.vue";

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