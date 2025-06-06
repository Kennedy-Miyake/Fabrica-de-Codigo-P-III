<template>
  <section class="max-w-2xl mx-auto mt-20 bg-white/90 rounded-2xl shadow-xl p-8 border border-neutral-200 relative">
    <header class="flex justify-between items-center mb-8">
      <h2 class="text-3xl font-bold text-neutral-800">Seu Carrinho de Compras</h2>
      <button class="bg-green-600 hover:bg-green-700 text-white px-5 py-2 rounded-xl font-semibold shadow">
        Finalizar Compra
      </button>
    </header>
   </section> 
</template>   

<script setup>
import { ref, computed } from 'vue'
import CartProductComponent from '../components/CartProductComponent.vue'
import RemoveComponent from '../components/RemoveComponent.vue'

// Exemplo de produtos do carrinho
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

const totalPrice = computed(() =>
  cartProducts.value.reduce((acc, p) => acc + p.price * p.quantity, 0).toFixed(2)
)

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
</script>