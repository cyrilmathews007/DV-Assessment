import { Component } from '@angular/core';
import { ProductService } from './product.service';
import { Product } from '../_models/product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  
  products: Product[] | undefined;
  error: string | undefined;


  constructor(private service: ProductService) { }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(){
    this.service.getProducts().subscribe({
      next: products => this.products = products,
      error: error => { this.error = error.message; console.log(error);}
    });
  }

  reloadPage() {
    window.location.reload();
 }
}
