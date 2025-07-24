import api from './api'

// Busca produto específico em uma empresa
export const getProductInCompany = async (companyId, productId) => {
    try {
        const response = await api.get(`/company/${companyId}/product/${productId}`)
        return response
    } catch (error) {
        console.error('Erro ao buscar produto:', error)
        return { data: null }
    }
}

// Busca todas as empresas
export const getAllCompanies = async () => {
    try {
        return await api.get('/companies')
    } catch (error) {
        console.error('Erro ao buscar empresas:', error)
        throw error
    }
}