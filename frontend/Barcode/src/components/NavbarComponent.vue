<template>
  <div>
    <nav class="bg-[#0a1931] text-white py-4 px-8 flex justify-between items-center">
      <button @click="toggleSidebar" class="hover:underline">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 6.878V6a2.25 2.25 0 0 1 2.25-2.25h7.5A2.25 2.25 0 0 1 18 6v.878m-12 0c.235-.083.487-.128.75-.128h10.5c.263 0 .515.045.75.128m-12 0A2.25 2.25 0 0 0 4.5 9v.878m13.5-3A2.25 2.25 0 0 1 19.5 9v.878m0 0a2.246 2.246 0 0 0-.75-.128H5.25c-.263 0-.515.045-.75.128m15 0A2.25 2.25 0 0 1 21 12v6a2.25 2.25 0 0 1-2.25 2.25H5.25A2.25 2.25 0 0 1 3 18v-6c0-.98.626-1.813 1.5-2.122" />
        </svg>
      </button>

      <h1 class="text-lg font-bold">Buycode</h1>
    </nav>
    <SideBarComponent v-if="isSidebarOpen" @close="toggleSidebar" />
  </div>
</template>

<script>
import { isSidebarOpen, toggleSidebar } from '../assets/Services/SideBar.js' 
import SideBarComponent from './SideBarComponent.vue'

export default {
  components: {
    SideBarComponent,
  },
  setup() {
    return {
      isSidebarOpen,
      toggleSidebar,
    }
  },
}
</script>
