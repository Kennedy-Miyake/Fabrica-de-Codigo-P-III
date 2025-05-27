<template>
  <div class="max-w-lg mx-auto p-4">
    <h1 class="text-3xl font-bold mb-4 text-center">Cadastrar Produto</h1>
    <ProductForm @submitForm="onSubmit" :loading="loading" />
    <p v-if="message" :class="messageColor" class="mt-4">{{ message }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import ProductForm from '../components/ProductForm.vue';
import { createProduct } from '../assets/services/products.js';

const loading = ref(false);
const message = ref('');
const messageColor = ref('');

async function onSubmit(form) {
  loading.value = true;
  message.value = '';

  const dto = {
    Name:        form.name,
    Description: form.description,
    ImageUrl:    form.imageurl,
    BarCode:     form.barcode
  };

  try {
    await createProduct(dto);
    message.value = 'Produto cadastrado com sucesso!';
    messageColor.value = 'text-green-500';
  } catch (err) {
    console.error(err);
    message.value = 'Erro ao cadastrar: ' + (err.response?.data ?? err.message);
    messageColor.value = 'text-red-500';
  } finally {
    loading.value = false;
  }
}
</script>