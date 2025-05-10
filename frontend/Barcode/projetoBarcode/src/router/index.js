import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SearchBarcCodesView from '../views/SearchBarcCodesView.vue'

const routes = [
  { path: '/',      name: 'home',    component: HomeView },
  { path: '/about', name: 'about',   component: () => import('../views/AboutView.vue') },
  { path: '/search', name: 'barcode', component: SearchBarcCodesView }
]

export default createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})
