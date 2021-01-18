import { Component } from '@angular/core';
import { Input, Output, EventEmitter } from '@angular/core';

interface Product {
    price: number;
}

@Component({
    selector: 'app-product-alerts',
    templateUrl: './product-alerts.component.html',
})
export class ProductAlertsComponent {
    @Input() product: Product;
    @Output() notify = new EventEmitter();

    ngOnInit() { }
}