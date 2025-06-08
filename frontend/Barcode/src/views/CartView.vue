<template>
  <section class="max-w-4xl mx-auto mt-20 bg-white/90 rounded-2xl shadow-xl p-8 border border-neutral-200">
    <header class="flex justify-between items-center mb-8">
      <h2 class="text-3xl font-bold text-neutral-800">Seu Carrinho</h2>
    </header>

    <!-- Lista de Produtos -->
    <div class="space-y-6">
      <div v-if="!cartItems.length" class="text-center text-gray-500 py-8">
        Seu carrinho está vazio
      </div>

      <div v-for="item in cartItems" :key="item.productCompanyId"
        class="flex items-center justify-between bg-white p-6 rounded-xl shadow border border-gray-100">
        <!-- Informações do Produto -->
        <div class="flex items-center gap-6 flex-1">
          <img :src="item.imageUrl || 'placeholder.jpg'" alt="Produto" class="w-24 h-24 object-cover rounded-lg">

          <div class="flex-1">
            <h3 class="font-semibold text-xl mb-2">{{ item.productName }}</h3>
            <p class="text-gray-600 mb-2">Vendido por: {{ item.companyName }}</p>
            <p class="text-lg font-bold text-green-600">R$ {{ (item.price * item.quantity).toFixed(2) }}</p>

            <!-- Controles de quantidade simplificados -->
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

    <!-- Total -->
    <div v-if="cartItems.length" class="mt-8 pt-6 border-t">
      <div class="flex justify-between items-center">
        <span class="text-xl font-semibold">Total:</span>
        <span class="text-2xl font-bold text-green-600">
          R$ {{ totalPrice }}
        </span>
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