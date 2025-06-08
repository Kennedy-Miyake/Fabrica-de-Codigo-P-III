<template>
  <section
    class="max-w-6xl mx-auto mt-20 grid grid-cols-1 lg:grid-cols-3 gap-6 bg-white/90 backdrop-blur-md rounded-2xl shadow-2xl p-8 border border-neutral-200 relative"
  >
    <!-- Header (span total das colunas em desktop) -->
    <header class="flex justify-between items-center mb-8 lg:col-span-3 border-b pb-4">
      <h2 class="text-3xl font-extrabold text-neutral-800">
        Seu Carrinho de Compras
      </h2>
    </header>

    <!-- Lista de Produtos (2/3 do grid em desktop) -->
    <div class="space-y-6 lg:col-span-2">
      <div
        v-for="product in cartProducts"
        :key="product.id"
        class="bg-white rounded-xl shadow hover:shadow-lg transition-shadow duration-200 p-4 flex items-center gap-4"
      >
        <CartProductComponent
          :product="product"
          @remove="openRemoveModal(product)"
          @quantityChange="changeQuantity(product, $event)"
        />
      </div>
    </div>

    <!-- Resumo (1/3 do grid em desktop) -->
    <div class="lg:col-span-1 bg-gray-50 rounded-xl p-6 shadow-md sticky top-24">
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
      <button
        @click="finalize"
        class="mt-6 w-full bg-green-500 hover:bg-green-800 text-white py-3 rounded-lg font-semibold shadow transition-colors duration-200"
      >
        Finalizar Compra
      </button>
    </div>

    <!-- Modal Remover -->
    <RemoveComponent
      v-if="modalOpen"
      :product="selectedProduct"
      @confirm="removeProduct"
      @cancel="modalOpen = false"
      class="absolute inset-0 flex items-center justify-center bg-black/50"
    />

    <!-- POP-UP 1: Loading -->
    <div
      v-if="loading"
      class="fixed inset-1 flex flex-col items-center justify-center rounded-3xl bg-neutral-500/50 z-50 overflow-hidden"
    >
      <div class="bg-white p-6 rounded-xl w-80 text-center">
        <p class="mb-3 font-semibold text-lg">Finalizando compra</p>
        <div class="w-full bg-gray-200 rounded-full h-3 overflow-hidden">
          <div
            class="h-3 bg-green-400 transition-all duration-200"
            :style="{ width: progress + '%' }"
          ></div>
        </div>
      </div>
    </div>

    <!-- POP-UP 2: Success -->
    <div
      v-if="success"
      class="fixed inset-1 flex flex-col items-center justify-center rounded-3xl bg-green-600/50 z-50 overflow-hidden"
    >
      <div class="bg-white p-6 rounded-xl w-96 text-center space-y-2">
        <p class="font-semibold text-lg text-green-700">Compra finalizada!</p>
        <p class="text-base font-semibold text-lg text-gray-600">Boleto encaminhado para o seu e-mail !</p>
        <p class="text-base text-gray-600">Obrigado, volte sempre 😉</p>
      </div>

    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import CartProductComponent from '../components/CartProductComponent.vue'
import RemoveComponent from '../components/RemoveComponent.vue'

// Estado do carrinho
const cartProducts = ref([
  {
    id: 1,
    name: 'Produto Exemplo 1',
    price: 25.50,
    quantity: 2,
    imageUrl: 'https://via.placeholder.com/64'
  },
  {
    id: 2,
    name: 'Produto Exemplo 2',
    price: 13.90,
    quantity: 1,
    imageUrl: 'https://via.placeholder.com/64'
  }
])

const modalOpen = ref(false)
const selectedProduct = ref(null)

// Estados dos pop-ups
const loading = ref(false)
const success = ref(false)
const progress = ref(0)

// Cálculo do total
const totalPrice = computed(() =>
  cartProducts.value.reduce((acc, p) => acc + p.price * p.quantity, 0).toFixed(2)
)

// Funções do carrinho
function openRemoveModal(product) {
  selectedProduct.value = product
  modalOpen.value = true
}
function removeProduct() {
  cartProducts.value = cartProducts.value.filter(p => p.id !== selectedProduct.value.id)
  modalOpen.value = false
}
function changeQuantity(product, newQty) {
  product.quantity = newQty
}

// Função de finalizar compra com dois pop-ups
function finalize() {
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
      }, 2000)
    }
  }, 200)
}
</script>
