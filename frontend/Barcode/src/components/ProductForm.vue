<!-- src/components/ProductForm.vue -->
<template>
  <form @submit.prevent="handleSubmit" class="max-x-md mx-auto mt-12 bg-neutral-900 border border-neutral-800 rounded-2xl shadow-2xl p-8 space-y-5">
    <!-- Campos do formulário -->
    <!-- Imagem do Produto -->
    <div>
      <label class="block mb-1 font-semibold text-neutral-300">Imagem do Produto</label>
      <input
        v-model="form.imageurl"
        type="text"
        class="w-full border border-neutral-700 bg-neutral-800 text-neutral-100 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-green-500 placeholder:text-neutral-500 transition"
        placeholder="URL da imagem"
        required
      />
    </div>

    <!-- Nome do Produto -->
    <div>
      <label class="block mb-1 font-semibold text-neutral-300">Nome do Produto</label>
      <input
        v-model="form.name"
        type="text"
        class="w-full border border-neutral-700 bg-neutral-800 text-neutral-100 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-green-500 placeholder:text-neutral-500 transition"
        placeholder="Nome do produto"
        required
      />
    </div>

    <!-- Descrição do Produto -->
    <div>
      <label class="block mb-1 font-semibold text-neutral-300">Descrição do Produto</label>
      <textarea
        v-model="form.description"
        class="w-full border border-neutral-700 bg-neutral-800 text-neutral-100 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-green-500 placeholder:text-neutral-500 transition resize-none"
        placeholder="Descrição do produto"
        rows="3"
        required
      ></textarea>
    </div>

    <!-- Código de Barras -->
    <div>
      <label class="block mb-1 font-semibold text-neutral-300">Código de Barras</label>
      <input
        v-model="form.barcode"
        type="text"
        class="w-full border border-neutral-700 bg-neutral-800 text-neutral-100 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-green-500 placeholder:text-neutral-500 transition"
        placeholder="Código de barras"
        required
      />
    </div>

    <!-- Botão de Enviar -->
    <div class="flex flex-col items-center">
      <button
        type="submit"
        class="mt-6 w-full bg-gradient-to-r from-green-500 to-lime-500 hover:from-green-600 hover:to-lime-600 text-neutral-900 font-bolt px-4 py-2 rounded-xl shadow-md hover:scale-105 transition-all duaration-200">
        Cadastrar
      </button>
    </div>
  </form>
</template>

<script>
export default {
  name: 'ProductForm',
  emits: ['submitForm'],
  data() {
    return {
      form: {
        imageurl: '',
        name: '',
        description: '',
        barcode: ''
      }
    };
  },
  methods: {
    handleSubmit() {
      this.$emit('submitForm', {...this.form });
    }
  }
};
</script>

<style scoped>
</style>
