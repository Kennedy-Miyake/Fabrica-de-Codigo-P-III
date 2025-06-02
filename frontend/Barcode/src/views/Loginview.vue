<template>
  <div class="max-w-sm mx-auto mt-20 bg-white rounded-2xl shadow-lg p-8">
    <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
    <form @submit.prevent="login">
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
          placeholder="Digite sua senha"
          required
        />
      </label>
      <button
        type="submit"
        class="w-full bg-blue-600 text-white font-semibold py-2 rounded-xl hover:bg-blue-700 transition"
      >
        Entrar
      </button>
    </form>
    <div class="text-center mt-6">
      <span class="text-gray-600">Não tem uma conta?</span>
      <span
        class="text-blue-600 font-semibold cursor-pointer hover:underline ml-1"
        @click="registrar"
      >
        Cadastre-se
      </span>
    </div>
  </div>
</template>

<script setup>

</script>
