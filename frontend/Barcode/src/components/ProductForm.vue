<!-- src/components/ProductForm.vue -->
<template>
  <form class="">
    <!-- Campos do formulário -->
    <!-- Imagem do Produto -->
    <div>
      <label class="block mb-1 font-medium">Imagem do Produto</label>
      <input
        v-model="form.image"
        type="text"
        class="w-full border rounded"
        placeholder="URL da imagem"
        required
      />
    </div>

    <!-- Nome do Produto -->
    <div>
      <label class="block mb-1 font-medium">Nome do Produto</label>
      <input
        v-model="form.name"
        type="text"
        class="w-full border rounded"
        placeholder="Nome do produto"
        required
      />
    </div>

    <!-- Descrição do Produto -->
    <div>
      <label class="block mb-1 font-medium">Descrição do Produto</label>
      <textarea
        v-model="form.description"
        class="w-full border rounded"
        placeholder="Descrição do produto"
        rows="3"
        required
      ></textarea>
    </div>
  </form>
</template>

<script>
export default {
  name: 'ProductForm',
  data() {
    return {
      form: {
        image: '',
        name: '',
      }
    };
  },
  methods: {
    handleSubmit() {
      this.$emit('submit', ...this.form);
    }
  }
};
</script>

<style scoped>
</style>
