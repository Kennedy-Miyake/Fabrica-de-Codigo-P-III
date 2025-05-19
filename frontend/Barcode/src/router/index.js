import { createRouter, createWebHistory } from 'vue-router'
import BarcodeView from '../views/BarcodeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'barcode',
      component: BarcodeView,
    },
    {
      path: '/product',
      name: 'product',
      component: () => import('../views/ProductView.vue')
    }
  ],
})

export default router
