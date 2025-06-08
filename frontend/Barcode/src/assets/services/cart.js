import api from './api'

export const cartService = {
    // Criar pedido
    async createOrder() {
        return await api.post('/order', {
            clientId: 1,
            status: 'PENDING'
        })
    },

    // Verificar pedido ativo
    async getActiveOrder() {
        try {
            const response = await api.get('/order/active')
            return response.data
        } catch (error) {
            if (error.response?.status === 404) {
                return null
            }
            throw error
        }
    },

    // Adicionar ao carrinho com criação automática de pedido
    async addToCart(data) {
        let orderId

        // Tenta obter pedido ativo ou cria novo
        try {
            const activeOrder = await this.getActiveOrder()
            orderId = activeOrder?.orderId

            if (!orderId) {
                const newOrder = await this.createOrder()
                orderId = newOrder.data.orderId
            }
        } catch (error) {
            const newOrder = await this.createOrder()
            orderId = newOrder.data.orderId
        }

        // Adiciona item ao pedido
        return api.post('/orderitem', {
            productCompanyId: data.productCompanyId,
            orderId: orderId,
            quantity: data.quantity || 1
        })
    },

    // Buscar itens do carrinho
    getCartItems() {
        return api.get('/orderitems')
    },

    // Remover item
    removeItem(itemId) {
        return api.delete(`/orderitem/${itemId}`)
    },

    // Atualizar quantidade 
    updateQuantity(itemId, quantity) {
        return api.put(`/orderitem/${itemId}`, {
            quantity: quantity,
            orderId: 1 
        })
    }
}
