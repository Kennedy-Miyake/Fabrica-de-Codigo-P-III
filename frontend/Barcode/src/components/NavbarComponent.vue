<template>
  <div>
    <nav class="bg-red-600 text-white py-4 px-8 flex justify-between items-center">
      <h1 class="text-lg font-bold">Barcode Project</h1>
      <button @click="toggleSidebar" class="hover:underline">
        Menu
      </button>
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