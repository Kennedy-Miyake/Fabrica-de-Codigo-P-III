<script setup>
import Navbar from './components/NavbarComponent.vue';
import Footer from './components/FooterComponent.vue';

</script>
<template>
  <div class="flex flex-col min-h-screen bg-[#0a1931]">
    <Navbar />
    <div class="main container mx-auto px-4 py-6">
      <router-view></router-view>
    </div>
  </div>
  <Footer />
</template>
