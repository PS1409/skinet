import { Component, OnInit, Input } from '@angular/core';
import { ShopService } from '../shop.service';
import { IProduct } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  id: number;
  product: IProduct;
  quantity = 1;
  constructor (private shopService: ShopService
    , private activatedRoute: ActivatedRoute
    , private bcService: BreadcrumbService
    , private basketService: BasketService) {
    this.bcService.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.id = +this.activatedRoute.snapshot.paramMap.get('id');
    this.onLoadProduct();
  }

  onLoadProduct(): void {
    this.shopService.getProduct(this.id).subscribe((response: IProduct) => {
      this.product = response;
      this.bcService.set('@productDetails', response.name);
    }, error => {
      console.log(error);
    });
  }

  addItemTobasket(): void {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementItemQuantity(): void {
    this.quantity++;
  }

  decreaseItemQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }

  }
}