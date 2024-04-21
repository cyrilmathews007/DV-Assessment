import { Component } from '@angular/core';
import { ProductService } from '../product.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/_models/product';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {

  product: Product | undefined;
  error: string | undefined;
  
  constructor(private service: ProductService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getProductDetails();
  }

  getProductDetails(){
    let productId = this.route.snapshot.paramMap.get('id');
    
    if (!productId) return;
    
    this.service.getProduct(productId).subscribe({
      next: product => this.product = product,
      error: error => { this.error = error.message; console.log(error);}
    });
  }
}
