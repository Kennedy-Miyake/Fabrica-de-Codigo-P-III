<template>
  <footer class="bg-[#0a1931] w-full text-white py-6 px-2 text-center relative">
    <div class="flex flex-col items-center space-y-2">
    <p class="text-sm font-bold">{{ new Date().getFullYear() }} Barcode-Len</p>
    <nav class="flex space-x-3 text-xs">
        <a href="/about" class="hover:underline">About</a>
        <span>|</span>
        <a href="/privacy" class="hover:underline">Privacy</a>
        <span>|</span>
        <a href="/contact" class="hover:underline">Contact</a>
    </nav>
    <div class="flex space-x-4 mt-2">
        <a href="https://www.instagram.com/oficialtft/" target="_blank" rel="noopener" aria-label="Instagram">
            <img src="../assets/Services/img/icons8-instagram.svg" alt="Instagram" class="w-12 h-12">
        </a>
    </div>
      <p class="text-xs italic">TECNOLOGIA COM PROPOSITO.</p>
      <p class="text-[13px] mt-1">Desenvolvido por equipe TFT</p>
    </div>
    <button 
      @click="scrollToTop"   
      class="absolute right-4 bottom-4 bg-white bg-opacity-10 hover:bg-opacity-20 text-white p-2 rounded-full transition"
      aria-label="Voltar ao topo">
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
        <path stroke-linecap="round" stroke-linejoin="round" d="m9 9 6-6m0 0 6 6m-6-6v12a6 6 0 0 1-12 0v-3" />
      </svg>
    </button>
  </footer>
</template>

<script setup>
function scrollToTop() {
  window.scrollTo({ top: 0, behavior: 'smooth' });
}
</script>
