import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { IBasket } from '../shared/models/ibasket';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product';
import { IBasketItem } from '../shared/models/ibasketitem';
import { Basket } from '../shared/models/basket';
import { IBasketTotals } from '../shared/models/ibaskettotals';


@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl: string = environment.apiUrl;
  private basketSource: BehaviorSubject<IBasket> = new BehaviorSubject<IBasket>(null);
  private basketTotalSource: BehaviorSubject<IBasketTotals> = new BehaviorSubject<IBasketTotals>(null);
  basket$: Observable<IBasket> = this.basketSource.asObservable();
  basketTotals$: Observable<IBasketTotals> = this.basketTotalSource.asObservable();

  constructor (private http: HttpClient) { }

  getBasket(basketId: string) {
    return this.http.get(this.baseUrl + 'basket?basketId=' + basketId)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotal();
        })
      );
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[itemIndex].quantity > 1) {
      basket.items[itemIndex].quantity--;
    }
    else {
      this.removeItemFromBasket(item)
    }
    this.setBasket(basket);
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotal();
    }, error => {
      console.log(error);
    });
  }

  getCurrentBasketValue(): IBasket {
    return this.basketSource.value;
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(i => i.id !== item.id);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      }
      else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: IBasket) {
    this.http.delete(this.baseUrl + 'basket?basketId=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });

  }

  addItemToBasket(item: IProduct, qty: number = 1): void {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, qty);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, qty);
    this.setBasket(basket);
  }



  private calculateTotal(): void {
    const basket = this.getCurrentBasketValue();
    const shippingCharge = 0;
    const codCharge = 0;
    const subtotal = basket.items.reduce((a, b) => (b.quantity * b.price) + a, 0);
    const total = shippingCharge + codCharge + subtotal;
    this.basketTotalSource.next({ shippingCharge, codCharge, subtotal, total });
  }

  private addOrUpdateItem(items: IBasketItem[], item: IBasketItem, qty: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === item.id);
    if (index === -1) {
      item.quantity = qty;
      items.push(item);
    }
    else {
      items[index].quantity += qty;
    }
    return items;
  }

  private createBasket(): IBasket {
    const cart = new Basket();
    localStorage.setItem('basket_id', cart.id);
    return cart;
  }

  private mapProductItemToBasketItem(item: IProduct, qty: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      pictureUrl: item.pictureUrl,
      price: item.price,
      brand: item.productBrand,
      type: item.productType,
      quantity: qty
    };
  }
}
