import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';

@Component({
    selector: 'app-shipping',
    templateUrl: './shipping.component.html',
})
export class ShippingComponent {
    shippingCosts = this.cartService.getShippingPrices();

    constructor(private cartService: CartService) { }
}