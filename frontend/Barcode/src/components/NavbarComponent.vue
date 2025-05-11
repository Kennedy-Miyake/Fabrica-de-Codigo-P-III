<template>
    <nav class="bg-red-600 text-white py-4 px-8 flex justify-between items-center">
        <h1 class="text-lg font-bold">Barcode Project</h1>
        <ul class="flex gap-6">
            <li>
                <router-link to="/" class="hover:underline">Scan</router-link>
            </li>
           
        </ul>
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
