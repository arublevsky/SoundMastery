import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../products';

interface ShippingItem {
    type: string;
    price: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
    items: Product[] = [];

    constructor(private http: HttpClient) { }

    addToCart(product: Product) {
        this.items.push(product);
    }

    getItems() {
        return this.items;
    }

    clearCart() {
        this.items = [];
        return this.items;
    }

    getShippingPrices() {
        return this.http.get<Observable<ShippingItem[]>>('/assets/shipping.json');
    }
}