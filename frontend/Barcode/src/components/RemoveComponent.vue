<template>
  <div class="fixed inset-0 z-50 bg-black/40 flex items-center justify-center">
    <div class="bg-white rounded-2xl p-8 shadow-xl max-w-md w-full">
      <h3 class="text-2xl font-bold mb-4">Remover Produto</h3>
      <p class="mb-6">Você tem certeza que deseja remover este produto do carrinho?</p>
      <div class="flex justify-end gap-3">
        <button class="bg-neutral-200 rounded px-5 py-2 font-semibold" @click="$emit('cancel')">Cancelar</button>
        <button class="bg-red-600 text-white rounded px-5 py-2 font-semibold" @click="$emit('confirm')">Remover</button>
      </div>
    </div>
  </div>
</template>
