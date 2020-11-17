import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, AbstractControl } from '@angular/forms';
import { DiningExpense } from '../../models/dining-expense.model';
import { DiningExpenseService } from '../../services/dining-expense.service';
import { ResponseMessage } from 'src/app/model/response-message';
import { Router, ActivatedRoute } from '@angular/router';
import { ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { ExpenseVoucher } from '../../models/expense-voucher';
import { CommonMethod } from 'src/app/merp-common/common-method';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-dining-expense',
  templateUrl: './add-dining-expense.component.html',
  styleUrls: ['./add-dining-expense.component.css']
})
export class AddDiningExpenseComponent implements OnInit {
  diningExpenseForm: FormGroup;
  diningExpense: DiningExpense = new DiningExpense();
  diningExpenseAddData: any;
  reset: any[] = [];
  diningExpenseId: number;
  lstVoucher: ExpenseVoucher[];
  units: Array<string> = [];
  constructor(private formBuilder: FormBuilder, private diningExpenseService: DiningExpenseService,
    private router: Router, private activatedRoute: ActivatedRoute, public commonMethod: CommonMethod,
    private matSnackBar: MatSnackBar) {
    this.diningExpenseForm = this.formBuilder.group({
      id: [""],
      campusId: [''],
      expenseDate: ["", Validators.required],
      expenseHeadId: ["", Validators.required],
      isStockProduct: [false, Validators.required],
      productName: ["", Validators.required],
      rate: ["", Validators.required],
      qty: ["", Validators.required],
      unitName: ["", Validators.required],
      amount: ["", Validators.required],
      note: [""],
      unit: ["", Validators.required]
    })
  }
  ngOnInit() {
    if (this.commonMethod.showForInstituteAdmin()) {
      this.diningExpenseForm.get('campusId').setValidators(Validators.required);
    }
    this.diningExpenseForm.get('unit').valueChanges.subscribe(e => {
      this.updateValidity(e);
    });
    this.diningExpenseForm.get('productName').valueChanges.subscribe(e => {
      this.setProductUnit(e);
    });
    this.diningExpenseForm.get('isStockProduct').valueChanges.subscribe(e => {
      this.disableEnableUnitControl(e);
    });
    this.units = this.commonMethod.units;
    this.activatedRoute.paramMap.subscribe(param => {
      this.diningExpenseId = +param.get("id");
      if (this.diningExpenseId > 0) {
        this.diningExpenseService.SearchDiningExpense('"' + this.diningExpenseId + '"').subscribe((res: ResponseMessage) => {
          this.diningExpense = <DiningExpense>res.responseObj;
          this.diningExpenseForm.patchValue(this.diningExpense);
        })
      }
    })
    this.diningExpenseService.SearchDiningExpenseAddData("").subscribe((res: ResponseMessage) => {
      if (res.responseObj != null) {
        this.diningExpenseAddData = res.responseObj;
        this.units = this.units.concat(this.diningExpenseAddData.units).map((e, i, s) => e.toUpperCase()).filter((e, i, s) =>
          s.indexOf(e) == i).map((e, i, s) => e.charAt(0) + e.slice(1).toLowerCase());
      }
    });
    this.diningExpenseForm.get('rate').valueChanges.subscribe(e => {
      this.calculateAmmount();
    })
    this.diningExpenseForm.get('qty').valueChanges.subscribe(e => {
      this.calculateAmmount();
    })
  }
  setProductUnit(e) {
    const unitFormControl: AbstractControl = this.diningExpenseForm.get('unit');
    if (this.diningExpenseForm.get('isStockProduct').value && this.diningExpenseAddData) {
      const data = this.diningExpenseAddData.diningStockProducts.find(x => x.id == e);
      if (this.units.indexOf(data.unit) == -1) {
        this.units.push(data.unit);
      }
      unitFormControl.setValue(data.unit);
    }
  }
  disableEnableUnitControl(e) {
    const unitFormControl: AbstractControl = this.diningExpenseForm.get('unit');
    if (e) {
      unitFormControl.disable();
    }
    else {
      unitFormControl.enable();
    }
  }
  updateValidity(e) {
    if (e == 'x') {
      this.diningExpenseForm.get('unitName').setValidators(Validators.required);
      this.diningExpenseForm.get('unitName').updateValueAndValidity();
    }
    else {
      this.diningExpenseForm.get('unitName').clearValidators();
      this.diningExpenseForm.get('unitName').updateValueAndValidity();
    }
  }
  calculateAmmount() {
    let qty = this.diningExpenseForm.get('qty').value;
    let rate = this.diningExpenseForm.get('rate').value;
    this.diningExpenseForm.get('amount').setValue((qty * rate).toFixed(2));
  }
  onSubmit(formDirective: FormGroupDirective) {
    this.diningExpenseForm.markAllAsTouched();
    if (this.diningExpenseForm.valid) {
      let unit = this.diningExpenseForm.get('unit').value;
      if (unit == 'x') {
        this.diningExpenseForm.get('unit').setValue(this.diningExpenseForm.get('unitName').value);
      }
      this.diningExpense = this.diningExpenseForm.getRawValue();
      if (this.diningExpenseId > 0) {
        this.diningExpenseService.UpdateDiningExpense(this.diningExpense).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res, formDirective, true);
        })
      }
      else {
        this.diningExpenseService.SaveDiningExpense(this.diningExpense).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res, formDirective, false);
        })
      }
    }
  }
  workWithResponse(res: ResponseMessage, formDirective: FormGroupDirective, isUpdate: boolean) {
    if (res.statusCode == ResponseStatusCodeEnum.Success) {
      if (isUpdate) {
        this.router.navigateByUrl('/finance/diningExpense');
      }
      else {
        formDirective.resetForm();
        this.diningExpenseForm.reset();
        this.diningExpenseForm.get('isStockProduct').setValue(false);
      }
    }
    this.matSnackBar.open(res.message, "OK", { duration: 5000 })
  }
  getProductUnit() {
    // if (this.diningExpenseAddData != null) {
    //   let data = this.diningExpenseAddData.diningStockProducts.filter(e => e.id == this.diningExpenseForm.get('productName').value);
    //   if (data) {
    //     return data.productUnit;
    //   }
    // }
    return "product unit"
  }
}
