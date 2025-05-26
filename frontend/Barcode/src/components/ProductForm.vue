<!-- src/components/ProductForm.vue -->
<template>
  <form class="">
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
