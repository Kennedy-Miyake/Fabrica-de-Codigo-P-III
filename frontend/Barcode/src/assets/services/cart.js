import api from './api' 

export function PostOrderItem() {
    return api.post(`/orderitem`);   
}
