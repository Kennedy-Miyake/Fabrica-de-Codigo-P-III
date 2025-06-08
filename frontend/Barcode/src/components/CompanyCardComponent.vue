<template>
  <div class="bg-white/90 w-[350px] rounded-xl shadow-lg p-8">
    <div class="flex flex-col items-center gap-4">
      <h3 class="font-semibold text-lg text-gray-800">{{ company.name }}</h3>
      <p class="text-xl font-bold text-green-600">R$ {{ company.price?.toFixed(2) }}</p>
      <p class="text-sm text-gray-600">Estoque: {{ company.stock }} unidades</p>

      <router-link :to="{
        name: 'cart',
        query: {
          productCompanyId: company.productCompanyId,
          companyId: company.companyId,
          companyName: company.name,
          productName: product.name,
          price: company.price,
          quantity: 1,
          stock: company.stock,
          imageUrl: product.imageUrl
        }
      }"
        class="w-full bg-green-600 hover:bg-green-700 disabled:bg-gray-400 text-white text-center font-semibold py-2 px-4 rounded-lg transition-colors">
        Adicionar ao Carrinho
      </router-link>
    </div>
  </div>
</template>

<script setup>
defineProps({
  company: {
    type: Object,
    required: true
  },
  product: {
    type: Object,
    required: true
  }
})
</script>

<style scoped></style>