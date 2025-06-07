<template>
  <!-- Informação do Produto -->
  <section
    class="bg-white/90 shadow-xl backdrop-blur-md rounded-2xl p-8 max-w-xl mx-auto mt-32 border border-neutral-200">
    <div v-if="loading" class="text-center text-neutral-500 text-lg py-10">Carregando...</div>
    <div v-else-if="error" class="text-red-500 text-center font-semibold py-6">{{ error }}</div>
    <div v-else class="flex flex-col items-center gap-6">
      <h3 class="text-3xl font-bold text-neutral-800 tracking-tight mb-2">{{ product.name }}</h3>
      <!-- Imagem do Produto -->
      <img :src="product.imageUrl" alt="">
      <p class="text-center text-neutral-500 text-lg py-4 break-all">{{ product.imageUrl }}</p>
      <!--descriçao-->
      <div class="w-full flex flex-col items-center">
        <h4 class="text-lg font-semibold text-neutral-700 mb-1">Descrição</h4>
        <p
          class="text-base text-neutral-900 bg-neutral-100 rounded-lg px-4 py-2 text-center w-full border border-neutral-200">
          {{ product.description }}
        </p>
      </div>

      <!-- botão de adicionar ao carrinho -->
      <button @click="handleAddToCart"
        class="mt-4 px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors">
        {{ cartLoading ? 'Adicionando...' : 'Adicionar ao Carrinho' }}
      </button>
      <p v-if="cartMessage" :class="cartMessageType">{{ cartMessage }}</p>
    </div>
  </section>

  <!-- Informação das Empresas que Vendem o Produto Acima -->
  <section
    class="bg-gradient-to-r from-blue-600 via-blue-500 to-blue-400 mt-10 max-w-3xl w-full mx-auto rounded-2xl shadow-xl border border-blue-200 p-8">
    <h2 class="text-2xl font-bold text-white text-center mb-8 drop-shadow">Empresas que vendem o produto</h2>

    <!-- carregando a pagina -->
    <div v-if="loading" class="text-white text-center">
      Carregando empresas...
    </div>

    <!-- Erro -->
    <div v-else-if="error" class="text-red-200 text-center">
      {{ error }}
    </div>

    <!-- Companias não cadastradas -->
    <div v-else-if="!productCompanies?.length" class="text-white text-center">
      Nenhuma empresa encontrada vendendo este produto.
    </div>

    <!-- Companias -->
    <div v-else class="flex flex-row justify-center gap-6 min-h-[180px] flex-wrap">
      <CompanyCard v-for="company in productCompanies" :key="company.companyId" :company="company" />
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getProductByBarcode } from '../assets/services/products.js'
import { getProductInCompany, getAllCompanies } from "../assets/services/productCompanies.js"
import { PostOrderItem } from '../assets/services/cart.js'
import CompanyCard from "../components/CompanyCardComponent.vue";

const route = useRoute()

const product = ref(null)
const productCompanies = ref(null)
const loading = ref(true)
const error = ref('')


// Novos estados para controle do carrinho
const cartLoading = ref(false)
const cartMessage = ref('')
const cartMessageType = ref('')

// Nova função para adicionar ao carrinho
const handleAddToCart = async () => {
  if (!product.value || !productCompanies.value?.[0]) {
    cartMessage.value = 'Erro: Produto não encontrado'
    cartMessageType.value = 'text-red-500'
    return
  }

  cartLoading.value = true
  try {
    const companyId = productCompanies.value[0].id
    await PostOrderItem(companyId, product.value.id, 1)
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
  try {
    const { data } = await getProductByBarcode(route.params.barcode)
    product.value = data
  } catch (error) {
    console.error(error)
    error.value = 'Não foi possível carregar o produto.'
  } finally {
    loading.value = false
  }
}

const fetchProductCompanies = async () => {
  try {
    if (!product.value?.productId) {
      console.error('ID do produto não encontrado')
      return
    }

    // Busca todas as empresas primeiro
    const { data: companies } = await getAllCompanies()

    // Para cada empresa, verifica se tem o produto
    const availableCompanies = []

    for (const company of companies) {
      try {
        const { data: productCompany } = await getProductInCompany(company.companyId, product.value.productId)
        if (productCompany) {
          // Combina os dados da empresa com os dados do produto naquela empresa
          availableCompanies.push({
            ...company,
            price: productCompany.price,
            stock: productCompany.stock
          })
        }
      } catch (err) {
        // Se der erro 404 significa que a empresa não tem o produto
        if (err.response?.status !== 404) {
          console.error(`Erro ao verificar produto na empresa ${company.companyId}:`, err)
        }
      }
    }

    productCompanies.value = availableCompanies
    console.log('Empresas encontradas:', availableCompanies)

  } catch (error) {
    console.error('Erro ao buscar empresas:', error)
    error.value = 'Não foi possível carregar as empresas'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchProduct()
  fetchProductCompanies()
})

watch(
  () => route.params.barcode,
  () => {
    loading.value = true
    error.value = ''
    fetchProduct()
    fetchProductCompanies()
  }
)
</script>
