import api from './api';

export function createProduct(dto) {
    return api.post('/products', dto);
}

export function getAllProducts() {
    return api.get('/products');
}

export function getProductByBarcode(barcode) {
    return api.get(`/product/${barcode}`);
}

export function deleteProduct(id) {
    return api.delete(`/product/${id}`);
}