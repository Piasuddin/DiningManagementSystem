import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormGroupDirective } from '@angular/forms';
import { ResponseMessage } from 'src/app/model/response-message';
import { StockProduct } from '../../models/stock-product.model';
import { StockProductService } from '../../services/stock-product.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ResponseStatusCodeEnum, StockProductType } from 'src/app/enums/Enums';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonMethod } from 'src/app/merp-common/common-method';

@Component({
  selector: 'app-add-dining-stock-product',
  templateUrl: './add-dining-stock-product.component.html',
  styleUrls: ['./add-dining-stock-product.component.css']
})
export class AddDiningStockProductComponent implements OnInit {
  diningStockProductForm: FormGroup;
  response: ResponseMessage = new ResponseMessage();
  diningStockProduct: StockProduct = new StockProduct();
  lstDiningStockProduct: any[] = [];
  reset: any = [{}];
  diningStockProductId: number;
  constructor(private _formBuilder: FormBuilder, private diningStockProductService: StockProductService,
    public commonMethod: CommonMethod, private router: Router, private activatedRoute: ActivatedRoute,
    private matSnackBar: MatSnackBar) {
    this.diningStockProductForm = this._formBuilder.group({
      id: [""],
      campusId: [''],
      productName: ["", Validators.required],
      brand: ["", Validators.required],
      unit: ["", Validators.required],
      attributeAndValue: this._formBuilder.array([
        this.createAttributeAndValueForm()
      ])
    })
  }
  ngOnInit() {
    if (this.commonMethod.showForInstituteAdmin()) {
      this.diningStockProductForm.get('campusId').setValidators(Validators.required);
    }
    this.diningStockProductService.SearchStockProduct(JSON.stringify({ id: 0, type: StockProductType.DiningStockProduct }))
      .subscribe((res: ResponseMessage) => {
        this.lstDiningStockProduct = <StockProduct[]>res.responseObj;
        this.activatedRoute.paramMap.subscribe(param => {
          this.diningStockProductId = +param.get("id");
          if (this.diningStockProductId > 0) {
            if (this.lstDiningStockProduct.length > 0) {
              let data = this.lstDiningStockProduct.find(e => e.id == this.diningStockProductId);
              this.diningStockProductForm.patchValue({
                productName: data.productName,
                brand: data.brand,
                unit: data.unit,
              })
              this.assignDataToAttributeAndValueForm(data.attributeAndValue);
            }
          }
        })
      })
  }
  assignDataToAttributeAndValueForm(attributeAndValue: string) {
    let data = JSON.parse(attributeAndValue);
    let attributeAndValueFormArray = <FormArray>this.diningStockProductForm.get('attributeAndValue');
    if (data.length > 0) {
      if (attributeAndValueFormArray.length < data.length) {
        for (let i = 1; i < data.length; i++) {
          this.onAddMoreAttributeAndValue();
        }
      }
      attributeAndValueFormArray.patchValue(data);
    }
  }
  createAttributeAndValueForm(): FormGroup {
    return this._formBuilder.group({
      attribute: ["", Validators.required],
      value: ["", Validators.required]
    })
  }
  onAddMoreAttributeAndValue() {
    (<FormArray>this.diningStockProductForm.get('attributeAndValue')).push(this.createAttributeAndValueForm());
  }
  onRemoveMoreAttributeAndValue(index: number) {
    (<FormArray>this.diningStockProductForm.get('attributeAndValue')).removeAt(index);
  }
  onSubmit(formDirective: FormGroupDirective) {
    this.diningStockProductForm.markAllAsTouched();
    if (this.diningStockProductForm.valid) {
      this.diningStockProduct = this.diningStockProductForm.value;
      this.diningStockProduct.stockProductType = StockProductType.DiningStockProduct;
      this.diningStockProduct.attributeAndValue = JSON.stringify(this.diningStockProductForm.get('attributeAndValue').value);
      if (!this.diningStockProductService.checkIfStockProductAlreadyExists(this.lstDiningStockProduct, this.diningStockProduct)) {
        if (this.diningStockProductId > 0) {
          this.diningStockProduct.id = this.diningStockProductId;
          this.diningStockProductService.UpdateStockProduct(this.diningStockProduct).subscribe((res: ResponseMessage) => {
            this.workWithResponse(res, formDirective, true);
          })
        }
        else {
          if (this.diningStockProductForm.valid) {
            this.diningStockProductService.SaveStockProduct(this.diningStockProduct).subscribe((res: ResponseMessage) => {
              this.workWithResponse(res, formDirective, false);
            })
          }

        }
      }
      else {
        this.matSnackBar.open("Already exists a same product with these information",
          "GOT IT", { duration: 10000 })
      }
    }
  }
  workWithResponse(res: ResponseMessage, formDirective: FormGroupDirective, isUpdate: boolean) {
    if (res.statusCode == ResponseStatusCodeEnum.Success) {
      if (isUpdate) {
        this.router.navigateByUrl("/finance/diningStockProduct");
      }
      else {
        formDirective.resetForm();
        this.diningStockProductForm.reset();
        (<FormArray>this.diningStockProductForm.get('attributeAndValue')).clear();
        this.onAddMoreAttributeAndValue();
      }
    }
    this.matSnackBar.open(res.message, "OK", { duration: 5000 })
  }
}