import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket } from '../shared/models/ibasket';
import { BasketService } from './basket.service';
import { IBasketItem } from '../shared/models/ibasketitem';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {

   basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  removeBasketItem(item: IBasketItem): void {
    this.basketService.removeItemFromBasket(item);
  }

  increaseBasketItemQuantity(item: IBasketItem): void {
   this.basketService.incrementItemQuantity(item);
  }

  decreaseBasketItemQuantity(item: IBasketItem): void {
   this.basketService.decrementItemQuantity(item);
  }
}
