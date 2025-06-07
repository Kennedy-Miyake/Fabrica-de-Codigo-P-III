<template>
  <div
    class="bg-white shadow-lg rounded-2xl p-6 w-72 m-6 flex flex-col hover:shadow-2xl transition-shadow duration-300"
  >
    <svg xmlns="http://www.w3.org/2000/svg"fill="none"viewBox="0 0 24 24"stroke-width="1.5"stroke="currentColor"class="w-8 h-8 text-gray-700 ml-auto">
      <path stroke-linecap="round"stroke-linejoin="round"d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 0 0-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 0 0-16.536-1.84M7.5 14.25 5.106 5.272M6 20.25a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Zm12.75 0a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Z"/>
    </svg>
    <h3 class="mt-2 text-lg font-semibold text-gray-800 text-center">
      Imagem da Empresa
    </h3>
    <p class="mt-2 text-base text-gray-600 text-center">
      {{ company.company.name }}
    </p>
    <p class="mt-6 text-xl font-bold text-green-600 text-center">
      R${{ company.price }}
    </p>
    <button
      @click="AdicionadoAoCarrinho"
      class="mt-6 bg-green-600 hover:bg-green-700 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-200 mx-auto">
      Comprar
    </button>
  </div>
</template>


<script setup lang="ts">
import { Ref } from 'vue';
import { PostOrderItem } from '../assets/services/cart';

defineProps({
  company: Object
})
function AdicionadoAoCarrinho() {
  console.log(`Adicionado ao carrinho: Adicionou`);
}
</script>

<style scoped>
</style>