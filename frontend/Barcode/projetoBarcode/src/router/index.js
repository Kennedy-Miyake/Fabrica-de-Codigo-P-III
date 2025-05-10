import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SearchBarcCodesView from '../views/SearchBarcCodesView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
    },
    {
      path: '/search',
      name: 'barcode',
      component: SearchBarcCodesView,
    } 
  ],
})

export default router
