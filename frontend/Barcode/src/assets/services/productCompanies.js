import api from './api'

export function getProductCompaniesByBarcode(barcode) {
    return api.get('/companies/product/${barcode}');
}