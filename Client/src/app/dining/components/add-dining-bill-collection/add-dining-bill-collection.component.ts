import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBillCollectionAddData, DiningBillCollection } from '../../models/dining-bill-collection.model';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { DiningBillCollectionService } from '../../services/dining-bill-collection.service';
import { CollectionMode, ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { DiningBillCollectionDetails } from '../../models/dining-bill-collection-details.model';
import { catchError, debounceTime, distinctUntilChanged, filter, map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs/internal/observable/of';
import { DiningBoarderDetails } from '../../models/dining-boarder.model';
import { DiningBoarderService } from '../../services/dining-boarder.service';
import { CommonMethod } from 'src/app/merp-common/common-method';

@Component({
  selector: 'app-add-dining-bill-collection',
  templateUrl: './add-dining-bill-collection.component.html',
  styleUrls: ['./add-dining-bill-collection.component.css']
})
export class AddDiningBillCollectionComponent implements OnInit {

  selectedMode: number;
  cashForm: FormGroup;
  cashChequeForm: FormGroup;
  accountPayeeChequeForm: FormGroup;
  depositToBankForm: FormGroup;
  mobileBankingForm: FormGroup;
  collectionDate: Date;
  diningBillCollectionAddData: DiningBillCollectionAddData;
  response: ResponseMessage = new ResponseMessage();
  totalFee;
  totalPayingFee = 0;
  reset: any[] = [];
  filteredOptions: DiningBoarderDetails[] = [];
  boarderId: string;
  boarderSearchKey: FormControl = new FormControl();

  constructor(private formBuilder: FormBuilder, private diningBillCollectionService: DiningBillCollectionService,
    private dataService: DataService, private router: Router, private diningBoarderService: DiningBoarderService,
    public commonMethod: CommonMethod) {
    this.cashForm = this.formBuilder.group({
      note: ['']
    });
    this.cashChequeForm = this.formBuilder.group({
      chequeNo: ['', [Validators.required]],
      chequeDate: ['', [Validators.required]],
      accountNo: ['', [Validators.required]],
      bankName: ['', [Validators.required]],
      branch: ['', [Validators.required]],
      note: ['']
    });
    this.accountPayeeChequeForm = this.formBuilder.group({
      chequeNo: ['', [Validators.required]],
      chequeDate: ['', [Validators.required]],
      transferDate: ['', [Validators.required]],
      accountNo: ['', [Validators.required]],
      bankName: ['', [Validators.required]],
      branch: ['', [Validators.required]],
      bankAccountId: ['', Validators.required],
      note: ['']
    });
    this.depositToBankForm = this.formBuilder.group({
      depositDate: ['', [Validators.required]],
      bankAccountId: ['', Validators.required],
      transectionNo: ['', [Validators.required]],
      note: ['']
    });
    this.mobileBankingForm = this.formBuilder.group({
      transferDate: ['', [Validators.required]],
      MobileBankingAccId: ['', Validators.required],
      transectionNo: ['', [Validators.required]],
      note: ['']
    });
  }
  ngOnInit() {
  }
  ngAfterViewInit() {
    var data = this.boarderSearchKey.valueChanges
      .pipe(
        map(e => e.trim()),
        filter(e => e.length > 2),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(key => this.diningBoarderService.SearchBoarderBySearchKey(key)
          .pipe(catchError(error => of(error)))),
      ).subscribe(e => {
        this.filteredOptions = e.responseObj.diningBoarders;
      });
  }
  getMode(modeId: number): boolean {
    if (this.selectedMode != undefined && CollectionMode[this.selectedMode] == CollectionMode[modeId]) {
      return true;
    }
    return false;
  }
  onInput() {
    this.isFound = false;
  }
  onMainSave() {
    alert("Please select a payment mode!")
  }
  getName(id: number) {
    let data = this.filteredOptions.find(e => e.id == id);
    return data.boarderNo + ", " + data.boarderName + ", " + data.mobile
  }
  isFound = false;
  isShow = false;
  getBorder(id: number) {
    this.isShow = true;
    this.diningBillCollectionAddData = new DiningBillCollectionAddData();
    this.diningBillCollectionService.SearchDiningBillCollectionAddData(JSON.stringify(id))
      .subscribe((res: ResponseMessage) => {
        if (res.responseObj != null) {
          this.isFound = false;
          this.diningBillCollectionAddData = <DiningBillCollectionAddData>res.responseObj;
          this.calculateTotla();
        }
        else {
          this.isFound = true;
        }
        this.isShow = false;
      })
  }
  calculatePayingFee(event: any, diningBillId: number) {
    this.isShow = false;
    this.totalPayingFee = 0;
    for (var i = 0; i < this.diningBillCollectionAddData.diningBillPayable.length; i++) {
      var fields = <HTMLInputElement>document.getElementById("inputVal" + i);
      let feePay = Number(fields.value);
      this.totalPayingFee += feePay;
      this.totalFee -= this.totalPayingFee;
    }
    let fieldValue = Number(event.target.value);
    this.diningBillCollectionAddData.diningBillPayable
      .filter(e => {
        if (e.diningBillId == diningBillId) {
          e.payingAmount = e.payable - fieldValue;
        }
      });
    this.calculateTotla();
  }
  calculateTotla() {
    this.totalFee = 0;
    for (var i = 0; i < this.diningBillCollectionAddData.diningBillPayable.length; i++) {
      this.totalFee += Number(this.diningBillCollectionAddData.diningBillPayable[i].payingAmount);
    }
  }
  saveInfo(instrument: string) {
    let diningBillCollection: DiningBillCollection = new DiningBillCollection();
    diningBillCollection.campusId = this.diningBillCollectionAddData.diningBorderDetails.campusId;
    diningBillCollection.boarderId = this.diningBillCollectionAddData.diningBorderDetails.id;
    diningBillCollection.paymentModeId = this.selectedMode;
    diningBillCollection.instrument = instrument == '' ? null : instrument;
    diningBillCollection.collectionDate = this.collectionDate;
    diningBillCollection.note = instrument == '' ? this.cashForm.get("note").value : null;
    diningBillCollection.amount = 0;
    let diningBillCollectionDetails = [];
    for (var i = 0; i < this.diningBillCollectionAddData.diningBillPayable.length; i++) {
      var fields = <HTMLInputElement>document.getElementById("inputVal" + i);
      let diningBill = new DiningBillCollectionDetails();
      diningBill.amount = Number(fields.value);
      diningBillCollection.amount += Number(fields.value);
      diningBill.diningBillId = this.diningBillCollectionAddData.diningBillPayable[i].diningBillId;
      diningBillCollectionDetails.push(diningBill);
    }
    diningBillCollection.diningBillCollectionDetails = diningBillCollectionDetails;
    this.diningBillCollectionService.SaveDiningBillCollection(JSON.stringify(diningBillCollection)).subscribe((res: ResponseMessage) => {
      this.response.message = res.message;
      this.response.statusCode = res.statusCode;
      if (res.statusCode == ResponseStatusCodeEnum.Success) {
        this.dataService.setValueToResponseMessageProperty(this.response);
        this.router.navigateByUrl("/finance/diningBillCollection");
      }
      else {
        this.reset.push({});
      }
    });
  }
  onCashSubmit() {
    let instrument;
    if (this.cashForm.valid) {
      instrument = '';
      this.saveInfo(instrument);
    }
  }
  onCashChequeSubmit() {
    let instrument;
    if (this.cashChequeForm.valid) {
      instrument = JSON.stringify(this.cashChequeForm.value);
      this.saveInfo(instrument);
    }
  }
  onaccountPayeeChequeSubmit() {
    let instrument;
    if (this.accountPayeeChequeForm.valid) {
      instrument = JSON.stringify(this.accountPayeeChequeForm.value);
      this.saveInfo(instrument);
    }
  }
  onDepositToBankSubmit() {
    let instrument;
    if (this.depositToBankForm.valid) {
      instrument = JSON.stringify(this.depositToBankForm.value);
      this.saveInfo(instrument);
    }
  }
  onMobileBankingSubmit() {
    let instrument;
    if (this.mobileBankingForm.valid) {
      instrument = JSON.stringify(this.mobileBankingForm.value);
      this.saveInfo(instrument);
    }
  }
}
