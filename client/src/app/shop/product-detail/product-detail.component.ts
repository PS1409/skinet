import { Component, OnInit, Input } from '@angular/core';
import { ShopService } from '../shop.service';
import { IProduct } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
   id: number;
  product: IProduct;
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = +this.activatedRoute.snapshot.paramMap.get('id');
    this.onLoadProduct();
  }

  onLoadProduct(): void {
    this.shopService.getProduct(this.id).subscribe((response: IProduct) => {
      this.product = response;
    }, error => {
      console.log(error);
    });
  }
}
