<template>
  <div class="max-w-sm mx-auto mt-20 bg-white rounded-2xl shadow-lg p-8">
    <h2 class="text-2xl font-bold mb-6 text-center">Cadastro</h2>
    <form @submit.prevent="registrar">
      <label class="block mb-4">
        <span class="text-gray-700">Nome</span>
        <input
          v-model="nome"
          type="text"
          class="mt-1 block w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
          placeholder="Digite seu nome"
          required
        />
      </label>
      <label class="block mb-4">
        <span class="text-gray-700">E-mail</span>
        <input
          v-model="email"
          type="email"
          class="mt-1 block w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
          placeholder="Digite seu e-mail"
          required
        />
      </label>
      <label class="block mb-6">
        <span class="text-gray-700">Senha</span>
        <input
          v-model="senha"
          type="password"
          class="mt-1 block w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
          placeholder="Crie uma senha"
          required
        />
      </label>
      <button
        type="submit"
        class="w-full bg-blue-600 text-white font-semibold py-2 rounded-xl hover:bg-blue-700 transition"
      >
        Cadastrar
      </button>
    </form>
    <div class="text-center mt-6">
      <span class="text-gray-600">Já tem uma conta?</span>
      <RouterLink
        to="/login"
        class="text-blue-600 font-semibold cursor-pointer hover:underline ml-1"
      >
        Entrar
      </RouterLink>
    </div>
  </div>
</template>

<script setup>

</script>
