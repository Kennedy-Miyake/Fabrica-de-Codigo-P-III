<template>
  <div>
    <nav class="bg-[#202432] text-white py-4 px-8 flex items-center justify-between">
      <!-- Esquerda: Botão do menu + nome -->
      <div class="flex items-center gap-3 min-w-max">
        <button @click="toggleSidebar" class="hover:underline">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 6.878V6a2.25 2.25 0 0 1 2.25-2.25h7.5A2.25 2.25 0 0 1 18 6v.878m-12 0c.235-.083.487-.128.75-.128h10.5c.263 0 .515.045.75.128m-12 0A2.25 2.25 0 0 0 4.5 9v.878m13.5-3A2.25 2.25 0 0 1 19.5 9v.878m0 0a2.246 2.246 0 0 0-.75-.128H5.25c-.263 0-.515.045-.75.128m15 0A2.25 2.25 0 0 1 21 12v6a2.25 2.25 0 0 1-2.25 2.25H5.25A2.25 2.25 0 0 1 3 18v-6c0-.98.626-1.813 1.5-2.122" />
          </svg>
        </button>
        <h1 class="text-lg font-bold tracking-wide">
          <router-link to="/">BuyCode</router-link>
        </h1>
      </div>

      <!-- Centro: Barra de pesquisa -->
      <div class="flex-1 flex justify-center">
        <input
          v-model="searchQuery"
          @keyup.enter="onSearch"
          type="text"
          placeholder="Digite aqui seu código de barras"
          class="bg-transparent border-2 border-[#4facfe] text-white placeholder-[#4facfe] font-mono text-xl rounded-xl px-4 py-2 w-full max-w-xl shadow-[0_0_0_2px_#222_inset] focus:ring-2 focus:ring-[#4facfe] transition"
        />
      </div>

        <router-link to="/cart" class="hover:text-emerald-400 transition">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z" />
            </svg>
        </router-link>

      <!-- Direita: Login -->
      <div class="flex items-center min-w-max">
        <router-link to="/login" class="ml-4 transition">
          <button class="bg-[#4facfe] text-white font-semibold py-2 px-4 rounded-3xl hover:bg-[#8ce7fe] transition-colors shadow-md">
            Login
          </button>
        </router-link>
      </div>
      <div class="flex items-center min-w-max">
        <router-link to="/register" class="ml-4 transition">
          <button class="bg-[#4facfe] text-white font-semibold py-2 px-4 rounded-3xl  hover:bg-[#8ce7fe] transition-colors shadow-md">
            Regiter
          </button>
        </router-link>
        
      </div>
    </nav>
    <SideBarComponent v-if="isSidebarOpen" @close="toggleSidebar" />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { isSidebarOpen, toggleSidebar } from '../assets/services/SideBar.js'
import SideBarComponent from './SideBarComponent.vue'

const searchQuery = ref('')
const router = useRouter()

function onSearch() {
  const code = searchQuery.value.trim()
  if(!code) return

  router.push({ name: 'ProductDetails', params: { barcode: code } })

  searchQuery.value = ''
}
</script>
