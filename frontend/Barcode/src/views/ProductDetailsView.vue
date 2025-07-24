<template>
  <!-- Container principal -->
  <div class="max-w-6xl mx-auto px-4 py-8 space-y-8">
    <!-- Card de detalhes do produto -->
    <section class="bg-white rounded-2xl shadow-xl overflow-hidden">
      <div v-if="loading" class="text-center text-neutral-500 text-lg py-10">Carregando...</div>
      <div v-else-if="error" class="text-red-500 text-center font-semibold py-6">{{ error }}</div>
      <div v-else class="flex flex-col md:flex-row">
        <!-- Imagem do produto -->
        <div class="md:w-1/2 p-6">
          <img :src="product.imageUrl" alt="Produto" class="w-full h-[400px] object-cover rounded-xl shadow-md">
        </div>

        <!-- Informações do produto -->
        <div class="md:w-1/2 p-8 flex flex-col justify-between bg-gradient-to-br from-gray-50 to-gray-100">
          <div class="space-y-4">
            <h1 class="text-3xl font-bold text-gray-800">{{ product.name }}</h1>
            <p class="text-gray-600">{{ product.description }}</p>
            <div class="bg-blue-50 p-4 rounded-lg">
              <span class="text-sm font-medium text-blue-600">Código de Barras:</span>
              <p class="font-mono text-blue-800">{{ product.barCode }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Card das empresas -->
    <section class="bg-white rounded-2xl shadow-xl p-8">
      <h2 class="text-2xl font-bold text-gray-800 mb-6">Disponível em</h2>

      <!-- carregando -->
      <div v-if="companiesLoading" class="text-center text-gray-500 py-4">
        Carregando empresas...
      </div>

      <!-- Verifica erro -->
      <div v-else-if="error" class="text-red-500 text-center py-4">
        {{ error }}
      </div>

      <!-- Caso não tennha empresas vendendo o produto  -->
      <div v-else-if="!productCompanies?.length" class="text-center text-gray-500 py-8">
        Nenhuma empresa encontrada vendendo este produto.
      </div>

      <!-- Companias grid -->
      <div v-else class="grid grid-cols-1 gap-4">
        <CompanyCard v-for="company in productCompanies" :key="company.companyId" :company="company"
          :product="product" />
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getProductByBarcode } from '../assets/services/products.js'
import { getProductInCompany, getAllCompanies } from "../assets/services/productCompanies.js"
import { cartService } from '../assets/services/cart'
import CompanyCard from "../components/CompanyCardComponent.vue";

const route = useRoute()

const product = ref(null)
const productCompanies = ref([])
const loading = ref(true)
const error = ref('')
const companiesLoading = ref(false)

// controle do carrinho
const cartLoading = ref(false)
const cartMessage = ref('')
const cartMessageType = ref('')

// adicionar ao carrinho
const handleAddToCart = async () => {
  if (!product.value || !productCompanies.value?.[0]) {
    cartMessage.value = 'Erro: Produto não encontrado'
    cartMessageType.value = 'text-red-500'
    return
  }

  cartLoading.value = true
  try {
    await cartService.addToCart({
      productCompanyId: productCompanies.value[0].productCompanyId,
      quantity: 1
    })
    cartMessage.value = 'Produto adicionado ao carrinho!'
    cartMessageType.value = 'text-green-500'
  } catch (err) {
    console.error('Erro ao adicionar ao carrinho:', err)
    cartMessage.value = 'Erro ao adicionar ao carrinho'
    cartMessageType.value = 'text-red-500'
  } finally {
    cartLoading.value = false
    // Limpa a mensagem após 3 segundos
    setTimeout(() => {
      cartMessage.value = ''
    }, 3000)
  }
}

const fetchProduct = async () => {
  loading.value = true
  error.value = ''
  try {
    const { data } = await getProductByBarcode(route.params.barcode)
    product.value = data

    // Chama fetchProductCompanies somente após ter os dados do produto
    await fetchProductCompanies(data.productId)
  } catch (error) {
    console.error('Erro ao buscar produto:', error)
    error.value = 'Não foi possível carregar o produto.'
  } finally {
    loading.value = false
  }
}

const fetchProductCompanies = async (productId) => {
  if (!productId) return

  companiesLoading.value = true
  try {
    const { data: companies } = await getAllCompanies()

    const promises = companies.map(async (company) => {
      try {
        const response = await getProductInCompany(company.companyId, productId)
        if (response?.data) {
          return {
            ...company,
            price: response.data.price,
            stock: response.data.stock,
            productCompanyId: response.data.productCompanyId
          }
        }
      } catch (err) {
        console.log(`Produto não encontrado na empresa ${company.name}`)
        return null
      }
    })

    const results = await Promise.all(promises)
    productCompanies.value = results.filter(Boolean)

  } catch (error) {
    console.error('Erro ao carregar empresas:', error)
    error.value = 'Erro ao carregar as empresas'
  } finally {
    companiesLoading.value = false
  }
}

//usar tela de carregamento
onMounted(() => {
  fetchProduct()
})

watch(
  () => route.params.barcode,
  () => {
    if (route.params.barcode) {
      fetchProduct()
    }
  }
)
</script>
