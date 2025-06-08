<template>
  <section
    class="max-w-6xl mx-auto mt-20 grid grid-cols-1 lg:grid-cols-3 gap-6 bg-white/90 backdrop-blur-md rounded-2xl shadow-2xl p-8 border border-neutral-200 relative">
    <!-- Header -->
    <header class="flex justify-between items-center mb-8 lg:col-span-3 border-b pb-4">
      <h2 class="text-3xl font-extrabold text-neutral-800">Seu Carrinho de Compras</h2>
    </header>

    <!-- Lista de Produtos (2/3 do grid) -->
    <div class="space-y-6 lg:col-span-2">
      <div v-if="!cartItems.length" class="text-center text-gray-500 py-8">
        Seu carrinho está vazio
      </div>

      <div v-for="item in cartItems" :key="item.productCompanyId"
        class="bg-white rounded-xl shadow hover:shadow-lg transition-shadow duration-200 p-6 flex items-center justify-between">
        <!-- Informações do Produto -->
        <div class="flex items-center gap-6 flex-1">
          <img :src="item.imageUrl || 'placeholder.jpg'" alt="Produto" class="w-24 h-24 object-cover rounded-lg">

          <div class="flex-1">
            <h3 class="font-semibold text-xl mb-2">{{ item.productName }}</h3>
            <p class="text-gray-600 mb-2">Vendido por: {{ item.companyName }}</p>
            <p class="text-lg font-bold text-green-600">R$ {{ (item.price * item.quantity).toFixed(2) }}</p>

            <!-- Controles de quantidade -->
            <div class="flex items-center gap-4 mt-4">
              <button @click="decreaseQuantity(item)" :disabled="item.quantity <= 1"
                class="w-8 h-8 rounded-full bg-neutral-200 hover:bg-neutral-300 flex items-center justify-center disabled:opacity-50 disabled:cursor-not-allowed">
                <span class="text-lg font-bold">-</span>
              </button>

              <span class="text-lg font-semibold w-8 text-center">{{ item.quantity }}</span>

              <button @click="increaseQuantity(item)" :disabled="!item.stock || item.quantity >= item.stock"
                class="w-8 h-8 rounded-full bg-neutral-200 hover:bg-neutral-300 flex items-center justify-center disabled:opacity-50 disabled:cursor-not-allowed">
                <span class="text-lg font-bold">+</span>
              </button>

              <!-- Indicador de estoque (opcional) -->
              <span v-if="item.stock" class="text-sm text-gray-500">
                ({{ item.stock - item.quantity }} restantes)
              </span>
            </div>
          </div>
        </div>

        <!-- Botão Remover -->
        <button @click="removeItem(item.productCompanyId)"
          class="px-4 py-2 text-red-600 hover:text-red-700 hover:bg-red-50 rounded-lg transition-colors">
          Remover
        </button>
      </div>
    </div>

    <!-- Resumo (1/3 do grid) -->
    <div v-if="cartItems.length" class="lg:col-span-1 bg-gray-50 rounded-xl p-6 shadow-md sticky top-24">
      <div class="flex justify-between items-center mb-4">
        <span class="text-lg font-medium text-neutral-700">Subtotal</span>
        <span class="text-lg font-medium text-green-600">R$ {{ totalPrice }}</span>
      </div>
      <div class="flex justify-between items-center mb-4">
        <span class="text-base text-neutral-500">Frete</span>
        <span class="text-base text-green-500">Grátis</span>
      </div>
      <div class="border-t pt-4 flex justify-between items-center">
        <span class="text-xl font-bold text-neutral-800">Total</span>
        <span class="text-2xl font-extrabold text-green-700">R$ {{ totalPrice }}</span>
      </div>
      <button @click="finalizePurchase"
        class="mt-6 w-full bg-green-500 hover:bg-green-800 text-white py-3 rounded-lg font-semibold shadow transition-colors duration-200">
        Finalizar Compra
      </button>
    </div>

    <!-- Loading Modal -->
    <div v-if="loading"
      class="fixed inset-1 flex flex-col items-center justify-center rounded-3xl bg-neutral-500/50 z-50 overflow-hidden">
      <div class="bg-white p-6 rounded-xl w-80 text-center">
        <p class="mb-3 font-semibold text-lg">Finalizando compra</p>
        <div class="w-full bg-gray-200 rounded-full h-3 overflow-hidden">
          <div class="h-3 bg-green-400 transition-all duration-200" :style="{ width: progress + '%' }"></div>
        </div>
      </div>
    </div>

    <!-- Success Modal -->
    <div v-if="success"
      class="fixed inset-1 flex flex-col items-center justify-center rounded-3xl bg-green-600/50 z-50 overflow-hidden">
      <div class="bg-white p-6 rounded-xl w-96 text-center space-y-2">
        <p class="font-semibold text-lg text-green-700">Compra finalizada!</p>
        <p class="text-base font-semibold text-lg text-gray-600">Boleto encaminhado para o seu e-mail!</p>
        <p class="text-base text-gray-600">Obrigado, volte sempre 😉</p>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const cartItems = ref([])

// Carrega os itens salvos do localStorage
const loadSavedItems = () => {
  const savedItems = localStorage.getItem('cartItems')
  if (savedItems) {
    cartItems.value = JSON.parse(savedItems)
  }
}

// Salva os itens no localStorage
const saveItems = () => {
  localStorage.setItem('cartItems', JSON.stringify(cartItems.value))
}

// Funções simplificadas para manipulação do carrinho
const increaseQuantity = (item) => {
  if (!item.stock || item.quantity >= item.stock) {
    console.log('Limite de estoque atingido')
    return
  }
  item.quantity++
  saveItems()
}

const decreaseQuantity = (item) => {
  const updatedItem = cartItems.value.find(i => i.productCompanyId === item.productCompanyId)
  if (updatedItem && updatedItem.quantity > 1) {
    updatedItem.quantity--
    saveItems()
  }
}

const removeItem = (itemId) => {
  cartItems.value = cartItems.value.filter(item => item.productCompanyId !== itemId)
  saveItems()
}

// Adiciona novo item ao carrinho com verificação de estoque
const addNewItem = (newItem) => {
  const existingItem = cartItems.value.find(
    item => item.productCompanyId === newItem.productCompanyId
  )

  if (existingItem) {
    if (existingItem.quantity >= newItem.stock) {
      console.log('Limite de estoque atingido')
      return
    }
    existingItem.quantity++
  } else {
    cartItems.value.push({
      ...newItem,
      quantity: 1,
      stock: Number(newItem.stock) || 0
    })
  }
  saveItems()
}

// Calcula o preço total
const totalPrice = computed(() => {
  return cartItems.value
    .reduce((total, item) => {
      // Garante que price e quantity são números
      const price = Number(item.price) || 0
      const quantity = Number(item.quantity) || 0
      return total + (price * quantity)
    }, 0)
    .toFixed(2)
})

const loading = ref(false)
const success = ref(false)
const progress = ref(0)

// Função para finalizar compra
const finalizePurchase = () => {
  loading.value = true
  progress.value = 0

  const interval = setInterval(() => {
    if (progress.value < 100) {
      progress.value += 10
    } else {
      clearInterval(interval)
      loading.value = false
      success.value = true
      setTimeout(() => {
        success.value = false
        cartItems.value = [] // Limpa o carrinho
        saveItems()
      }, 2000)
    }
  }, 200)
}

onMounted(() => {
  loadSavedItems()

  const params = route.query
  if (Object.keys(params).length > 0) {
    addNewItem({
      productCompanyId: Number(params.productCompanyId),
      productName: params.productName,
      companyName: params.companyName,
      price: Number(params.price),
      stock: Number(params.stock),
      imageUrl: params.imageUrl
    })
  }
})
</script>