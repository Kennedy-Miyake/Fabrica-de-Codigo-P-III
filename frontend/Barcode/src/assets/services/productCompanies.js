import axios from 'axios'

const API_URL = 'http://localhost:8080/api/v1'

// Busca produto específico em uma empresa
export const getProductInCompany = async (companyId, productId) => {
    return axios.get(`${API_URL}/company/${companyId}/product/${productId}`)
}

// Busca todas as empresas
export const getAllCompanies = async () => {
    return axios.get(`${API_URL}/companies`)
}