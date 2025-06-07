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
            <p class="text-lg font-bold text-green-600">R$ {{ item.price?.toFixed(2) }}</p>
          </div>
        </div>

        <!-- Botão Remover -->
        <button @click="removeItem(item)"
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

// Calcula o preço total
const totalPrice = computed(() => {
  return cartItems.value
    .reduce((total, item) => total + item.price, 0)
    .toFixed(2)
})

// Adiciona item ao carrinho quando recebe os parâmetros da URL
onMounted(() => {
  const params = route.query
  if (Object.keys(params).length > 0) {
    const newItem = {
      productCompanyId: Number(params.productCompanyId),
      productName: params.productName,
      companyName: params.companyName,
      price: Number(params.price),
      imageUrl: params.imageUrl
    }

    // Verifica se o item já existe no carrinho
    const exists = cartItems.value.some(
      item => item.productCompanyId === newItem.productCompanyId
    )

    if (!exists) {
      cartItems.value.push(newItem)
    }
  }
})

// Remove item do carrinho
function removeItem(item) {
  cartItems.value = cartItems.value.filter(
    i => i.productCompanyId !== item.productCompanyId
  )
}
</script>