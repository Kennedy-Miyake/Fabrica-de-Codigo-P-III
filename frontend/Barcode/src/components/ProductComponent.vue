<template>
    <div>
      <p v-if="loading">Carregando...</p>
      <p v-else-if="error">{{ error }}</p>
      <div v-else-if="product">
        <h2>{{ product.name }}</h2>
        <p>{{ product.description }}</p>
        <p>Código de barras: {{ product.barcode }}</p>
      </div>
      <p v-else>Nenhum produto encontrado.</p>
    </div>
</template>
  
