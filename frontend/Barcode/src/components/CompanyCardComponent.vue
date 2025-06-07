<template>
  <div class="bg-white/90 w-[300px] rounded-xl shadow-lg p-6">
    <div class="flex flex-col items-center gap-4">
      <h3 class="font-semibold text-lg text-gray-800">{{ company.name }}</h3>
      <p class="text-gray-600">{{ company.address }}</p>
      <p class="text-xl font-bold text-green-600">R$ {{ company.price?.toFixed(2) }}</p>
      <p class="text-sm text-gray-600">Estoque: {{ company.stock }} unidades</p>

      <button @click="handleAddToCart" :disabled="loading || company.stock === 0"
        class="w-full bg-green-600 hover:bg-green-700 disabled:bg-gray-400 text-white font-semibold py-2 px-4 rounded-lg transition-colors">
        {{ loading ? 'Adicionando...' : company.stock === 0 ? 'Sem Estoque' : 'Comprar' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { PostOrderItem } from '../assets/services/cart'

const props = defineProps({
  company: {
    type: Object,
    required: true
  }
})

const loading = ref(false)

async function handleAddToCart() {
  if (loading.value || props.company.stock === 0) return

  loading.value = true
  try {
    await PostOrderItem(props.company.companyId, props.productId, 1)
    console.log('Produto adicionado ao carrinho!')
  } catch (error) {
    console.error('Erro ao adicionar ao carrinho:', error)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped></style>