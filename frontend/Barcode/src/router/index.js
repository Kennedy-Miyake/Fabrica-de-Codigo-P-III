/* src/router/index.js */
import { createRouter, createWebHistory } from 'vue-router'

import HomeView     from '../views/HomeView.vue'           
import BarcodeView  from '../views/BarcodeView.vue'
import RegisterProductView from '../views/RegisterProductView.vue'
import ProductDetailsView from '../views/ProductDetailsView.vue'

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
      path: '/admin/register-product',
      name: 'register-product',
      component: RegisterProductView,
    },
    {
      path: '/products/:barcode',
      name: 'ProductDetails',
      component: ProductDetailsView,
    }
  ],
})

export default router
