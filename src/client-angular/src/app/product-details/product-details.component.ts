import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product, products } from '../products';
import { CartService } from '../services/cart.service';

@Component({
    selector: 'app-product-details',
    templateUrl: './product-details.component.html',
})
export class ProductDetailsComponent implements OnInit {
    product: Product;

    constructor(
        private route: ActivatedRoute,
        private cartService: CartService
    ) { }

    ngOnInit() {
        const { paramMap } = this.route.snapshot;
        const productId = Number(paramMap.get('productId'));
        this.product = products.find(product => product.id === productId);
    }

    addToCart(product: Product) {
        this.cartService.addToCart(product);
        window.alert('Your product has been added to the cart!');
    }
}