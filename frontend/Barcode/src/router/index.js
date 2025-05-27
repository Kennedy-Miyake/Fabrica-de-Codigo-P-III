/* src/router/index.js */
import { createRouter, createWebHistory } from 'vue-router'

import HomeView     from '../views/HomeView.vue'           
import BarcodeView  from '../views/BarcodeView.vue'
import RegisterProductView from '../views/RegisterProductView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',            
      name: 'home',
      component: HomeView,
    },
    {
      path: '/scanner',     
      name: 'barcode',
      component: BarcodeView,
    },
    {
      path: '/product',    
      name: 'product',
      component: () => import('../views/ProductView.vue'),
    },
    {
      path: '/admin/register-product',
      name: 'register-product',
      component: RegisterProductView,
    }
  ],
})

export default router
