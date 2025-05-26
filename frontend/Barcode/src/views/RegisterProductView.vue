<template>
  <h1 class="flex flex-col text-2xl font-semibold mb-4 items-center justify-center">Cadastrar Produto</h1>
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