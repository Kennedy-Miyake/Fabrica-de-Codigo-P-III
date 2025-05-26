<template>
  <h1 class="flex flex-col text-3xl font-bold mb-4 items-center justify-center">Cadastrar Produto</h1>
  <div class="max-w-lg mx-auto p-4">
    <ProductForm @submit="onSubmit" />
  </div>
</template>

<script>
import ProductForm from '../components/ProductForm.vue'

export default {
  name: 'RegisterProductView',
  components: {
    ProductForm
  },
  methods: {
    onSubmit(formData) {
      console.log('Form submitted', formData);
    }
  }
}
</script>