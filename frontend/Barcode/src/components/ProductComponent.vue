<template>
    <div>
      <p v-if="loading">Carregando...</p>
      <p v-else-if="error">{{ error }}</p>
      <div v-else-if="product">

        <h2>{{ product.name }}</h2>
        <p>{{ product.description }}</p>
        <p>Código de barras: {{ product.barCode }}</p>
      </div>

      <p v-else>Nenhum produto encontrado.</p>
    </div>
</template>
  
<script>
export default {
  data() {
    return {
      product: null,
      loading: true,
      error: null
    }
  },
  mounted() {
    const code = this.$route.query.code
    if (code) {
      this.fetchProduct(code)
    } else {
      this.loading = false
    }
  },
  methods: {
    async fetchProduct(code) {
      try {
        const response = await fetch(`http://localhost:8080/products/${code}`)
        if (!response.ok) throw new Error('Produto não encontrado')
        const data = await response.json()
        this.product = data
      } catch (e) {
        this.error = e.message
      } finally {
        this.loading = false
      }
    }
  }
}
</script>
