import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product, products } from '../products';

@Component({
    selector: 'app-product-details',
    templateUrl: './product-details.component.html',
})
export class ProductDetailsComponent implements OnInit {
    product: Product;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        const { paramMap } = this.route.snapshot;
        const productId = Number(paramMap.get('productId'));
        this.product = products.find(product => product.id === productId);
    }
}