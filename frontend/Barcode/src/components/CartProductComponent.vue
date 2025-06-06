<template>
  <div class="flex items-center justify-between bg-neutral-50 rounded-xl p-4 border mb-2 shadow">
    <div class="flex items-center gap-4">
      <img :src="product.imageUrl" alt="Imagem" class="w-16 h-16 object-cover rounded-xl border" />
      <div>
        <div class="font-semibold text-lg text-neutral-800">{{ product.name }}</div>
        <div class="text-green-700 font-bold">R$ {{ product.price }}</div>
      </div>
    </div>
    <div class="flex items-center gap-2">
      <input
        type="number"
        class="w-16 border rounded text-center"
        min="1"
        v-model.number="localQuantity"
        @change="emitQuantityChange"
      />
      <button @click="$emit('remove')" class="ml-2 text-red-600 hover:text-red-800">
        🗑️
      </button>
    </div>
  </div>
</template>

