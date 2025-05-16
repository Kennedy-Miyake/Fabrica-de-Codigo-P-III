<template>
  <div class="fixed inset-0 z-50 flex">
    <div
      class="fixed inset-0 bg-black bg-opacity-50"
      @click="$emit('close')"
    ></div>

    <!-- Sidebar -->
    <div class="relative w-64 bg-white shadow-lg h-full z-50 transition-transform transform translate-x-0 fixed right-0">
      <div class="p-4">
        <h5 class="text-xl font-semibold text-gray-800">Menu</h5>
      </div>
      <nav class="p-4">
        <ul class="space-y-2">
          <li>
            <a href="#" class="block p-2 text-gray-700 hover:bg-gray-100 rounded">Cadastrar Produto</a>
          </li>
          <li>
            <a href="#" class="block p-2 text-gray-700 hover:bg-gray-100 rounded">Scanear Codigo de Barra</a>
          </li>
          <li>
            <a href="#" class="block p-2 text-gray-700 hover:bg-gray-100 rounded">Scanear Qr Code</a>
          </li>
        </ul>
      </nav>
    </div>
  </div>
</template>

<script>
export default {
  name: 'SideBarComponent',
};
</script>

<style scoped>
</style>